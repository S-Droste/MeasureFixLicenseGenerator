using Standard.Licensing;
using Standard.Licensing.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management;

namespace MeasureFixLicenseGenerator
{
    public static class LicenseGenerator
    {
        private const string PassPhrase = "MeasureFixSecure2024!";
        private const string PrivateKeyFile = "mf_private.key";

        // Öffentlicher Schlüssel – muss in MeasureFixOS eingetragen sein
        public static string PublicKey { get; private set; } = "";
        private static string _privateKey = "";

        /// <summary>
        /// Schlüsselpaar laden oder neu erstellen
        /// </summary>
        public static void InitializeKeys()
        {
            if (File.Exists(PrivateKeyFile))
            {
                // Bestehenden Schlüssel laden
                var lines = File.ReadAllLines(PrivateKeyFile);
                _privateKey = lines[0];
                PublicKey = lines[1];
            }
            else
            {
                // Neues Schlüsselpaar generieren
                var keyGenerator = KeyGenerator.Create();
                var keyPair = keyGenerator.GenerateKeyPair();
                _privateKey = keyPair.ToEncryptedPrivateKeyString(PassPhrase);
                PublicKey = keyPair.ToPublicKeyString();

                // Schlüssel speichern
                File.WriteAllLines(PrivateKeyFile, new[] { _privateKey, PublicKey });
            }
        }

        /// <summary>
        /// Lizenz erstellen und als .mfx Datei speichern
        /// </summary>
        public static void CreateLicense(LicenseData data, string outputPath)
        {
            var license = License.New()
                .WithUniqueIdentifier(Guid.NewGuid())
                .As(LicenseType.Standard)
                .ExpiresAt(data.MaintenanceDate)
                .LicensedTo(customer =>
                {
                    customer.Name = data.ContactPerson;
                    customer.Email = data.Email;
                    customer.Company = data.CompanyName;
                })
                .WithAdditionalAttributes(new Dictionary<string, string>
                {
                    { "COMPANY",          data.CompanyName },
                    { "ADDRESS",          data.Address },
                    { "CONTACT",          data.ContactPerson },
                    { "EMAIL",            data.Email },
                    { "PHONE",            data.Phone },
                    { "SERIAL",           data.SerialNumber },
                    { "MODEL",            data.TableModel },
                    { "TABLE_VERSION",    data.TableVersion },
                    { "PURCHASE_DATE",    data.PurchaseDate.ToString("dd.MM.yyyy") },
                    { "MAINTENANCE_DATE", data.MaintenanceDate.ToString("dd.MM.yyyy") },
                    { "HSM",              data.MotherboardId }
                })
                .CreateAndSignWithPrivateKey(_privateKey, PassPhrase);

            using var stream = File.Create(outputPath);
            license.Save(stream);
        }

        /// <summary>
        /// Motherboard-ID des aktuellen PCs lesen
        /// </summary>
        public static string GetMotherboardId()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
                foreach (ManagementObject obj in searcher.Get())
                    return obj["SerialNumber"]?.ToString() ?? "";
            }
            catch { }
            return "";
        }
    }

    public class LicenseData
    {
        // Kundendaten
        public string CompanyName { get; set; } = "";
        public string Address { get; set; } = "";
        public string ContactPerson { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";

        // Tischdaten
        public string SerialNumber { get; set; } = "";
        public string TableModel { get; set; } = "";
        public string TableVersion { get; set; } = "";

        // Datumsfelder
        public DateTime PurchaseDate { get; set; } = DateTime.Today;
        public DateTime MaintenanceDate { get; set; } = DateTime.Today.AddYears(1);

        // Hardware-Bindung
        public string MotherboardId { get; set; } = "";
    }
}
