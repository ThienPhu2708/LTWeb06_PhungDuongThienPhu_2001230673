using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LTWeb06.Models
{
    public class DuLieu
    {
        //====================================================
        // 🔹 1. KẾT NỐI CSDL
        //====================================================
        static string strcon = "Data Source=LAPTOP-L3MTTMQ3\\SQLEXPRESS01;Initial Catalog=QL_NHASACH_1;Integrated Security=True;TrustServerCertificate=True;";
        SqlConnection con = new SqlConnection(strcon);

        //====================================================
        // 🔹 2. DANH SÁCH DỮ LIỆU (BỘ NHỚ TẠM)
        //====================================================
        public List<SanPham> dsSP = new List<SanPham>();
        public List<Loai> dsLoai = new List<Loai>();
        public List<KhachHang> dsKH = new List<KhachHang>();

        //====================================================
        // 🔹 3. HÀM KHỞI TẠO - TẢI DỮ LIỆU BAN ĐẦU
        //====================================================
        public DuLieu()
        {
            ThietLap_Loai();
            ThietLap_SP();
            ThietLap_KhachHang();
        }

        //====================================================
        // 🔹 4. CÁC HÀM THIẾT LẬP DỮ LIỆU BAN ĐẦU
        //====================================================

        // 4.1. Loại sản phẩm
        public void ThietLap_Loai()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tblLoai", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                var loai = new Loai();
                loai.MaLoai = dr["MaLoai"].ToString();
                loai.TenLoai = dr["TenLoai"].ToString();
                dsLoai.Add(loai);
            }
        }

        // 4.2. Sản phẩm
        public void ThietLap_SP()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tblSanPham", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                var sanpham = new SanPham();
                sanpham.MaSP = dr["MaSP"].ToString();
                sanpham.TenSP = dr["TenSP"].ToString();
                sanpham.MaNSX = dr["MaNSX"].ToString();
                sanpham.DonGia = Convert.ToDecimal(dr["Gia"]);
                sanpham.Hinh = dr["Hinh"].ToString();
                sanpham.MaLoai = dr["MaLoai"].ToString();
                sanpham.GhiChu = dr["GhiChu"].ToString();
                dsSP.Add(sanpham);
            }
        }

        // 4.3. Khách hàng
        public void ThietLap_KhachHang()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tblKhachHang", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                var kh = new KhachHang();
                kh.MaKH = dr["MaKH"].ToString();
                kh.TenKH = dr["TenKhachHang"].ToString();
                kh.SDT = dr["SoDienThoai"].ToString();
                kh.MatKhau = dr["MatKhau"].ToString();
                dsKH.Add(kh);
            }
        }

        //====================================================
        // 🔹 5. HÀM XEM CHI TIẾT
        //====================================================

        // 5.1. Xem chi tiết loại
        public Loai XemChiTiet_Loai(string MaLoai)
        {
            Loai loai = null;
            string sql = string.Format("SELECT * FROM tblLoai WHERE MaLoai = '{0}'", MaLoai);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                loai = new Loai();
                loai.MaLoai = dt.Rows[0]["MaLoai"].ToString();
                loai.TenLoai = dt.Rows[0]["TenLoai"].ToString();
            }

            return loai;
        }

        // 5.2. Xem chi tiết sản phẩm
        public SanPham XemChiTiet_SP(string maSP)
        {
            SanPham sp = null;
            string sql = string.Format("SELECT * FROM tblSanPham WHERE MaSP = '{0}'", maSP);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                sp = new SanPham();
                sp.MaSP = dt.Rows[0]["MaSP"].ToString();
                sp.TenSP = dt.Rows[0]["TenSP"].ToString();
                sp.MaNSX = dt.Rows[0]["MaNSX"].ToString();
                sp.DonGia = Convert.ToDecimal(dt.Rows[0]["Gia"]);
                sp.Hinh = dt.Rows[0]["Hinh"].ToString();
                sp.MaLoai = dt.Rows[0]["MaLoai"].ToString();
                sp.GhiChu = dt.Rows[0]["GhiChu"].ToString();
            }

            return sp;
        }

        //====================================================
        // 🔹 6. HÀM LỌC & TÌM KIẾM DANH SÁCH
        //====================================================

        // 6.1. Lọc sản phẩm theo loại
        public List<SanPham> SanPhamTheoLoai(string ID)
        {
            List<SanPham> dsTheoLoai = new List<SanPham>();
            string sql = string.Format("SELECT * FROM tblSanPham WHERE MaLoai = '{0}'", ID);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                var sp = new SanPham();
                sp.MaSP = dr["MaSP"].ToString();
                sp.TenSP = dr["TenSP"].ToString();
                sp.MaNSX = dr["MaNSX"].ToString();
                sp.DonGia = Convert.ToDecimal(dr["Gia"]);
                sp.Hinh = dr["Hinh"].ToString();
                sp.MaLoai = dr["MaLoai"].ToString();
                sp.GhiChu = dr["GhiChu"].ToString();
                dsTheoLoai.Add(sp);
            }

            return dsTheoLoai;
        }

        // 6.2. Tìm kiếm sản phẩm theo tên gần đúng
        public List<SanPham> TimKiemSanPham(string tuKhoa)
        {
            List<SanPham> dsKetQua = new List<SanPham>();
            string sql = "SELECT * FROM tblSanPham WHERE TenSP LIKE @TuKhoa";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                var sp = new SanPham();
                sp.MaSP = dr["MASP"].ToString();
                sp.TenSP = dr["TENSP"].ToString();
                sp.MaNSX = dr["MaNSX"].ToString();
                sp.DonGia = (decimal)dr["Gia"];
                sp.GhiChu = dr["GhiChu"].ToString();
                dsKetQua.Add(sp);
            }

            return dsKetQua;
        }

        //====================================================
        // 🔹 7. HÓA ĐƠN CỦA KHÁCH HÀNG
        //====================================================
        public List<HoaDon> LayHoaDonTheoKH(string maKH)
        {
            List<HoaDon> dsHD = new List<HoaDon>();
            string sql = "SELECT * FROM tblHoaDon WHERE MaKH='" + maKH + "'";
            SqlDataAdapter da = new SqlDataAdapter(sql, con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                dsHD.Add(new HoaDon
                {
                    MaHD = dr["MaHD"].ToString(),
                    NgayTao = Convert.ToDateTime(dr["NgayTao"]),
                    MaKH = dr["MaKH"].ToString()
                });
            }

            return dsHD;
        }
    }
}
