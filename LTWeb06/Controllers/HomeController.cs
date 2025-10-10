using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTWeb06.Models;

namespace LTWeb06.Controllers
{
    public class HomeController : Controller
    {
        DuLieu csdl = new DuLieu();


        // ==========================
        // 🔹 PHẦN SẢN PHẨM
        // ==========================

        // Hiển thị danh sách sản phẩm theo loại
        public ActionResult SanPham_Loai(string MaLoai)
        {
            ViewBag.dsLoai = csdl.dsLoai;
            List<SanPham> dsSP;

            if (string.IsNullOrEmpty(MaLoai))
            {
                dsSP = csdl.dsSP;
                ViewBag.TieuDe = "Tất cả sản phẩm";
            }
            else
            {
                dsSP = csdl.SanPhamTheoLoai(MaLoai);
                Loai loai = csdl.XemChiTiet_Loai(MaLoai);
                ViewBag.TieuDe = "Sản phẩm loại: " + loai.TenLoai;
            }

            return View(dsSP);
        }

        // Chi tiết sản phẩm
        public ActionResult ChiTiet_SP(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            SanPham sp = csdl.XemChiTiet_SP(id);
            if (sp == null)
                return HttpNotFound();

            return View(sp);
        }


        // ==========================
        // 🔹 PHẦN TÀI KHOẢN (ĐĂNG NHẬP / ĐĂNG KÝ)
        // ==========================

        // Đăng ký (chỉ hiển thị, không lưu database)
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(string TenKhachHang, string SoDienThoai, string MatKhau)
        {
            ViewBag.ThongBao = "Trang đăng ký chỉ mô phỏng, không lưu dữ liệu!";
            return View();
        }

        // Đăng nhập (chỉ kiểm tra dữ liệu có sẵn trong CSDL)
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(string TenKhachHang, string MatKhau)
        {
            var kh = csdl.dsKH.FirstOrDefault(x => x.TenKH == TenKhachHang && x.MatKhau == MatKhau);

            if (kh != null)
            {
                Session["MaKH"] = kh.MaKH;
                Session["TenKhachHang"] = kh.TenKH;
                return RedirectToAction("SanPham_Loai");
            }

            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }

        // Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }


        // ==========================
        // 🔹 PHẦN KHÁC (TÌM KIẾM, LỊCH SỬ)
        // ==========================

        // Tìm kiếm sản phẩm
        public ActionResult TimKiemSanPham(string tuKhoa)
        {
            List<SanPham> dsKetQua = new List<SanPham>();
            ViewBag.TuKhoa = tuKhoa;

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                dsKetQua = csdl.TimKiemSanPham(tuKhoa);
                ViewBag.TongSo = dsKetQua.Count;
            }
            else
            {
                ViewBag.TongSo = 0;
            }

            return View(dsKetQua);
        }

        // Lịch sử mua hàng (chỉ hiển thị nếu đã đăng nhập)
        public ActionResult LichSu()
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("DangNhap");

            string maKH = Session["MaKH"].ToString();
            var dsHD = csdl.LayHoaDonTheoKH(maKH);

            return View(dsHD);
        }
    }
}
