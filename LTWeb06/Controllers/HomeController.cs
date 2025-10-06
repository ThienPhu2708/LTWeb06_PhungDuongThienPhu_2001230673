using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTWeb06.Models;

namespace LTWeb06.Controllers
{
    public class HomeController : Controller
    {
        DuLieu csdl = new DuLieu();


        //danh sach san pham theo loai
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

        // chi tiet san pham
        public ActionResult ChiTiet_SP(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            SanPham sp = csdl.XemChiTiet_SP(id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp); // trả về 1 sản phẩm
        }


        //đăng ký
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(string TenKhachHang, string SoDienThoai, string MatKhau)
        {
            var kh = new KhachHang
            {
                MaKH = "KH" + (csdl.dsKH.Count() + 1).ToString("00"),
                TenKH = TenKhachHang,
                SDT = SoDienThoai,
                MatKhau = MatKhau
            };
            csdl.dsKH.Add(kh);
            return RedirectToAction("Login");
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(string SoDienThoai, string MatKhau)
        {
            var kh = csdl.dsKH.FirstOrDefault(x => x.SDT == SoDienThoai && x.MatKhau == MatKhau);

            if (kh != null)
            {
                Session["MaKH"] = kh.MaKH;
                Session["TenKhachHang"] = kh.TenKH;

                return RedirectToAction("SanPham_Loai");
            }

            ViewBag.Error = "Sai số điện thoại hoặc mật khẩu!";
            return View();
        }



        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }


        // Tìm kiếm sản phẩm
        // Tham số tuKhoa sẽ nhận giá trị từ ô textbox trên View
        public ActionResult TimKiemSanPham(string tuKhoa)
        {
            List<SanPham> dsKetQua = new List<SanPham>();
            ViewBag.TuKhoa = tuKhoa; // Lưu lại từ khóa để hiển thị trên ô input

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                // Nếu có từ khóa, gọi hàm tìm kiếm trong Model
                dsKetQua = csdl.TimKiemSanPham(tuKhoa);
                ViewBag.TongSo = dsKetQua.Count;
            }
            else
            {
                // Nếu không có từ khóa (lần đầu vào trang), danh sách rỗng
                ViewBag.TongSo = 0;
            }

            // Trả về danh sách kết quả tìm kiếm cho View
            return View(dsKetQua);
        }




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