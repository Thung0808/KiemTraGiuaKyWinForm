using PhungTrongHung_2280601321.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PhungTrongHung_2280601321
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ProductContextDB context = new ProductContextDB();
                List<LoaiSP> listLoaiSP = context.LoaiSP.ToList();
                List<SanPham> listSanPham = context.SanPham.ToList();
                FillLoaiSPCombobox(listLoaiSP);
                BindGrid(listSanPham);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillLoaiSPCombobox(List<LoaiSP> listLoaiSP)
        {
            this.cboLoaiSP.DataSource = listLoaiSP;
            this.cboLoaiSP.DisplayMember = "TenLoai";
            this.cboLoaiSP.ValueMember = "MaLoai";
        }
        private void BindGrid(List<SanPham> listSanPham)
        {
            dgvSanPham.Rows.Clear();

            foreach (var item in listSanPham)
            {
                int index = dgvSanPham.Rows.Add();
                dgvSanPham.Rows[index].Cells[0].Value = item.MaSP;
                dgvSanPham.Rows[index].Cells[1].Value = item.TenSP;
                dgvSanPham.Rows[index].Cells[2].Value = item.NgayNhap.ToString();
                dgvSanPham.Rows[index].Cells[3].Value = item.LoaiSP.TenLoai;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                SanPham newSanPham = new SanPham()
                {
                    MaSP = txtMaSP.Text,
                    TenSP = txtTenSP.Text,
                    NgayNhap = dtNgayNhap.Value,
                    MaLoai = (cboLoaiSP.SelectedValue.ToString())
                };

                using (ProductContextDB context = new ProductContextDB())
                {

                    context.SanPham.Add(newSanPham);
                    context.SaveChanges();
                }
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadSanPhamToListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //hanhDongHienTai = HanhDong.Them;
            //MessageBox.Show("Đã sẵn sàng thêm sản phẩm. Nhấn Lưu để xác nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void LoadSanPhamToListView()
        {
            try
            {

                using (ProductContextDB context = new ProductContextDB())
                {
                    List<SanPham> listSanPham = context.SanPham.ToList();

                    dgvSanPham.Rows.Clear();

                    foreach (var sp in listSanPham)
                    {
                        int index = dgvSanPham.Rows.Add();

                        dgvSanPham.Rows[index].Cells[0].Value = sp.MaSP;
                        dgvSanPham.Rows[index].Cells[1].Value = sp.TenSP;
                        dgvSanPham.Rows[index].Cells[2].Value = sp.NgayNhap.ToString();
                        dgvSanPham.Rows[index].Cells[3].Value = sp.LoaiSP.TenLoai;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];
                txtMaSP.Text = row.Cells[0].Value.ToString();
                txtTenSP.Text = row.Cells[1].Value.ToString();
                dtNgayNhap.Value = DateTime.Parse(row.Cells[2].Value.ToString());
                cboLoaiSP.Text = row.Cells[3].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (ProductContextDB context = new ProductContextDB())
                {
                    string maSP = txtMaSP.Text;
                    SanPham sp = context.SanPham.SingleOrDefault(x => x.MaSP == maSP);

                    if (sp != null)
                    {
                        sp.TenSP = txtTenSP.Text;
                        sp.NgayNhap = dtNgayNhap.Value;
                        sp.MaLoai = cboLoaiSP.SelectedValue.ToString();

                        context.SaveChanges();
                        MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSanPhamToListView();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //hanhDongHienTai = HanhDong.Sua;
            //MessageBox.Show("Đã sẵn sàng sửa sản phẩm. Nhấn Lưu để xác nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {

                    using (ProductContextDB context = new ProductContextDB())
                    {
                        string maSP = txtMaSP.Text;
                        SanPham sp = context.SanPham.SingleOrDefault(x => x.MaSP == maSP);

                        if (sp != null)
                        {
                            context.SanPham.Remove(sp);
                            context.SaveChanges();

                            MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadSanPhamToListView();

                            ClearInputFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //hanhDongHienTai = HanhDong.Xoa;
            //MessageBox.Show("Đã sẵn sàng xóa sản phẩm. Nhấn Lưu để xác nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ClearInputFields()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            dtNgayNhap.Value = DateTime.Now;
            cboLoaiSP.SelectedIndex = -1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string tenSP = txtTenSP.Text.Trim();

                if (string.IsNullOrWhiteSpace(tenSP))
                {
                    MessageBox.Show("Vui lòng nhập tên sản phẩm để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (ProductContextDB context = new ProductContextDB())
                {
                    var listSanPham = context.SanPham
                                            .Where(sp => sp.TenSP.Contains(tenSP))
                                            .ToList();
                    if (listSanPham.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm nào với tên: " + tenSP, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    BindGrid(listSanPham);

                    // Hiển thị tên sản phẩm tìm thấy đầu tiên vào TextBox
                    txtTenSPFound.Text = listSanPham[0].TenSP;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //private enum HanhDong
        //{
        //    None,
        //    Them,
        //    Sua,
        //    Xoa
        //}

        //private HanhDong hanhDongHienTai = HanhDong.None;



        //private void button6_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (ProductContextDB context = new ProductContextDB())
        //        {
        //            if (hanhDongHienTai == HanhDong.None)
        //            {
        //                MessageBox.Show("Không có hành động nào để thực hiện.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }

        //            if (hanhDongHienTai == HanhDong.Them)
        //            {
        //                // Thêm sản phẩm mới
        //                SanPham newSanPham = new SanPham()
        //                {
        //                    MaSP = txtMaSP.Text,
        //                    TenSP = txtTenSP.Text,
        //                    NgayNhap = dtNgayNhap.Value,
        //                    MaLoai = cboLoaiSP.SelectedValue.ToString()
        //                };
        //                context.SanPham.Add(newSanPham);
        //                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            else if (hanhDongHienTai == HanhDong.Sua)
        //            {
        //                // Sửa sản phẩm đã chọn
        //                string maSP = txtMaSP.Text;
        //                SanPham sp = context.SanPham.SingleOrDefault(x => x.MaSP == maSP);
        //                if (sp != null)
        //                {
        //                    sp.TenSP = txtTenSP.Text;
        //                    sp.NgayNhap = dtNgayNhap.Value;
        //                    sp.MaLoai = cboLoaiSP.SelectedValue.ToString();
        //                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Không tìm thấy sản phẩm để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                    return;
        //                }
        //            }
        //            else if (hanhDongHienTai == HanhDong.Xoa)
        //            {
        //                // Xóa sản phẩm đã chọn
        //                string maSP = txtMaSP.Text;
        //                SanPham sp = context.SanPham.SingleOrDefault(x => x.MaSP == maSP);
        //                if (sp != null)
        //                {
        //                    context.SanPham.Remove(sp);
        //                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Không tìm thấy sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                    return;
        //                }
        //            }

        //            // Lưu thay đổi vào cơ sở dữ liệu
        //            context.SaveChanges();

        //            // Tải lại dữ liệu lên ListView
        //            LoadSanPhamToListView();

        //            // Xóa trạng thái hành động
        //            hanhDongHienTai = HanhDong.None;

        //            // Xóa các trường nhập liệu
        //            ClearInputFields();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
    }
}
