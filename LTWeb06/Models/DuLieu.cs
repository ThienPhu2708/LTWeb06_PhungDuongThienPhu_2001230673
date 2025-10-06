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
		static string strcon = "Data Source=LAPTOP-L3MTTMQ3\\SQLEXPRESS01;Initial Catalog=QL_NHASACH_1;Integrated Security=True;TrustServerCertificate=True;";


		SqlConnection con = new SqlConnection(strcon);

		public List<SanPham> dsSP = new List<SanPham>();
		public List<Loai> dsLoai = new List<Loai>();
        public List<KhachHang> dsKH = new List<KhachHang>();

        public DuLieu() {
			ThietLap_Loai();
			ThietLap_SP();
            ThietLap_KhachHang();
        }

		public void ThietLap_Loai() { 
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


        //xem chi tiet loai san pham

        public Loai XemChiTiet_Loai(string MaLoai)
        {
            Loai loai = null; // để phân biệt khi không tìm thấy
            string sqlScript = string.Format("SELECT * FROM tblLoai WHERE MaLoai = '{0}'", MaLoai);
            SqlDataAdapter da = new SqlDataAdapter(sqlScript, con);

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


        //xem danh sach san pham theo ma loai
        public List<SanPham> SanPhamTheoLoai(string MaLoai)
		{
			List<SanPham> dsTheoLoai = new List<SanPham>();
			string sqlScript = string.Format("SELECT * FROM tblSanPham WHERE MaLoai = '{0}'", MaLoai);
			SqlDataAdapter da = new SqlDataAdapter(sqlScript, con);

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
                dsTheoLoai.Add(sanpham);
            }
			return dsTheoLoai;
        }



        //xem chi tiet san pham theo maSP
        public SanPham XemChiTiet_SP(string maSP)
        {
            SanPham sp = null;
            string sqlScript = string.Format("SELECT * FROM tblSanPham WHERE MaSP = '{0}'", maSP);
            SqlDataAdapter da = new SqlDataAdapter(sqlScript, con);

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

        // Tìm kiếm danh sách sản phẩm theo tên gần đúng
        public List<SanPham> TimKiemSanPham(string tuKhoa)
        {
            List<SanPham> dsKetQua = new List<SanPham>();

            // Sử dụng LIKE và SqlParameter để tìm kiếm gần đúng và an toàn
            string sqlScript = "SELECT * FROM tblSanPham WHERE TenSP LIKE @TuKhoa";

            SqlCommand cmd = new SqlCommand(sqlScript, con);

            // Thêm tham số @TuKhoa, bọc từ khóa trong dấu % để tìm kiếm bất kỳ đâu trong tên
            // Ví dụ: nếu người dùng nhập "phone", SQL sẽ tìm kiếm "%phone%"
            cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");

            SqlDataAdapter da = new SqlDataAdapter(cmd); // Sử dụng SqlCommand đã có tham số
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                var sp = new SanPham();
                sp.MaSP = (dr["MASP"]).ToString();
                sp.TenSP = dr["TENSP"].ToString();
                sp.MaNSX = dr["MaNSX"].ToString();
                sp.DonGia = (decimal)dr["Gia"];
                sp.GhiChu = dr["GhiChu"].ToString();
                dsKetQua.Add(sp);
            }
            return dsKetQua;
        }

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