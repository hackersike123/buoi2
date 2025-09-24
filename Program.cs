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
                // N?u ??ng nh?p thành công thì m? Form1
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new Form1());
                }
                // N?u không thì thoát ?ng d?ng
            }
        }
    }
}