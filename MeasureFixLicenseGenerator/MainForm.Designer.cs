namespace MeasureFixLicenseGenerator
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text = "MeasureFix License Generator";
            this.Size = new System.Drawing.Size(620, 720);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.White;

            int labelW = 160, fieldX = 175, fieldW = 400, rowH = 32, startY = 10;

            // ── KUNDENDATEN ──────────────────────────────────────────
            AddGroupLabel("Kundendaten", startY); startY += 28;

            txtCompanyName   = AddRow("Firmenname *",    ref startY, labelW, fieldX, fieldW, rowH);
            txtAddress       = AddRow("Adresse",         ref startY, labelW, fieldX, fieldW, rowH);
            txtContactPerson = AddRow("Ansprechpartner", ref startY, labelW, fieldX, fieldW, rowH);
            txtEmail         = AddRow("E-Mail",          ref startY, labelW, fieldX, fieldW, rowH);
            txtPhone         = AddRow("Telefon",         ref startY, labelW, fieldX, fieldW, rowH);

            startY += 8;

            // ── TISCHDATEN ───────────────────────────────────────────
            AddGroupLabel("Tischdaten", startY); startY += 28;

            txtSerial       = AddRow("Seriennummer *",  ref startY, labelW, fieldX, fieldW, rowH);
            txtTableModel   = AddRow("Tisch-Modell",    ref startY, labelW, fieldX, fieldW, rowH);
            txtTableVersion = AddRow("Tisch-Version",   ref startY, labelW, fieldX, fieldW, rowH);

            startY += 8;

            // ── DATUM ────────────────────────────────────────────────
            AddGroupLabel("Datum", startY); startY += 28;

            dtpPurchaseDate    = AddDateRow("Kaufdatum",    ref startY, labelW, fieldX, fieldW, rowH);
            dtpMaintenanceDate = AddDateRow("Wartung bis",  ref startY, labelW, fieldX, fieldW, rowH);
            dtpMaintenanceDate.Value = System.DateTime.Today.AddYears(1);

            startY += 8;

            // ── HARDWARE ─────────────────────────────────────────────
            AddGroupLabel("Hardware-Bindung", startY); startY += 28;

            txtMotherboardId = AddRow("Motherboard-ID *", ref startY, labelW, fieldX, 280, rowH);

            btnGetMotherboard = new System.Windows.Forms.Button
            {
                Text = "Von diesem PC",
                Location = new System.Drawing.Point(fieldX + 285, startY - rowH - 2),
                Size = new System.Drawing.Size(115, 26),
                BackColor = System.Drawing.Color.FromArgb(200, 200, 200),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat
            };
            btnGetMotherboard.Click += btnGetMotherboard_Click;
            this.Controls.Add(btnGetMotherboard);

            startY += 10;

            // ── BUTTONS ──────────────────────────────────────────────
            btnGenerate = new System.Windows.Forms.Button
            {
                Text = "✓  Lizenz erstellen & speichern",
                Location = new System.Drawing.Point(fieldX, startY),
                Size = new System.Drawing.Size(280, 40),
                BackColor = System.Drawing.Color.FromArgb(220, 50, 50),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold)
            };
            btnGenerate.Click += btnGenerate_Click;
            this.Controls.Add(btnGenerate);

            btnShowPublicKey = new System.Windows.Forms.Button
            {
                Text = "Public Key anzeigen",
                Location = new System.Drawing.Point(fieldX + 290, startY + 7),
                Size = new System.Drawing.Size(150, 26),
                BackColor = System.Drawing.Color.FromArgb(240, 240, 240),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat
            };
            btnShowPublicKey.Click += btnShowPublicKey_Click;
            this.Controls.Add(btnShowPublicKey);

            this.ClientSize = new System.Drawing.Size(620, startY + 60);
        }

        private System.Windows.Forms.TextBox AddRow(string label, ref int y, int lw, int fx, int fw, int rh)
        {
            var lbl = new System.Windows.Forms.Label
            {
                Text = label + ":",
                Location = new System.Drawing.Point(10, y + 4),
                Size = new System.Drawing.Size(lw, 20),
                Font = new System.Drawing.Font("Segoe UI", 9f)
            };
            var txt = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(fx, y),
                Size = new System.Drawing.Size(fw, 24),
                Font = new System.Drawing.Font("Segoe UI", 9f),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };
            this.Controls.Add(lbl);
            this.Controls.Add(txt);
            y += rh;
            return txt;
        }

        private System.Windows.Forms.DateTimePicker AddDateRow(string label, ref int y, int lw, int fx, int fw, int rh)
        {
            var lbl = new System.Windows.Forms.Label
            {
                Text = label + ":",
                Location = new System.Drawing.Point(10, y + 4),
                Size = new System.Drawing.Size(lw, 20),
                Font = new System.Drawing.Font("Segoe UI", 9f)
            };
            var dtp = new System.Windows.Forms.DateTimePicker
            {
                Location = new System.Drawing.Point(fx, y),
                Size = new System.Drawing.Size(fw, 24),
                Format = System.Windows.Forms.DateTimePickerFormat.Short
            };
            this.Controls.Add(lbl);
            this.Controls.Add(dtp);
            y += rh;
            return dtp;
        }

        private void AddGroupLabel(string text, int y)
        {
            var lbl = new System.Windows.Forms.Label
            {
                Text = "── " + text + " ──────────────────────────────",
                Location = new System.Drawing.Point(10, y),
                Size = new System.Drawing.Size(580, 20),
                Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(180, 30, 30)
            };
            this.Controls.Add(lbl);
        }

        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtContactPerson;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.TextBox txtTableModel;
        private System.Windows.Forms.TextBox txtTableVersion;
        private System.Windows.Forms.DateTimePicker dtpPurchaseDate;
        private System.Windows.Forms.DateTimePicker dtpMaintenanceDate;
        private System.Windows.Forms.TextBox txtMotherboardId;
        private System.Windows.Forms.Button btnGetMotherboard;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnShowPublicKey;
    }
}
