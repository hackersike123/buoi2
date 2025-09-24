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
        private SplitContainer split;

        private List<Invoice> _invoices;

        public SeatDetailsForm(List<Invoice> invoices)
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            _invoices = invoices ?? new List<Invoice>();

            Populate(_invoices);
        }

        private void InitializeComponent()
        {
            this.split = new SplitContainer();
            this.lv = new ListView();

            // form
            this.SuspendLayout();
            this.ClientSize = new Size(500, 420);
            this.Name = "SeatDetailsForm";
            this.Text = "Danh sách khách hàng";

            // split
            this.split.Dock = DockStyle.Fill;
            this.split.Orientation = Orientation.Vertical;
            this.split.SplitterDistance = 300;

            // left: listview (only names)
            this.lv.Dock = DockStyle.Fill;
            this.lv.View = View.List; // show names only
            this.lv.FullRowSelect = true;
            this.lv.MultiSelect = false;
            this.lv.ItemActivate += Lv_ItemActivate; // handles double-click or Enter

            this.split.Panel1.Controls.Add(this.lv);

            // right: simple instruction panel
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(10);

            var lbl = new Label
            {
                Text = "Chọn một khách hàng (double-click) để xem chi tiết.",
                AutoSize = true,
                Top = 8,
                Left = 8
            };
            panel.Controls.Add(lbl);

            this.split.Panel2.Controls.Add(panel);

            this.Controls.Add(this.split);
            this.ResumeLayout(false);
        }

        private void Lv_ItemActivate(object sender, EventArgs e)
        {
            if (lv.SelectedItems.Count == 0) return;
            var item = lv.SelectedItems[0];
            if (item?.Tag is Invoice inv)
            {
                using (var dlg = new CustomerDetailDialog(inv))
                {
                    dlg.ShowDialog(this);
                }
            }
        }

        public void Populate(List<Invoice> invoices)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            _invoices = invoices ?? new List<Invoice>();
            lv.Items.Clear();
            foreach (var inv in _invoices)
            {
                var lvi = new ListViewItem(inv.Name ?? string.Empty);
                lvi.Tag = inv;
                lv.Items.Add(lvi);
            }
            ClearRightPanel();
        }

        private void ClearRightPanel()
        {
            // nothing for now
        }
    }
}
