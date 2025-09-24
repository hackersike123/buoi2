using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace bai1
{
    public class InvoiceForm : Form
    {
        private ListBox listBox1;
        private List<Invoice> _invoices;

        public InvoiceForm(List<Invoice> invoices)
        {
            _invoices = invoices ?? new List<Invoice>();

            this.Text = "Danh sách hóa ??n";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(600, 400);

            listBox1 = new ListBox();
            listBox1.Dock = DockStyle.Fill;
            listBox1.Font = new Font("Consolas", 10);

            PopulateList();

            this.Controls.Add(listBox1);
        }

        private void PopulateList()
        {
            listBox1.Items.Clear();
            foreach (var inv in _invoices)
            {
                listBox1.Items.Add($"Tên: {inv.Name} | CCCD: {inv.CCCD} | Gh?: {inv.Seats} | T?ng: {inv.Total:N0}");
            }
        }

        public void UpdateInvoices(List<Invoice> invoices)
        {
            _invoices = invoices ?? new List<Invoice>();
            if (this.IsHandleCreated)
            {
                // if form created, update on UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)PopulateList);
                }
                else
                {
                    PopulateList();
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // InvoiceForm
            // 
            this.ClientSize = new System.Drawing.Size(741, 337);
            this.Name = "InvoiceForm";
            this.Load += new System.EventHandler(this.InvoiceForm_Load);
            this.ResumeLayout(false);

        }

        private void InvoiceForm_Load(object sender, EventArgs e)
        {

        }
    }
}
