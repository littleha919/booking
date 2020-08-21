using slnLLWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace slnLLWP.Controllers
{
    public class tryPetController : Controller
    {
        // GET: tryPet
        public ActionResult List()
        {
            IQueryable<tTryPetTable> tryPets = null;
            string keyword = Request.Form["txtKeyword"];
            if (string.IsNullOrEmpty(keyword))
            {
                tryPets = from p in (new dbLLWPEntities1()).tTryPetTable
                          select p;
            }
            else
            {
                tryPets = from p in (new dbLLWPEntities1()).tTryPetTable
                          where p.fTryPetName.Contains(keyword) || p.fTryPetNum.Equals(keyword)
                          select p;
            }
            return View(tryPets);
        }
        public ActionResult ShowList()
        {
            return RedirectToAction("List");
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(tTryPetTable p)
        {
            //fImage是一開始接要上傳的檔案，fTryPetPhoto是資料庫的資料放圖片的欄位
            if(p.fImage != null)//若有要上傳的檔案(此處不能用p.fTryPetPhoto，因為創新的前本來就是null)
            {
                string photoName = Guid.NewGuid().ToString() + Path.GetExtension(p.fImage.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/tryPetPic/"), photoName);
                p.fImage.SaveAs(path);
                p.fTryPetPhoto = "../Content/tryPetPic/" + photoName;
            }
            dbLLWPEntities1 db = new dbLLWPEntities1();
            if (p.fTryPetPhoto != null)
            {
                db.tTryPetTable.Add(p);
                db.SaveChanges();            
            }

            return RedirectToAction("List");
        }

        //public ActionResult Create(tTryPetTable p)
        //{
        //    dbLLWPEntities1 db = new dbLLWPEntities1();
        //    if (p.fTryPetPhoto != null)
        //    {
        //        db.tTryPetTable.Add(p);
        //        db.SaveChanges();
        //        return RedirectToAction("List");
        //    }
        //    else
        //    {
        //        TempData["message"] = "請上傳照片";
        //    }
        //    return View();

        //}

        //GET:data
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            dbLLWPEntities1 db = new dbLLWPEntities1();
            tTryPetTable p = db.tTryPetTable.FirstOrDefault(m => m.fTryPetId == id);
            return View(p);
        }
        
        [HttpPost]
        public ActionResult Edit(tTryPetTable p)
        {
            if (string.IsNullOrEmpty(p.fTryPetName))
                return RedirectToAction("List");

            dbLLWPEntities1 db = new dbLLWPEntities1();
            tTryPetTable pNew = db.tTryPetTable.FirstOrDefault(m => m.fTryPetId == p.fTryPetId);

            if(pNew != null)
            {
                //照片檔案上傳，有新檔案要上傳(p.fImage!=null)才執行，否則會有例外錯誤
                if (p.fTryPetPhoto != null && p.fImage !=null)
                {
                    string photoName = Guid.NewGuid().ToString() + Path.GetExtension(p.fImage.FileName);
                    //string photoName = Path.GetExtension(p.fImage.FileName);//會沒檔名
                    var path = Path.Combine(Server.MapPath("~/Content/tryPetPic/"), photoName);
                    p.fImage.SaveAs(path);
                    p.fTryPetPhoto = "../Content/tryPetPic/" + photoName;
                    db.tTryPetTable.Add(p);
                    db.SaveChanges();
                }

                pNew.fTryPetNum = p.fTryPetNum;
                pNew.fTryPetName = p.fTryPetName;
                pNew.fTryPetVar = p.fTryPetVar;
                pNew.fTryPetAge = p.fTryPetAge;
                pNew.fTryPetWei = p.fTryPetWei;
                pNew.fTryPetSex = p.fTryPetSex;
                pNew.fTryPetNature = p.fTryPetNature;
                pNew.fTryPetVac = p.fTryPetVac;
                pNew.fTryPetFix = p.fTryPetFix;
                pNew.fTryPetPhoto = p.fTryPetPhoto;
                db.SaveChanges();
            }          
            return RedirectToAction("List");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            dbLLWPEntities1 db = new dbLLWPEntities1();
            tTryPetTable p = db.tTryPetTable.FirstOrDefault(m => m.fTryPetId == id);
            if(p != null)
            {
                db.tTryPetTable.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}