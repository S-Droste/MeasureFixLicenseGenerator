using System;
using System.Drawing;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace MeasureFixLicenseGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LicenseGenerator.InitializeKeys();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // Pflichtfelder prüfen
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text) ||
                string.IsNullOrWhiteSpace(txtSerial.Text) ||
                string.IsNullOrWhiteSpace(txtMotherboardId.Text))
            {
                MessageBox.Show("Bitte Firmenname, Seriennummer und Motherboard-ID ausfüllen.",
                    "Fehlende Felder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var data = new LicenseData
            {
                CompanyName    = txtCompanyName.Text.Trim(),
                Address        = txtAddress.Text.Trim(),
                ContactPerson  = txtContactPerson.Text.Trim(),
                Email          = txtEmail.Text.Trim(),
                Phone          = txtPhone.Text.Trim(),
                SerialNumber   = txtSerial.Text.Trim(),
                TableModel     = txtTableModel.Text.Trim(),
                TableVersion   = txtTableVersion.Text.Trim(),
                PurchaseDate   = dtpPurchaseDate.Value,
                MaintenanceDate = dtpMaintenanceDate.Value,
                MotherboardId  = txtMotherboardId.Text.Trim()
            };

            using var saveDialog = new SaveFileDialog
            {
                Title = "Lizenz speichern",
                Filter = "MeasureFix Lizenz (*.mfx)|*.mfx",
                FileName = $"license_{data.SerialNumber}.mfx",
                DefaultExt = "mfx"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LicenseGenerator.CreateLicense(data, saveDialog.FileName);
                    MessageBox.Show(
                        $"Lizenz erfolgreich erstellt!\n\n" +
                        $"Datei: {saveDialog.FileName}\n\n" +
                        $"Bitte diese Datei auf den Tisch-PC kopieren:\n" +
                        $"C:\\License\\license.mfx",
                        "Lizenz erstellt",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Erstellen der Lizenz:\n{ex.Message}",
                        "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGetMotherboard_Click(object sender, EventArgs e)
        {
            string id = LicenseGenerator.GetMotherboardId();
            if (string.IsNullOrEmpty(id))
                MessageBox.Show("Motherboard-ID konnte nicht gelesen werden.", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                txtMotherboardId.Text = id;
        }

        private void btnShowPublicKey_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"Öffentlicher Schlüssel (für MeasureFixOS):\n\n{LicenseGenerator.PublicKey}\n\n" +
                $"Dieser Schlüssel muss in LicenseService.cs im OS eingetragen sein.",
                "Public Key",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
