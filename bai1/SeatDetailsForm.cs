using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace bai1
{
    public class SeatDetailsForm : Form
    {
        private ListView lv;

        public SeatDetailsForm(List<Invoice> invoices)
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            this.Text = "Chi tiết ghế đã mua";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            lv = new ListView();
            lv.Dock = DockStyle.Fill;
            lv.View = View.Details;
            lv.FullRowSelect = true;
            lv.Columns.Add("Tên");
            lv.Columns.Add("CCCD");
            lv.Columns.Add("Ghế");
            lv.Columns.Add("Tổng");

            Populate(invoices);

            this.Controls.Add(lv);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Name = "SeatDetailsForm";
            this.ResumeLayout(false);
        }

        public void Populate(List<Invoice> invoices)
        {
            if (lv == null) return;
            lv.Items.Clear();
            foreach (var inv in invoices)
            {
                var item = new ListViewItem(new[] { inv.Name, inv.CCCD, inv.Seats, inv.Total.ToString("N0") });
                lv.Items.Add(item);
            }
            foreach (ColumnHeader c in lv.Columns) c.Width = -2;
        }
    }
}
