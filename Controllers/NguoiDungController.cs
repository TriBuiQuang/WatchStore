using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcWatchStore.Models;
using System.Web.Security;

namespace MvcWatchStore.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        dbWatchStoreManegerDataContext db = new dbWatchStoreManegerDataContext();



        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["Ten"];
            var tendn = collection["TenDangNhap"];
            var matkhau = collection["PassWord"];
            var matkhaunhaplai = collection["RePassWord"];
            var email = collection["Email"];
            var diachi = collection["DiaChi"];
            var dienthoai = collection["SoDienThoai"];
            var ngaysinh = $"{collection["NgaySinh"]:mm/dd/yyyy}";
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Nhập Mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Địa chỉ không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Điện thoại không được bỏ trống";
            }
            else {
                kh.Name = hoten;
                kh.UserName = tendn;
                kh.Pass = matkhau;
                kh.Email = email;
                kh.Address = diachi;
                kh.Phone = dienthoai;
                kh.Birth = DateTime.Parse(ngaysinh);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
            
        }
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải điền tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải điền mật khẩu";
            }
            else {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.UserName == tendn && n.Pass == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["UserName"] = kh;
                    
                    return RedirectToAction("Index", "WatchStore");
                }
                else {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["UserName"] = null;
            return RedirectToAction("Index", "WatchStore");
        }
    }
}