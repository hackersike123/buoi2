using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace bai1
{
    public partial class LoginForm : Form
    {
        // Example user data (replace with database or other data source as needed)
        private List<User> users = new List<User>
        {
            new User { Username = "admin", Password = "123456" },
            new User { Username = "user", Password = "password" }
        };

        public LoginForm()
        {
            InitializeComponent();
            dgvSeats.DataSource = invoices;
        }
    }
}