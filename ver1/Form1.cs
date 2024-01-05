using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ver1
{
    public partial class Form1 : Form
    {
        private SqlDataAdapter adapter;
        private DataTable QLDT;
        Test2Entities3 db = new Test2Entities3();
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-0VQF58J;Initial Catalog=ver1;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
            load();
        }

        private void Reset()
        {
            foreach (ListViewItem item in lv_ds.Items)
            {
                item.Selected = false;
            }

            txtMaDon.Clear();
            txtTenPhim.Clear();
            txtQuocGia.Clear();
            rbtTinhCam.Checked = false;
            rbtHanhDong.Checked = false;
            dTNgayCongChieu.Value = DateTime.Now;
            txtDTQD.Clear();
            rB3D.Checked = false;
            rB2D.Checked = false;
            txtPTXCDB.Clear();
            txtPTGD.Clear();
        }

        private void rB2D_CheckedChanged(object sender, EventArgs e)
        {
            lblPTGD.Visible = true;
            txtPTGD.Visible = true;
            lblPTXCDB.Visible = false;
            txtPTXCDB.Visible = false;
        }

        private void rB3D_CheckedChanged(object sender, EventArgs e)
        {
            lblPTGD.Visible = false;
            txtPTGD.Visible = false;
            lblPTXCDB.Visible = true;
            txtPTXCDB.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblPTGD.Visible = true;
            txtPTGD.Visible = true;
            lblPTXCDB.Visible = false;
            txtPTXCDB.Visible = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv_ds.SelectedItems.Count > 0)
            {
                txtMaDon.Text = lv_ds.SelectedItems[0].SubItems[0].Text;
                txtTenPhim.Text = lv_ds.SelectedItems[0].SubItems[1].Text;
                if (rbtTinhCam.Checked)
                    rbtTinhCam.Text = lv_ds.SelectedItems[0].SubItems[2].Text;
                else
                    rbtHanhDong.Text = lv_ds.SelectedItems[0].SubItems[2].Text;
                dTNgayCongChieu.Text = lv_ds.SelectedItems[0].SubItems[3].Text;

            }
        }
        public void load() // tai len danh sach trong sql
        {
            lv_ds.View = View.Details;
            lv_ds.GridLines = true;
            lv_ds.Columns.Add("MaDon");
            lv_ds.Columns.Add("TenPhim");
            lv_ds.Columns.Add("TheLoai");
            lv_ds.Columns.Add("NgayCongChieu");
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select * From QLDT", connection);
            SqlDataReader da;
            da = cmd.ExecuteReader();
            while (da.Read())
            {
                var item1 = lv_ds.Items.Add(da[0].ToString());
                item1.SubItems.Add(da[1].ToString());
                item1.SubItems.Add(da[3].ToString());
                item1.SubItems.Add(da[4].ToString());

            }
            connection.Close();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn đóng chương trình?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Reset();
            txtMaDon.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (Test2Entities3 db = new Test2Entities3())
            {
                QLDT phim = new QLDT();
                phim.MaDon = txtMaDon.Text;
                phim.TenPhim = txtTenPhim.Text;
                phim.TheLoai = rbtTinhCam.Text;
                phim.TheLoai = rbtHanhDong.Checked ? rbtTinhCam.Text : (rbtTinhCam.Checked ? rbtHanhDong.Text : "NONE");
                phim.NgayCongChieu = dTNgayCongChieu.Value;
                phim.QuocGia = txtQuocGia.Text;
        //        phim.DoTuoi = Convert.ToInt32(txtDTQD.Text);

                if (rB2D.Checked)
                {
                    phim.PTGD = Convert.ToInt32(txtPTGD.Text);
                }
                if (rB3D.Checked)
                {
                    phim.PTSCDB = Convert.ToInt32(txtPTXCDB.Text);
                }
                db.QLDTs.Add(phim);
                db.SaveChanges();
                load();
            }

            Reset();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (lv_ds.SelectedItems.Count > 0)
                {
                    lv_ds.Items.Remove(lv_ds.SelectedItems[0]);
                    db.SaveChanges();
                }
            }
            else
            {
                txtMaDon.Text = "";
                txtTenPhim.Text = "";
                txtQuocGia.Text = "";
                dTNgayCongChieu.Value = DateTime.Now;
                txtDTQD.Text = "";
                txtPTXCDB.Text = "";
                txtPTGD.Text = "";
            }
            Reset();
        }
    }
}
