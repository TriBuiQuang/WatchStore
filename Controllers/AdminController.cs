using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcWatchStore.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Linq;

namespace MvcWatchStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbWatchStoreManegerDataContext db = new dbWatchStoreManegerDataContext();
       
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Admin");
        }
        public ActionResult dongho(int ? page,string search)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                
                return View(db.WATCHes.Where(n=>n.WatchName.StartsWith(search)||search==null).ToList().OrderBy(n => n.IdWatch).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Admin");
        }
        public ActionResult DonghoSXTen(int ?page)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(db.WATCHes.ToList().OrderBy(n => n.WatchName).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Admin");
        }
        public ActionResult DonghoSXGia(int? page)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(db.WATCHes.ToList().OrderBy(n => n.Cost).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "phải nhập mật khẩu";
            }
            else {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad.HoTen;
                    return RedirectToAction("Index", "Admin");
                }
                else {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["Taikhoanadmin"] = null;
            return RedirectToAction("Login", "Admin");
        }
        [HttpGet]
        public ActionResult Themdongho()
        {
            if (Session["Taikhoanadmin"] != null)
            {
                //dua du lieu vao drop down list
                //lay ds tu table thuonghieu , sap xep tang dan theo ten thuong hieu, 
                //chon lay gia tri math, hien thi ten thuong hieu
                ViewBag.IdBrand = new SelectList(db.Brands.ToList().OrderBy(n => n.BrandName), "IdBrand", "BrandName");
                return View();
            }
            return RedirectToAction("Login","Admin");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themdongho(WATCH dongho,HttpPostedFileBase fileupload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.IdBrand = new SelectList(db.Brands.ToList().OrderBy(n => n.BrandName), "IdBrand", "BrandName");
            //ktra duong dan file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "vui lòng chọn ảnh bìa";
                return View();
            }
            //them vao csdl
            else {
                if (ModelState.IsValid)
                {
                    //luu ten file , dung thu vien System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //luu duong dan file
                    var path = Path.Combine(Server.MapPath("~/images/products"), fileName);
                    //kiem tra hien ton tai chua?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình đã được tồn tại";
                    }
                    else
                    {
                        //luu hinh anh vao duong dan
                        fileupload.SaveAs(path);
                    }
                    dongho.Img = fileName;
                    db.WATCHes.InsertOnSubmit(dongho);
                    db.SubmitChanges();
                }
                return RedirectToAction("dongho");
            }      
        }

        public ActionResult Chitietdongho(int id)
        {
            if (Session["Taikhoanadmin"] != null)
            { 
                ChiTietDongHo ctdh = db.ChiTietDongHos.SingleOrDefault(n => n.IdWatch == id);    
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
        public ActionResult Suachitietdongho(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                ChiTietDongHo ctdh = db.ChiTietDongHos.SingleOrDefault(n => n.IdChiTietDongHo == id);
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
        public ActionResult Suachitietdongho(FormCollection f)
        {
            ChiTietDongHo ctdh = db.ChiTietDongHos.First(d => d.IdWatch == int.Parse(f["IdWatch"]));
            if (ModelState.IsValid)
            {
                ctdh.BaoHanh = int.Parse(f["BaoHanh"]);
                ctdh.XuatXu = f["BaoHanh"];
                ctdh.Tinhtrang = int.Parse(f["Tinhtrang"]);
                ctdh.Mota = f["Mota"];
                UpdateModel(ctdh);
                db.SubmitChanges();
            }
            return RedirectToAction("dongho");
        }
        

        [HttpGet]
        public ActionResult Xoadongho(int id)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                WATCH dh = db.WATCHes.SingleOrDefault(n => n.IdWatch == id);
            ViewBag.IdDongho = dh.IdWatch;
            if(dh==null){
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost,ActionName("Xoadongho")]
       
        public ActionResult Xacnhanxoa(int id)
        {
            WATCH dh = db.WATCHes.SingleOrDefault(n => n.IdWatch == id);
            
            ViewBag.IdDongho = dh.IdWatch;
         
            
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.WATCHes.DeleteOnSubmit(dh);
            db.SubmitChanges();
            return RedirectToAction("dongho");
        }

        [HttpGet]
        public ActionResult Suadongho(int id)
        {
            
            if (Session["Taikhoanadmin"]!=null)
            { 
                WATCH dh = db.WATCHes.SingleOrDefault(n => n.IdWatch == id);
                if (dh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.IdBrand = new SelectList(db.Brands.ToList().OrderBy(n => n.BrandName), "IdBrand", "BrandName", dh.IdBrand);
                return View(dh);
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suadongho(HttpPostedFileBase fileupload,FormCollection f)
        {
            WATCH dongho = db.WATCHes.First(d => d.IdWatch == int.Parse(f["IdWatch"]));
            ViewBag.IdBrand = new SelectList(db.Brands.ToList().OrderBy(n => n.BrandName), "IdBrand", "BrandName");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dongho.WatchName = f["WatchName"];
                    dongho.Cost = decimal.Parse(f["Cost"]);
                    dongho.DateUp = DateTime.Now;
                    dongho.number = int.Parse(f["number"]);
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/products"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình đã được tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    dongho.Img = fileName;
                    UpdateModel(dongho);
                    db.SubmitChanges();
                }
                return RedirectToAction("dongho");
            }
            
        }
        
        public ActionResult Thuonghieu(int ?page)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 10;
                return View(db.Brands.ToList().OrderBy(n => n.IdBrand).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpGet]
        public ActionResult Themthuonghieu()
        {
            if (Session["Taikhoanadmin"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themthuonghieu(Brand thuonghieu)
        {
            if (ModelState.IsValid)
            { 
                db.Brands.InsertOnSubmit(thuonghieu);
                db.SubmitChanges();
            }
                return RedirectToAction("Thuonghieu");   
        }
        [HttpGet]
        public ActionResult Xoathuonghieu(int id)
        {
            if (Session["Taikhoanadmin"] != null)
            {
                Brand th = db.Brands.SingleOrDefault(n => n.IdBrand == id);

                if (th == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(th);
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost, ActionName("Xoathuonghieu")]

        public ActionResult Xacnhanxoathuonghieu(int id)
        {
            Brand th = db.Brands.SingleOrDefault(n => n.IdBrand == id);
            if (th == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Brands.DeleteOnSubmit(th);
            db.SubmitChanges();
            return RedirectToAction("Thuonghieu");
        }
        [HttpGet]
        public ActionResult Suathuonghieu(int id)
        {

            if (Session["Taikhoanadmin"] != null)
            {
                Brand th = db.Brands.SingleOrDefault(n => n.IdBrand == id);
                if (th == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }        
                return View(th);
            }
            return RedirectToAction("Login", "Admin");
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suathuonghieu(FormCollection f)
        {
            Brand th = db.Brands.First(d => d.IdBrand == int.Parse(f["IdBrand"]));
                if (ModelState.IsValid)
                {
                    th.BrandName = f["BrandName"];   
                    UpdateModel(th);
                    db.SubmitChanges();
                }
                return RedirectToAction("Thuonghieu");
        }
    }
}