using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using K22CNT4_NGUYENDANHTRUONG_2210900071.Models;

namespace K22CNT4_NGUYENDANHTRUONG_2210900071.Controllers
{
    public class NdtSanPhamsController : Controller
    {
        private readonly Entities db = new Entities();

        // GET: NdtSanPhams
        public ActionResult NdtIndex(int? categoryId, string search)
        {
            ViewBag.DanhMucList = new SelectList(db.DanhMucs.ToList(), "ID", "TenDanhMuc");

            var sanPhams = db.SanPhams.Include(s => s.DanhMuc);

            // Lọc theo categoryId nếu có
            if (categoryId.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.ID_DanhMuc == categoryId.Value);
            }

            // Tìm kiếm theo từ khóa nếu có
            if (!string.IsNullOrEmpty(search))
            {
                sanPhams = sanPhams.Where(s => s.TenSanPham.Contains(search) || s.MoTa.Contains(search));
            }

            return View(sanPhams.ToList());
        }

        // GET: NdtSanPhams/NdtDetails/5
        public ActionResult NdtDetails(int id)
        {
            var sanPham = db.SanPhams.Include(s => s.DanhMuc).FirstOrDefault(s => s.ID == id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            return View(sanPham);
        }

        // GET: NdtSanPhams/NdtCreate
        public ActionResult NdtCreate()
        {
            ViewBag.ID_DanhMuc = new SelectList(db.DanhMucs, "ID", "TenDanhMuc");
            return View();
        }

        // POST: NdtSanPhams/NdtCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtCreate(SanPham sanPham, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                // Xử lý ảnh nếu có
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    // Tạo tên file duy nhất
                    string fileName = Path.GetFileNameWithoutExtension(HinhAnh.FileName);
                    string fileExtension = Path.GetExtension(HinhAnh.FileName);
                    string uniqueFileName = fileName + "_" + Guid.NewGuid().ToString() + fileExtension;

                    string folderPath = Server.MapPath("~/Images");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = Path.Combine(folderPath, uniqueFileName);
                    HinhAnh.SaveAs(filePath);

                    sanPham.HinhAnh = uniqueFileName;
                }

                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }

            ViewBag.ID_DanhMuc = new SelectList(db.DanhMucs, "ID", "TenDanhMuc", sanPham.ID_DanhMuc);
            return View(sanPham);
        }

        // GET: NdtSanPhams/NdtEdit/5
        public ActionResult NdtEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            ViewBag.ID_DanhMuc = new SelectList(db.DanhMucs, "ID", "TenDanhMuc", sanPham.ID_DanhMuc);
            return View(sanPham);
        }

        // POST: NdtSanPhams/NdtEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtEdit([Bind(Include = "ID,TenSanPham,MoTa,Gia,SoLuong,HinhAnh,ID_DanhMuc")] SanPham sanPham, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(HinhAnh.FileName);
                    string fileExtension = Path.GetExtension(HinhAnh.FileName);
                    string uniqueFileName = fileName + "_" + Guid.NewGuid().ToString() + fileExtension;

                    string folderPath = Server.MapPath("~/Images");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = Path.Combine(folderPath, uniqueFileName);
                    HinhAnh.SaveAs(filePath);

                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(sanPham.HinhAnh))
                    {
                        string oldFilePath = Path.Combine(folderPath, sanPham.HinhAnh);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    sanPham.HinhAnh = uniqueFileName;
                }

                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }

            ViewBag.ID_DanhMuc = new SelectList(db.DanhMucs, "ID", "TenDanhMuc", sanPham.ID_DanhMuc);
            return View(sanPham);
        }

        // GET: NdtSanPhams/NdtDelete/5
        public ActionResult NdtDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            return View(sanPham);
        }

        // POST: NdtSanPhams/NdtDelete/5
        [HttpPost, ActionName("NdtDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            if (sanPham != null)
            {
                string folderPath = Server.MapPath("~/Images");
                if (!string.IsNullOrEmpty(sanPham.HinhAnh))
                {
                    string filePath = Path.Combine(folderPath, sanPham.HinhAnh);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                db.SanPhams.Remove(sanPham);
                db.SaveChanges();
            }

            return RedirectToAction("NdtIndex");
        }

        // Thêm sản phẩm vào giỏ hàng
        public ActionResult NdtAddToCart(int id)
        {
            var gioHang = Session["GioHang"] as List<GioHang> ?? new List<GioHang>();

            var item = gioHang.FirstOrDefault(g => g.ID_SanPham == id);
            if (item == null)
            {
                var sanPham = db.SanPhams.Find(id);
                if (sanPham != null)
                {
                    gioHang.Add(new GioHang
                    {
                        ID_SanPham = sanPham.ID,
                        SanPham = sanPham,
                        SoLuong = 1
                    });
                }
            }
            else
            {
                item.SoLuong++;
            }

            Session["GioHang"] = gioHang;
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
