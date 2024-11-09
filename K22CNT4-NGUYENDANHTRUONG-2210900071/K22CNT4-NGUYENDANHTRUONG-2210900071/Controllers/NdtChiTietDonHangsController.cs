using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using K22CNT4_NGUYENDANHTRUONG_2210900071.Models;

namespace K22CNT4_NGUYENDANHTRUONG_2210900071.Controllers
{
    public class NdtChiTietDonHangsController : Controller
    {
        private Entities db = new Entities();

        // GET: ChiTietDonHangs
        public ActionResult NdtIndex()
        {
            var chiTietDonHangs = db.ChiTietDonHangs.Include(c => c.DonHang).Include(c => c.SanPham);
            return View(chiTietDonHangs.ToList());
        }

        // GET: ChiTietDonHangs/NdtDetails/5
        public ActionResult NdtDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonHang chiTietDonHang = db.ChiTietDonHangs.Find(id);
            if (chiTietDonHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/NdtCreate
        public ActionResult NdtCreate()
        {
            ViewBag.ID_DonHang = new SelectList(db.DonHangs, "ID", "TrangThai");
            ViewBag.ID_SanPham = new SelectList(db.SanPhams, "ID", "TenSanPham");
            return View();
        }

        // POST: ChiTietDonHangs/NdtCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtCreate([Bind(Include = "ID,ID_DonHang,ID_SanPham,SoLuong,Gia")] ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietDonHangs.Add(chiTietDonHang);
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }

            ViewBag.ID_DonHang = new SelectList(db.DonHangs, "ID", "TrangThai", chiTietDonHang.ID_DonHang);
            ViewBag.ID_SanPham = new SelectList(db.SanPhams, "ID", "TenSanPham", chiTietDonHang.ID_SanPham);
            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/NdtEdit/5
        public ActionResult NdtEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonHang chiTietDonHang = db.ChiTietDonHangs.Find(id);
            if (chiTietDonHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_DonHang = new SelectList(db.DonHangs, "ID", "TrangThai", chiTietDonHang.ID_DonHang);
            ViewBag.ID_SanPham = new SelectList(db.SanPhams, "ID", "TenSanPham", chiTietDonHang.ID_SanPham);
            return View(chiTietDonHang);
        }

        // POST: ChiTietDonHangs/NdtEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtEdit([Bind(Include = "ID,ID_DonHang,ID_SanPham,SoLuong,Gia")] ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietDonHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }
            ViewBag.ID_DonHang = new SelectList(db.DonHangs, "ID", "TrangThai", chiTietDonHang.ID_DonHang);
            ViewBag.ID_SanPham = new SelectList(db.SanPhams, "ID", "TenSanPham", chiTietDonHang.ID_SanPham);
            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/NdtDelete/5
        public ActionResult NdtDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonHang chiTietDonHang = db.ChiTietDonHangs.Find(id);
            if (chiTietDonHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDonHang);
        }

        // POST: ChiTietDonHangs/NdtDelete/5
        [HttpPost, ActionName("NdtDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietDonHang chiTietDonHang = db.ChiTietDonHangs.Find(id);
            if (chiTietDonHang != null)
            {
                db.ChiTietDonHangs.Remove(chiTietDonHang);
                db.SaveChanges();
            }
            return RedirectToAction("NdtIndex");
        }

        // Dispose the context properly
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
