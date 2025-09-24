using System;
using System.Windows.Forms;

namespace bai1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var loginForm = new LoginForm())
            {
                // Nếu đăng nhập thành công thì mở Form1
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new Form1());
                }
                // Nếu không thì thoát ứng dụng
            }
        }
    }
}

// Add this class if it does not exist in your project
public class LoginForm : Form
{
    // Implement your login logic here
}
