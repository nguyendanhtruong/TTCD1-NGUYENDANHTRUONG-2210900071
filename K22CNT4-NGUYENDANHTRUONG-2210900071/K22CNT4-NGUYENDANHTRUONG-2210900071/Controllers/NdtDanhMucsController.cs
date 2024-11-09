using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using K22CNT4_NGUYENDANHTRUONG_2210900071.Models;

namespace K22CNT4_NGUYENDANHTRUONG_2210900071.Controllers
{
    public class NdtDanhMucsController : Controller
    {
        private Entities db = new Entities();

        // GET: NdtDanhMucs
        public ActionResult NdtIndex(string searchString)
        {
            var danhMucs = from d in db.DanhMucs
                           select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                danhMucs = danhMucs.Where(d => d.TenDanhMuc.Contains(searchString));
            }

            return View(danhMucs.ToList());
        }

        // GET: NdtDanhMucs/NdtDetails/5
        public ActionResult NdtDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // GET: NdtDanhMucs/NdtCreate
        public ActionResult NdtCreate()
        {
            return View();
        }

        // POST: NdtDanhMucs/NdtCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtCreate([Bind(Include = "ID,TenDanhMuc,MoTa")] DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                db.DanhMucs.Add(danhMuc);
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }

            return View(danhMuc);
        }

        // GET: NdtDanhMucs/NdtEdit/5
        public ActionResult NdtEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: NdtDanhMucs/NdtEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtEdit([Bind(Include = "ID,TenDanhMuc,MoTa")] DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhMuc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }
            return View(danhMuc);
        }

        // GET: NdtDanhMucs/NdtDelete/5
        public ActionResult NdtDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: NdtDanhMucs/NdtDelete/5
        [HttpPost, ActionName("NdtDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhMuc danhMuc = db.DanhMucs.Find(id);

            // Kiểm tra xem danh mục có chứa sản phẩm không trước khi xóa
            var productsInCategory = db.SanPhams.Where(s => s.ID_DanhMuc == id).ToList();
            if (productsInCategory.Any())
            {
                // Nếu có sản phẩm, không xóa danh mục và thông báo lỗi
                ViewBag.ErrorMessage = "Danh mục này có sản phẩm, không thể xóa.";
                return View(danhMuc);
            }

            // Xóa danh mục nếu không có sản phẩm
            db.DanhMucs.Remove(danhMuc);
            db.SaveChanges();
            return RedirectToAction("NdtIndex");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
