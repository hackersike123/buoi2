using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace bai1
{
    public class InvoiceForm : Form
    {
        private ListBox listBox1;

        public InvoiceForm(List<Invoice> invoices)
        {
            this.Text = "Danh sách hóa ??n";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(600, 400);

            listBox1 = new ListBox();
            listBox1.Dock = DockStyle.Fill;
            listBox1.Font = new Font("Consolas", 10);

            foreach (var inv in invoices)
            {
                listBox1.Items.Add($"Tên: {inv.Name} | CCCD: {inv.CCCD} | Gh?: {inv.Seats} | T?ng: {inv.Total:N0}");
            }

            this.Controls.Add(listBox1);
        }
    }
}
