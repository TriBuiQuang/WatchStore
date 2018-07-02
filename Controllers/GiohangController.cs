using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcWatchStore.Models;

namespace MvcWatchStore.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Gio
        //tao doi tuong data chua dữ liệu từ model dbWatchStore đã tạo
        dbWatchStoreManegerDataContext db = new dbWatchStoreManegerDataContext();
        //lay gio hang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang==null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        //them hang vao gio
        public ActionResult ThemGiohang(int iIdDongho, string strURL)
        {
            //lay ra session gio hang
            List<Giohang> lstGiohang = Laygiohang();
            //ktra dh này có tồn tại trong session giỏ hàng chưa ?
            Giohang sanpham = lstGiohang.Find(n=>n.iIdDongho == iIdDongho);
            if (sanpham == null)
            {
                sanpham = new Giohang(iIdDongho);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        public int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n=>n.iSoluong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        
        //xaydung trang gio hang
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index","WatchStore");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        //tạo partial view để hiển thị thông tin giỏ hàng
        public ActionResult GiohangPartial()
        {
            if (TongSoLuong()==0)
            {
                ViewBag.Tongsoluong = 0;
                return PartialView();
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iIdSP)
        {
            //lay ra session gio hang
            List<Giohang> lstGiohang = Laygiohang();
            //ktra dh này có tồn tại trong session giỏ hàng chưa ?
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iIdDongho == iIdSP);
            if (sanpham != null)
            {
                
                lstGiohang.RemoveAll(n=>n.iIdDongho == iIdSP);
                return RedirectToAction("Giohang");
            }
            if(lstGiohang.Count==0)
            {
                return RedirectToAction("Index","WatchStore");
            }  
            return RedirectToAction("GioHang");
        }
        //cap nhat gio hang
        public ActionResult CapnhatGiohang(int iIdSP, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang(); 
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iIdDongho == iIdSP);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["UserName"] == null || Session["UserName"].ToString() == "")
            {
                return RedirectToAction("DangNhap","NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index","WatchStore");
            }
            List<Giohang> lstgiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstgiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang ddh = new DonHang();
            KhachHang kh = (KhachHang)Session["UserName"];
            List<Giohang> gh = Laygiohang();
            ddh.IdUser = kh.IdKH;
            ddh.NgayDH = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiao = false;
            ddh.DaThanhToan = false;
            db.DonHangs.InsertOnSubmit(ddh);
            db.SubmitChanges();
            //them chi tiet don hang
            foreach (var item in gh)
            {
                ChiTietDatHang ctdh = new ChiTietDatHang();
                ctdh.IdDonHang = ddh.IdDonHang;
                ctdh.IdWatch = item.iIdDongho;
                ctdh.Number = item.iSoluong;
                ctdh.Cost = (decimal)item.dDongia;
                db.ChiTietDatHangs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}