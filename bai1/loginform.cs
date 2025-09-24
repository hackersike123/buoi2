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
        // Default test credentials
        private const string DefaultUser = "admin";
        private const string DefaultPass = "123456";

        public LoginForm()
        {
            InitializeComponent();

            // wire up events (designer did not add click handlers)
            this.btnLogin.Click += BtnLogin_Click;
            this.btnCancel.Click += BtnCancel_Click;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // Cancel closes the dialog with Cancel result
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var user = txtUsername.Text?.Trim() ?? string.Empty;
            var pass = txtPassword.Text ?? string.Empty;

            // Simple credential check - change to your real auth if needed
            if (string.Equals(user, DefaultUser, StringComparison.OrdinalIgnoreCase) && pass == DefaultPass)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPassword.SelectAll();
            txtPassword.Focus();
        }
    }
}
