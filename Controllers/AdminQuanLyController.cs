using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using MvcWatchStore.Models;

namespace MvcWatchStore.Controllers
{
    public class AdminQuanLyController : Controller
    {
        // GET: AdminQuanLy
        dbWatchStoreManegerDataContext db = new dbWatchStoreManegerDataContext();
        public ActionResult QLUser(int? page, string search)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 10;
                return View(db.KhachHangs.Where(n => n.Name.StartsWith(search)||search==null).ToList().OrderBy(n => n.IdKH).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpGet]
        public ActionResult Themuser()
        {
            if (Session["Taikhoanadmin"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themuser(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
            }
            return RedirectToAction("QLUser");
        }
        public ActionResult QLTimkiem(int? page)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 10;
                return View(db.TimKiems.ToList().OrderBy(n => n.Id).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpGet]
        public ActionResult Suauser(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.IdKH == id);
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suauser(FormCollection f)
        {
            KhachHang kh = db.KhachHangs.First(d => d.IdKH == int.Parse(f["IdKH"]));
            if (ModelState.IsValid)
            {
                UpdateModel(kh);
                db.SubmitChanges();
            }
            return RedirectToAction("QLUser");
        }

        [HttpGet]
        public ActionResult Xoauser(int id)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.IdKH == id);
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost, ActionName("Xoauser")]

        public ActionResult Xacnhanxoauser(int id)
        {
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.IdKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KhachHangs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("QLUser");
        }
        public ActionResult QLDonhang(int ?page)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(db.DonHangs.ToList().OrderBy(n => n.IdDonHang).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpGet]
        public ActionResult SuaDonhang(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                DonHang dh = db.DonHangs.SingleOrDefault(n => n.IdDonHang == id);
                if (dh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(dh);
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaDonhang( FormCollection f)
        {
            DonHang dh = db.DonHangs.First(d => d.IdDonHang == int.Parse(f["IdDonHang"]));
                if (ModelState.IsValid)
                {
                    UpdateModel(dh);
                    db.SubmitChanges();
                }
                return RedirectToAction("QLDonhang");
        }
        public ActionResult Chitietdonhang(int id)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                ChiTietDatHang ctdh = db.ChiTietDatHangs.SingleOrDefault(n => n.IdDonHang == id);
                if (ctdh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ctdh);
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpGet]
        public ActionResult Suachitietdonhang(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                ChiTietDatHang ctdh = db.ChiTietDatHangs.SingleOrDefault(n => n.IdDonHang == id);
                if (ctdh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View(ctdh);
            }
            return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suachitietdonhang(FormCollection f)
        {
            ChiTietDatHang ctdh = db.ChiTietDatHangs.First(d => d.IdDonHang == int.Parse(f["IdDonHang"]));
            if (ModelState.IsValid)
            {

                UpdateModel(ctdh);
                db.SubmitChanges();
            }
            return RedirectToAction("QLDonhang");
        }
        [HttpGet]
        public ActionResult Xoadonhang(int id)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                DonHang dh = db.DonHangs.SingleOrDefault(n => n.IdDonHang == id);
                if (dh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(dh);
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost, ActionName("Xoadonhang")]

        public ActionResult XacnhanxoaDonhang(int id)
        {
            DonHang dh = db.DonHangs.SingleOrDefault(n => n.IdDonHang == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.DonHangs.DeleteOnSubmit(dh);
            db.SubmitChanges();
            return RedirectToAction("QLDonhang");
        }
    }
}