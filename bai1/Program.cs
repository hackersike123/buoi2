using System;
using System.Windows.Forms;

namespace bai1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show login dialog before starting main form
            using (var login = new LoginForm())
            {
                var result = login.ShowDialog();
                if (result != DialogResult.OK)
                {
                    // Exit if login cancelled or failed
                    return;
                }
            }

            Application.Run(new Form1());
        }
    }
}

// Add this class if it does not exist in your project
public class LoginForm : Form
{
    // Implement your login logic here
}
