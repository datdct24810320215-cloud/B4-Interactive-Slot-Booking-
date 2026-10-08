#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Bai4_Interactive_Slot_Booking_
{
    public partial class Form1 : Form
    {
        // FIELDS
        private TableLayoutPanel tlpSeats;
        private ComboBox cboTimeSlot;
        private Label lblSelectedCount;
        private Label lblTotalPrice;
        private Button btnConfirm;
        private Button btnClearAll;

        private List<Button> allSeats = new List<Button>();
        private HashSet<Button> selectedSeats = new HashSet<Button>();
        private HashSet<Button> bookedSeats = new HashSet<Button>();

        private const int PRICE_SANG = 100000;
        private const int PRICE_TOI = 150000;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetupUI();
            CreateSeats();
            UpdateStats();
        }

        private void SetupUI()
        {
            this.Font = new Font("Segoe UI", 10);

            // Label "Khung giờ:" at (20,20), bold 11pt.
            Label lblTime = new Label
            {
                Text = "Khung giờ:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            this.Controls.Add(lblTime);

            // ComboBox cboTimeSlot at (120,17), size 220x30, DropDownList
            cboTimeSlot = new ComboBox
            {
                Location = new Point(120, 17),
                Size = new Size(220, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTimeSlot.Items.Add("Sáng (100.000đ)");
            cboTimeSlot.Items.Add("Tối (150.000đ)");
            cboTimeSlot.SelectedIndex = 0;
            cboTimeSlot.SelectedIndexChanged += (s, e) => UpdateStats();
            this.Controls.Add(cboTimeSlot);

            // TableLayoutPanel tlpSeats at (20,70), size 520x480, ColumnCount=5, RowCount=4
            tlpSeats = new TableLayoutPanel
            {
                Location = new Point(20, 70),
                Size = new Size(520, 480),
                ColumnCount = 5,
                RowCount = 4,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            for (int i = 0; i < 5; i++)
                tlpSeats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            for (int i = 0; i < 4; i++)
                tlpSeats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.Controls.Add(tlpSeats);

            // Label "Số vị trí đang chọn:" and lblSelectedCount
            Label lblCountText = new Label
            {
                Text = "Số vị trí đang chọn:",
                Location = new Point(570, 80),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            this.Controls.Add(lblCountText);

            lblSelectedCount = new Label
            {
                Text = "0",
                Location = new Point(570, 115),
                AutoSize = true,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.Green
            };
            this.Controls.Add(lblSelectedCount);

            // Label "Tạm tính tiền:" and lblTotalPrice
            Label lblPriceText = new Label
            {
                Text = "Tạm tính tiền:",
                Location = new Point(570, 170),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            this.Controls.Add(lblPriceText);

            lblTotalPrice = new Label
            {
                Text = "0 VNĐ",
                Location = new Point(570, 205),
                AutoSize = true,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.Red
            };
            this.Controls.Add(lblTotalPrice);

            // Button btnConfirm
            btnConfirm = new Button
            {
                Text = "XÁC NHẬN ĐẶT",
                Location = new Point(570, 280),
                Size = new Size(220, 55),
                BackColor = Color.LightGreen,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);

            // Button btnClearAll
            btnClearAll = new Button
            {
                Text = "HỦY CHỌN TẤT CẢ",
                Location = new Point(570, 350),
                Size = new Size(220, 55),
                BackColor = Color.LightCoral,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnClearAll.Click += BtnClearAll_Click;
            this.Controls.Add(btnClearAll);

            // Label note
            Label lblNote = new Label
            {
                Text = "Chú thích:\n● Trắng = Trống\n● Xanh = Đang chọn\n● Đỏ = Đã đặt",
                Location = new Point(570, 430),
                AutoSize = true
            };
            this.Controls.Add(lblNote);
        }

        private void CreateSeats()
        {
            for (int i = 0; i < 20; i++)
            {
                Button btn = new Button
                {
                    Text = "Bàn " + (i + 1),
                    Dock = DockStyle.Fill,
                    BackColor = Color.WhiteSmoke,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Tag = i + 1,
                    Margin = new Padding(2)
                };
                btn.Click += Seat_Click;
                tlpSeats.Controls.Add(btn);
                allSeats.Add(btn);
            }
        }

        private void Seat_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn))
                return;

            if (bookedSeats.Contains(btn))
            {
                MessageBox.Show("Bàn này đã được đặt...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (selectedSeats.Contains(btn))
            {
                selectedSeats.Remove(btn);
                btn.BackColor = Color.WhiteSmoke;
            }
            else
            {
                selectedSeats.Add(btn);
                btn.BackColor = Color.LightGreen;
            }

            UpdateStats();
        }

        private void UpdateStats()
        {
            lblSelectedCount.Text = selectedSeats.Count.ToString();
            int price = cboTimeSlot.SelectedIndex == 0 ? PRICE_SANG : PRICE_TOI;
            lblTotalPrice.Text = (selectedSeats.Count * price).ToString("N0") + " VNĐ";
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một bàn để đặt.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string total = (selectedSeats.Count * (cboTimeSlot.SelectedIndex == 0 ? PRICE_SANG : PRICE_TOI)).ToString("N0") + " VNĐ";
            var dr = MessageBox.Show($"Bạn có chắc muốn đặt {selectedSeats.Count} bàn với tổng {total}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                foreach (var btn in selectedSeats)
                {
                    btn.BackColor = Color.LightCoral;
                    bookedSeats.Add(btn);
                }
                selectedSeats.Clear();
                UpdateStats();
                MessageBox.Show("Đặt bàn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnClearAll_Click(object sender, EventArgs e)
        {
            foreach (var btn in selectedSeats)
            {
                btn.BackColor = Color.WhiteSmoke;
            }
            selectedSeats.Clear();
            UpdateStats();
        }
    }
}