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
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        dbWatchStoreManegerDataContext db = new dbWatchStoreManegerDataContext();
        [HttpGet]
        public ActionResult TimKiem(int ?page,string search)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            /*if (ModelState.IsValid)
            {
                TimKiem tk = db.TimKiems.Where(s=>s.Key.Equals(search)).SingleOrDefault();
                if (tk != null)
                {
                    tk.Key = search;
                    tk.SoLan = tk.SoLan++;
                    db.TimKiems.InsertOnSubmit(tk);
                    db.SubmitChanges();
                }
                else
                {
                    tk.Key = search;
                    tk.SoLan=tk.SoLan++;
                    UpdateModel(tk);
                    db.SubmitChanges();
                }      
            }*/
            return View(db.WATCHes.Where(n => n.WatchName.StartsWith(search) || search == null).ToList().OrderBy(n => n.IdWatch).ToPagedList(pageNumber, pageSize));           
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TimKiem(string search)
        {
            
            return View();
        }
    }
}