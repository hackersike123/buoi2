using System;
using System.Windows.Forms;

namespace bai1
{
    public class CustomerDetailDialog : Form
    {
        private Invoice _invoice;

        public CustomerDetailDialog(Invoice inv)
        {
            _invoice = inv;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Chi tiết khách hàng";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new System.Drawing.Size(380, 300);

            int y = 10;
            int labelW = 80;
            int ctrlW = 250;
            int h = 22;

            void addRow(string labelText, out TextBox ctrl)
            {
                var lbl = new Label { Text = labelText, Left = 10, Top = y + 3, Width = labelW };
                ctrl = new TextBox { Left = 10 + labelW + 8, Top = y, Width = ctrlW, ReadOnly = true };
                this.Controls.Add(lbl);
                this.Controls.Add(ctrl);
                y += h + 8;
            }

            addRow("Tên:", out TextBox txtName);
            addRow("CCCD:", out TextBox txtCCCD);
            addRow("Giới tính:", out TextBox txtGender);
            addRow("SĐT:", out TextBox txtPhone);
            addRow("Tỉnh:", out TextBox txtProvince);
            addRow("Ghế:", out TextBox txtSeats);
            addRow("Tổng:", out TextBox txtTotal);

            var btnClose = new Button { Text = "Đóng", Left = 250, Top = y, Width = 80 };
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            // expose controls for LoadData
            this.Tag = new {
                txtName = this.Controls[1] as TextBox,
                txtCCCD = this.Controls[3] as TextBox,
                txtGender = this.Controls[5] as TextBox,
                txtPhone = this.Controls[7] as TextBox,
                txtProvince = this.Controls[9] as TextBox,
                txtSeats = this.Controls[11] as TextBox,
                txtTotal = this.Controls[13] as TextBox
            };
        }

        private void LoadData()
        {
            dynamic t = this.Tag;
            if (t == null) return;
            (t.txtName as TextBox).Text = _invoice.Name ?? string.Empty;
            (t.txtCCCD as TextBox).Text = _invoice.CCCD ?? string.Empty;
            (t.txtGender as TextBox).Text = _invoice.Gender ?? string.Empty;
            (t.txtPhone as TextBox).Text = _invoice.Phone ?? string.Empty;
            (t.txtProvince as TextBox).Text = _invoice.Province ?? string.Empty;
            (t.txtSeats as TextBox).Text = _invoice.Seats ?? string.Empty;
            (t.txtTotal as TextBox).Text = _invoice.Total.ToString("N0");
        }
    }
}
