using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcWatchStore.Models
{
    public class Giohang
    {
        dbWatchStoreManegerDataContext db = new dbWatchStoreManegerDataContext();
        public int iIdDongho { get; set; }
        public string sTendongho { get; set; }
        public string sAnhbia { get; set; }
        public Double dDongia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; } 
        }

        public Giohang(int IdWatch)
        {
            iIdDongho = IdWatch;
            WATCH dh = db.WATCHes.Single(n => n.IdWatch == iIdDongho);
            sTendongho = dh.WatchName;
            sAnhbia = dh.Img;
            dDongia = double.Parse(dh.Cost.ToString());
            iSoluong = 1;
        }
    }
}