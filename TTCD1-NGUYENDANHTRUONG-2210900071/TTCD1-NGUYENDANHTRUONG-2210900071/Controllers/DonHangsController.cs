using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCD1_NGUYENDANHTRUONG_2210900071.Models;

namespace TTCD1_NGUYENDANHTRUONG_2210900071.Controllers
{
    public class DonHangsController : Controller
    {
        private Entities db = new Entities(); // Kết nối với database (Entities)

        // GET: DonHangs - Hiển thị danh sách đơn hàng của người dùng
        public ActionResult Index()
        {
            int userId = 1; // Thay thế bằng ID người dùng thực tế
            var donHangs = db.DonHangs.Where(d => d.ID_NguoiDung == userId).ToList();
            return View(donHangs);
        }

        // GET: DonHangs/Details/5 - Hiển thị chi tiết đơn hàng
        public ActionResult Details(int id)
        {
            var donHang = db.DonHangs.FirstOrDefault(d => d.ID == id);
            if (donHang == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại.");
            }

            var chiTietDonHang = db.ChiTietDonHangs.Where(c => c.ID_DonHang == id).ToList();
            ViewBag.ChiTietDonHang = chiTietDonHang;
            return View(donHang);
        }

        // GET: DonHangs/Create - Hiển thị form tạo đơn hàng
        public ActionResult Create()
        {
            return View();
        }

        // POST: DonHangs/Create - Xử lý yêu cầu tạo đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                donHang.NgayTao = DateTime.Now; // Gán giá trị cho NgayTao
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donHang);
        }

        // GET: DonHangs/Edit/5 - Hiển thị form chỉnh sửa đơn hàng
        public ActionResult Edit(int id)
        {
            var donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại.");
            }
            return View(donHang);
        }

        // POST: DonHangs/Edit/5 - Xử lý yêu cầu chỉnh sửa đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donHang);
        }

        // GET: DonHangs/Delete/5 - Hiển thị trang xác nhận xóa

        // POST: DonHangs/DeleteConfirmed/5 - Xóa đơn hàng sau khi xác nhận
        // POST: DonHangs/DeleteConfirmed/5 - Xóa đơn hàng sau khi xác nhận
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete (int id)
        {
            try
            {
                // Xóa các chi tiết đơn hàng liên quan
                var chiTietDonHangs = db.ChiTietDonHangs.Where(c => c.ID_DonHang == id).ToList();
                foreach (var chiTiet in chiTietDonHangs)
                {
                    db.ChiTietDonHangs.Remove(chiTiet);
                }

                // Xóa đơn hàng
                var donHang = db.DonHangs.Find(id);
                if (donHang != null)
                {
                    db.DonHangs.Remove(donHang);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.InnerException?.Message;
                Console.WriteLine("Chi tiết lỗi: " + innerException);
                return View("Error", new HandleErrorInfo(ex, "DonHangs", "DeleteConfirmed"));
            }

            return RedirectToAction("Index");
        }
    }
}

        // POST: DonHangs/DeleteConfirmed/5 - Xóa đơn hàng sau khi xác nhận

    

