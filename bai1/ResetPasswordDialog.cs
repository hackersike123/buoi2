using System;
using System.Windows.Forms;

namespace bai1
{
    public class ResetPasswordDialog : Form
    {
        private TextBox txtUser;
        private Button btnOk;
        private Button btnCancel;

        public ResetPasswordDialog()
        {
            this.Text = "Quên mật khẩu";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new System.Drawing.Size(320, 150);

            var lblU = new Label { Text = "Tài khoản:", Left = 10, Top = 10 };
            txtUser = new TextBox { Left = 120, Top = 8, Width = 180 };

            btnOk = new Button { Text = "Đặt lại", Left = 120, Top = 40, Width = 80 };
            btnCancel = new Button { Text = "Hủy", Left = 220, Top = 40, Width = 80 };

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtUser.Text)) { MessageBox.Show("Vui lòng nhập tài khoản"); return; }
                // Set default password to '1'
                var ok = AuthService.SetPassword(txtUser.Text.Trim(), "1");
                if (!ok) { MessageBox.Show("Không tìm thấy tài khoản"); return; }
                MessageBox.Show("Mật khẩu đã được đặt về mặc định: 1");
                this.Close();
            };
            btnCancel.Click += (s, e) => { this.Close(); };

            this.Controls.AddRange(new Control[] { lblU, txtUser, btnOk, btnCancel });
        }
    }
}
