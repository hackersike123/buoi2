using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bai1
{
    public partial class Form1 : Form
    {
        private Color _seatDefaultBack = Color.White; // default now white
        private Color _seatSelectedBack = Color.LightGreen;

        // store invoices
        private List<Invoice> _invoices = new List<Invoice>();

        // single invoice form instance
        private InvoiceForm _invoiceFormInstance;

        public Form1()
        {
            InitializeComponent();
            NumberSeats();

            // Note: lblScreen is created in Designer so no runtime creation here.

            // Add tooltips to groupboxes for easier identification at runtime
            var tip = new ToolTip();
            // top-level groupboxes (do not overwrite Tag which is used for seat selection)
            foreach (var gb in this.Controls.OfType<GroupBox>())
            {
                tip.SetToolTip(gb, gb.Name);
            }
            // nested seat groupboxes inside groupBox1
            foreach (var gb in groupBox1.Controls.OfType<GroupBox>())
            {
                tip.SetToolTip(gb, gb.Name);
            }

            cbGender.SelectedIndex = -1;

            // Button texts
            if (btnChonGhe != null) btnChonGhe.Text = "Chọn";
            if (btnHuy != null) btnHuy.Text = "Xóa";
            if (btnThoat != null) btnThoat.Text = "Thoát";

            UpdateTongTien();

            // events
            txtProvince.TextChanged += (s, e) => UpdateTongTien();
        }

        private void NumberSeats()
        {
            var seatBoxes = groupBox1.Controls.OfType<GroupBox>()
                .OrderBy(gb => gb.Top)
                .ThenBy(gb => gb.Left)
                .ToList();

            int seatNumber = 1;
            foreach (var gb in seatBoxes)
            {
                // Ensure only the placeholder group boxes that represent seats are processed
                if (gb == groupBox44 || gb == groupBox51) continue;

                // initialize background to white for empty seats
                gb.BackColor = _seatDefaultBack;

                var existing = gb.Controls.OfType<Label>().FirstOrDefault(x => x.Name == "seatLabel");
                if (existing != null)
                {
                    // Do not overwrite designer-set label text; only set if empty
                    if (string.IsNullOrWhiteSpace(existing.Text))
                        existing.Text = seatNumber.ToString();
                    AttachSeatHandlers(gb, existing);
                    seatNumber++;
                    continue;
                }

                var lbl = new Label();
                lbl.Name = "seatLabel";
                lbl.Text = seatNumber.ToString();
                lbl.Dock = DockStyle.Fill;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
                lbl.BackColor = Color.Transparent;
                lbl.ForeColor = Color.Black;
                lbl.Margin = new Padding(0);

                gb.Text = string.Empty;
                gb.Controls.Add(lbl);

                if (_seatDefaultBack == default(Color))
                    _seatDefaultBack = gb.BackColor;

                // Tag stores selection state (bool)
                gb.Tag = false;

                AttachSeatHandlers(gb, lbl);
                seatNumber++;
            }

            UpdateTongTien();
        }

        private void AttachSeatHandlers(GroupBox gb, Label lbl)
        {
            gb.Click -= Seat_Click;
            lbl.Click -= Seat_Click;

            gb.Click += Seat_Click;
            lbl.Click += Seat_Click;
        }

        private void Seat_Click(object sender, EventArgs e)
        {
            // Require customer info filled before allowing seat selection
            if (string.IsNullOrWhiteSpace(txtKH.Text) || string.IsNullOrWhiteSpace(mtxtCCCD.Text))
            {
                MessageBox.Show("Vui lòng nhập thông tin khách hàng trước khi chọn ghế.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GroupBox gb = null;
            if (sender is Label lblSender && lblSender.Parent is GroupBox p)
                gb = p;
            else if (sender is GroupBox g)
                gb = g;

            if (gb == null) return;
            if (!gb.Enabled) return;

            bool selected = false;
            if (gb.Tag is bool b) selected = b;
            selected = !selected;
            gb.Tag = selected;

            var seatLbl = gb.Controls.OfType<Label>().FirstOrDefault(x => x.Name == "seatLabel");
            if (selected)
            {
                gb.BackColor = _seatSelectedBack; // green = selecting
                if (seatLbl != null) seatLbl.ForeColor = Color.White;
            }
            else
            {
                gb.BackColor = _seatDefaultBack; // white = empty
                if (seatLbl != null) seatLbl.ForeColor = Color.Black;
            }

            UpdateTongTien();
        }

        private void UpdateTongTien()
        {
            // Calculate total based on seat numbers (price per seat depends on seat index), not on count
            var selectedSeats = groupBox1.Controls.OfType<GroupBox>()
                .Where(gb => gb.Tag is bool b && b)
                .Select(gb => gb.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "seatLabel")?.Text)
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            long total = 0;
            foreach (var s in selectedSeats)
            {
                if (int.TryParse(s, out int seatNum))
                {
                    if (seatNum >= 1 && seatNum <= 5) total += 100000;
                    else if (seatNum >= 6 && seatNum <= 10) total += 110000;
                    else if (seatNum >= 11 && seatNum <= 15) total += 120000;
                    else if (seatNum >= 16) total += 130000;
                }
            }

            txtTongTien.Text = total.ToString("N0");
        }

        private void VeTypeChanged(object sender, EventArgs e)
        {
            // ticket type radio not used in new pricing, keep for UI compatibility
            UpdateTongTien();
        }

        private void btnChonGhe_Click(object sender, EventArgs e)
        {
            // Confirm/reserve selected seats and record customer
            if (string.IsNullOrWhiteSpace(txtKH.Text) || string.IsNullOrWhiteSpace(mtxtCCCD.Text))
            {
                MessageBox.Show("Vui lòng nhập thông tin khách hàng trước khi xác nhận.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = groupBox1.Controls.OfType<GroupBox>().Where(gb => gb.Tag is bool b && b).ToList();
            if (!selected.Any())
            {
                MessageBox.Show("Chưa chọn ghế.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var seatNumbers = selected.Select(gb => gb.Controls.OfType<Label>().FirstOrDefault(x => x.Name == "seatLabel")?.Text)
                .Where(s => !string.IsNullOrEmpty(s)).ToArray();

            UpdateTongTien();
            long total = 0;
            long.TryParse(txtTongTien.Text.Replace(",", ""), out total);

            // create invoice and store
            var invoice = new Invoice
            {
                Name = txtKH.Text.Trim(),
                CCCD = mtxtCCCD.Text.Trim(),
                Gender = cbGender.SelectedItem?.ToString() ?? string.Empty,
                Phone = txtPhone.Text.Trim(),
                Province = txtProvince.Text.Trim(),
                Seats = string.Join(",", seatNumbers),
                Total = total
            };
            _invoices.Add(invoice);

            // Update any open invoice form
            _invoiceFormInstance?.UpdateInvoices(_invoices);

            // add to grid
            try
            {
                dataGridView1.Rows.Add(invoice.Name, invoice.CCCD, invoice.Gender, invoice.Phone, invoice.Province, invoice.Seats, invoice.Total.ToString("N0"));
            }
            catch
            {
                dataGridView1.Rows.Add(invoice.Name, invoice.CCCD, invoice.Gender, invoice.Phone, invoice.Province);
            }

            // mark seats as reserved
            foreach (var gb in selected)
            {
                gb.Enabled = false;
                gb.BackColor = Color.Gray;
                var seatLbl = gb.Controls.OfType<Label>().FirstOrDefault(x => x.Name == "seatLabel");
                if (seatLbl != null) seatLbl.ForeColor = Color.White;
                gb.Tag = false;
            }

            // Clear customer inputs after confirming
            txtKH.Text = string.Empty;
            mtxtCCCD.Text = string.Empty;
            cbGender.SelectedIndex = -1;
            txtPhone.Text = string.Empty;
            txtProvince.Text = string.Empty;

            UpdateTongTien();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            // If a row in dataGridView1 is selected, delete that customer (and free seats)
            if (dataGridView1.SelectedRows != null && dataGridView1.SelectedRows.Count > 0)
            {
                var rows = dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToList();
                foreach (var row in rows)
                {
                    // seats are in column index 5 (if exists)
                    string seats = string.Empty;
                    try { seats = row.Cells[5].Value?.ToString() ?? string.Empty; } catch { seats = string.Empty; }

                    // remove invoice from list by matching Name + CCCD + Seats (best-effort)
                    string name = row.Cells[0].Value?.ToString() ?? string.Empty;
                    string cccd = row.Cells[1].Value?.ToString() ?? string.Empty;

                    var inv = _invoices.FirstOrDefault(i => i.Name == name && i.CCCD == cccd && i.Seats == seats);
                    if (inv != null) _invoices.Remove(inv);

                    // Update any open invoice form
                    _invoiceFormInstance?.UpdateInvoices(_invoices);

                    // free seats if any
                    if (!string.IsNullOrWhiteSpace(seats))
                    {
                        var seatNums = seats.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
                        foreach (var sn in seatNums)
                        {
                            // find groupbox with matching seatLabel text
                            var gb = groupBox1.Controls.OfType<GroupBox>()
                                .FirstOrDefault(g => g.Controls.OfType<Label>().Any(l => l.Name == "seatLabel" && l.Text == sn));
                            if (gb != null)
                            {
                                gb.Enabled = true;
                                gb.BackColor = _seatDefaultBack;
                                var seatLbl = gb.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "seatLabel");
                                if (seatLbl != null) seatLbl.ForeColor = Color.Black;
                            }
                        }
                    }

                    // remove row from grid
                    dataGridView1.Rows.Remove(row);
                }

                UpdateTongTien();
                return;
            }

            // Otherwise clear selected seats (deselect) and clear customer inputs
            var selected = groupBox1.Controls.OfType<GroupBox>().Where(gb => gb.Tag is bool b && b).ToList();
            foreach (var gb in selected)
            {
                gb.Tag = false;
                gb.BackColor = _seatDefaultBack;
                var seatLbl = gb.Controls.OfType<Label>().FirstOrDefault(x => x.Name == "seatLabel");
                if (seatLbl != null) seatLbl.ForeColor = Color.Black;
            }

            // Clear customer inputs
            txtKH.Text = string.Empty;
            mtxtCCCD.Text = string.Empty;
            cbGender.SelectedIndex = -1;
            txtPhone.Text = string.Empty;
            txtProvince.Text = string.Empty;

            UpdateTongTien();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Designer event handlers (no-op or simple behaviors)
        private void groupBox44_Enter(object sender, EventArgs e) { }
        private void lblProvince_Click(object sender, EventArgs e) { }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) { }
        private void mtxtCCCD_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Add customer row without seats (optional step before selecting seats)
            string kh = txtKH.Text.Trim();
            string cccd = mtxtCCCD.Text.Trim().Replace(" ", "");
            if (string.IsNullOrEmpty(kh))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng (KH).", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cccd.Length != 12 || !cccd.All(char.IsDigit))
            {
                MessageBox.Show("CCCD phải gồm đúng 12 chữ số.", "Lỗi CCCD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Add row with empty seat and total
            try
            {
                dataGridView1.Rows.Add(kh, cccd, cbGender.SelectedItem?.ToString() ?? string.Empty, txtPhone.Text.Trim(), txtProvince.Text.Trim(), string.Empty, "0");
            }
            catch
            {
                dataGridView1.Rows.Add(kh, cccd, cbGender.SelectedItem?.ToString() ?? string.Empty, txtPhone.Text.Trim(), txtProvince.Text.Trim());
            }

            MessageBox.Show("Khách hàng đã được thêm tạm.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        // Menu handler to open a single InvoiceForm
        private void menuOpenInvoiceForm_Click(object sender, EventArgs e)
        {
            try
            {
                if (_invoiceFormInstance == null || _invoiceFormInstance.IsDisposed)
                {
                    _invoiceFormInstance = new InvoiceForm(_invoices);
                    _invoiceFormInstance.Show(this);
                }
                else
                {
                    // refresh existing instance and bring to front
                    _invoiceFormInstance.UpdateInvoices(_invoices);
                    _invoiceFormInstance.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Invoice form lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lblGiaVeDon_Click(object sender, EventArgs e)
        {

        }

        private void lblTongTien_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
