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
                // N?u ??ng nh?p th�nh c�ng th� m? Form1
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new Form1());
                }
                // N?u kh�ng th� tho�t ?ng d?ng
            }
        }
    }
}