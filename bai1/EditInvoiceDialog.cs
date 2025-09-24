using System;
using System.Windows.Forms;

namespace bai1
{
    public class EditInvoiceDialog : Form
    {
        public Invoice Invoice { get; private set; }

        private TextBox txtName;
        private TextBox txtPhone;
        private TextBox txtProvince;
        private Button btnOk;
        private Button btnCancel;

        public EditInvoiceDialog(Invoice inv)
        {
            Invoice = new Invoice
            {
                Name = inv.Name,
                CCCD = inv.CCCD,
                Gender = inv.Gender,
                Phone = inv.Phone,
                Province = inv.Province,
                Seats = inv.Seats,
                Total = inv.Total
            };

            this.Text = "S?a hóa ??n";
            this.Size = new System.Drawing.Size(350, 200);
            this.StartPosition = FormStartPosition.CenterParent;

            var lblName = new Label { Text = "Tên:", Left = 10, Top = 10 };
            txtName = new TextBox { Left = 80, Top = 8, Width = 240, Text = Invoice.Name };
            var lblPhone = new Label { Text = "S?T:", Left = 10, Top = 40 };
            txtPhone = new TextBox { Left = 80, Top = 38, Width = 240, Text = Invoice.Phone };
            var lblProv = new Label { Text = "T?nh:", Left = 10, Top = 70 };
            txtProvince = new TextBox { Left = 80, Top = 68, Width = 240, Text = Invoice.Province };

            btnOk = new Button { Text = "OK", Left = 80, Top = 110, Width = 80 };
            btnCancel = new Button { Text = "H?y", Left = 160, Top = 110, Width = 80 };
            btnOk.Click += (s, e) => { Invoice.Name = txtName.Text.Trim(); Invoice.Phone = txtPhone.Text.Trim(); Invoice.Province = txtProvince.Text.Trim(); this.DialogResult = DialogResult.OK; this.Close(); };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] { lblName, txtName, lblPhone, txtPhone, lblProv, txtProvince, btnOk, btnCancel });
        }
    }
}
