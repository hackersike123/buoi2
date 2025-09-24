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

            this.Text = "Sửa hóa đơn";
            this.Size = new System.Drawing.Size(380, 220);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblName = new Label { Text = "Tên:", Left = 10, Top = 15, Width = 80 };
            txtName = new TextBox { Left = 100, Top = 12, Width = 250, Text = Invoice.Name };

            var lblPhone = new Label { Text = "SĐT:", Left = 10, Top = 50, Width = 80 };
            txtPhone = new TextBox { Left = 100, Top = 46, Width = 250, Text = Invoice.Phone };

            var lblProv = new Label { Text = "Tỉnh:", Left = 10, Top = 85, Width = 80 };
            txtProvince = new TextBox { Left = 100, Top = 82, Width = 250, Text = Invoice.Province };

            btnOk = new Button { Text = "Lưu", Left = 100, Top = 130, Width = 100 };
            btnCancel = new Button { Text = "Hủy", Left = 210, Top = 130, Width = 100 };
            btnOk.Click += (s, e) => { Invoice.Name = txtName.Text.Trim(); Invoice.Phone = txtPhone.Text.Trim(); Invoice.Province = txtProvince.Text.Trim(); this.DialogResult = DialogResult.OK; this.Close(); };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] { lblName, txtName, lblPhone, txtPhone, lblProv, txtProvince, btnOk, btnCancel });
        }
    }
}
