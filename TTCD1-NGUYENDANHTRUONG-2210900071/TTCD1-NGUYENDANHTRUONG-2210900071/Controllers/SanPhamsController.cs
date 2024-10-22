using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTCD1_NGUYENDANHTRUONG_2210900071.Models;

namespace TTCD1_NGUYENDANHTRUONG_2210900071.Controllers
{
    public class SanPhamsController : Controller
    {
        private Entities db = new Entities();

        // GET: SanPhams
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.Include(s => s.DanhMuc);
            return View(sanPhams.ToList());
        }
        public ActionResult Delete(int id)
        {
            DonHang donHang = db.DonHangs.Include(d => d.NguoiDung) // Bao gồm NguoiDung để hiển thị tên người dùng
                                          .FirstOrDefault(d => d.ID == id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }


        // Thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(int id)
        {
            // Kiểm tra nếu giỏ hàng đã tồn tại trong session, nếu không thì tạo mới
            List<GioHang> gioHang = Session["GioHang"] as List<GioHang>;
            if (gioHang == null)
            {
                gioHang = new List<GioHang>();
            }

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            GioHang item = gioHang.FirstOrDefault(g => g.ID_SanPham == id);
            if (item == null)
            {
                // Nếu chưa có thì thêm sản phẩm mới vào giỏ hàng
                SanPham sanPham = db.SanPhams.Find(id);
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
                // Nếu có rồi thì tăng số lượng
                item.SoLuong++;
            }

            // Lưu giỏ hàng vào session
            Session["GioHang"] = gioHang;

            // Quay lại trang sản phẩm sau khi thêm vào giỏ hàng
            return RedirectToAction("Index", "SanPhams");
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.ID_DanhMuc = new SelectList(db.DanhMucs, "ID", "TenDanhMuc");
            return View();
        }

        // POST: SanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sanPham, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu có file hình ảnh được upload
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    // Lấy tên file
                    string fileName = Path.GetFileName(HinhAnh.FileName);
                    string folderPath = Server.MapPath("~/Images");

                    // Kiểm tra thư mục Images có tồn tại không, nếu không thì tạo mới
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Lưu file vào thư mục 'Images'
                    string filePath = Path.Combine(folderPath, fileName);
                    HinhAnh.SaveAs(filePath);

                    // Cập nhật thuộc tính HinhAnh của sản phẩm
                    sanPham.HinhAnh = fileName;
                }

                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_DanhMuc = new SelectList(db.DanhMucs, "ID", "TenDanhMuc", sanPham.ID_DanhMuc);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_DanhMuc = new SelectList(db.DanhMucs, "ID", "TenDanhMuc", sanPham.ID_DanhMuc);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenSanPham,MoTa,Gia,SoLuong,HinhAnh,ID_DanhMuc")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_DanhMuc = new SelectList(db.DanhMucs, "ID", "TenDanhMuc", sanPham.ID_DanhMuc);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
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
