using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcWatchStore.Models;

using PagedList;
using PagedList.Mvc;
using System.Net.Mail;
using System.Web.Helpers;

namespace MvcWatchStore.Controllers
{
    public class WatchStoreController : Controller
    {
        // GET: WatchStore
        dbWatchStoreManegerDataContext data = new dbWatchStoreManegerDataContext();
        private List<WATCH> LayDongHoMoi(int count)
        {
            
            return data.WATCHes.OrderByDescending(a => a.DateUp).Take(count).ToList();
        }
        
        public ActionResult Index(int ? page)
        {
            //tao bien quy dinh so sp tren moi trang
            int pageSize = 4;
            //tao bien so trang
            int pageNum = (page ?? 1);
            //lay top 5 dong ho ban chay nhat
            var donghomoi = LayDongHoMoi(15);
            return View(donghomoi.ToPagedList(pageNum,pageSize));
        }
        public ActionResult ThuongHieu()
        {
            var thuonghieu = from Brand in data.Brands select Brand;
            return PartialView(thuonghieu);
        }
        
        public ActionResult SPTheoThuongHieu(int id,int ?page)
        {
            int pageSize = 6;
            //tao bien so trang
            int pageNum = (page ?? 1);
            var dongho = from w in data.WATCHes where w.IdBrand == id select w;
            return View(dongho.ToPagedList(pageNum, pageSize));
        }
        public ActionResult ChiTietSP(int id)
        {
            var chitietdongho = from wc in data.ChiTietDongHos
                                join a in data.WATCHes on wc.IdWatch equals a.IdWatch into wca
                                where wc.IdWatch == id
                                select wc;
            return View(chitietdongho.ToList());
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        { 
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string name ,string pass, string email ,string message)
        {
            try
            {
                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = "smtp.gmail.com";
                //gmail port to send emails  
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                WebMail.UserName = email;
                WebMail.Password = pass;

                //Sender email address.  
                WebMail.From = email;

                //Send email  
                WebMail.Send(to: "buiquangductri@gmail.com", subject: "góp ý", body: message, cc: null, bcc: null, isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }

            return View();
        }
        public ActionResult Th()
        {
            var thuonghieu = from Brand in data.Brands select Brand;
            return PartialView(thuonghieu);
        }
    }
}