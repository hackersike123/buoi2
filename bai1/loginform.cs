using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai1
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            // wire up events
            this.btnLogin.Click += BtnLogin_Click;
            this.btnCancel.Click += BtnCancel_Click;

            this.Text = "Đăng nhập";
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var user = txtUsername.Text?.Trim() ?? string.Empty;
            var pass = txtPassword.Text ?? string.Empty;

            try
            {
                if (AuthService.Verify(user, pass))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }
            catch
            {
            }

            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPassword.SelectAll();
            txtPassword.Focus();
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var dlg = new RegisterDialog())
            {
                dlg.ShowDialog(this);
            }
        }

        private void lnkForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var dlg = new ResetPasswordDialog())
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
