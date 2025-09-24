using System;
using System.Windows.Forms;

namespace bai1
{
    public class RegisterDialog : Form
    {
        private TextBox txtUser;
        private TextBox txtPass;
        private TextBox txtPass2;
        private Button btnOk;
        private Button btnCancel;

        public RegisterDialog()
        {
            this.Text = "Đăng ký";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new System.Drawing.Size(320, 200);

            var lblU = new Label { Text = "Tài khoản:", Left = 10, Top = 10 };
            txtUser = new TextBox { Left = 120, Top = 8, Width = 180 };
            var lblP = new Label { Text = "Mật khẩu:", Left = 10, Top = 40 };
            txtPass = new TextBox { Left = 120, Top = 38, Width = 180, PasswordChar = '*' };
            var lblP2 = new Label { Text = "Xác nhận:", Left = 10, Top = 70 };
            txtPass2 = new TextBox { Left = 120, Top = 68, Width = 180, PasswordChar = '*' };

            btnOk = new Button { Text = "OK", Left = 120, Top = 100, Width = 80 };
            btnCancel = new Button { Text = "Hủy", Left = 220, Top = 100, Width = 80 };

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtUser.Text)) { MessageBox.Show("Vui lòng nhập tài khoản"); return; }
                if (txtPass.Text != txtPass2.Text) { MessageBox.Show("Mật khẩu không khớp"); return; }
                var ok = AuthService.Register(txtUser.Text.Trim(), txtPass.Text);
                if (!ok) { MessageBox.Show("Tài khoản đã tồn tại"); return; }
                MessageBox.Show("Đăng ký thành công");
                this.Close();
            };
            btnCancel.Click += (s, e) => { this.Close(); };

            this.Controls.AddRange(new Control[] { lblU, txtUser, lblP, txtPass, lblP2, txtPass2, btnOk, btnCancel });
        }
    }
}
