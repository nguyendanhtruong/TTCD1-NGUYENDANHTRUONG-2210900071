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
    public class NdtDonHangsController : Controller
    {
        private Entities db = new Entities(); // Kết nối với database (Entities)

        // GET: NdtDonHangs/NdtIndex - Hiển thị danh sách đơn hàng của người dùng
        public ActionResult NdtIndex()
        {
            int userId = 1; // Thay thế bằng ID người dùng thực tế, có thể lấy từ session
            var donHangs = db.DonHangs.Where(d => d.ID_NguoiDung == userId).ToList();
            return View(donHangs);
        }

        // GET: NdtDonHangs/NdtDetails/5 - Hiển thị chi tiết đơn hàng
        public ActionResult NdtDetails(int id)
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

        // GET: NdtDonHangs/NdtCreate - Hiển thị form tạo đơn hàng
        public ActionResult NdtCreate()
        {
            return View();
        }

        // POST: NdtDonHangs/NdtCreate - Xử lý yêu cầu tạo đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtCreate(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                donHang.NgayTao = DateTime.Now; // Gán giá trị cho NgayTao
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }
            return View(donHang);
        }

        // GET: NdtDonHangs/NdtEdit/5 - Hiển thị form chỉnh sửa đơn hàng
        public ActionResult NdtEdit(int id)
        {
            var donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại.");
            }
            return View(donHang);
        }

        // POST: NdtDonHangs/NdtEdit/5 - Xử lý yêu cầu chỉnh sửa đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtEdit(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }
            return View(donHang);
        }

        // GET: NdtDonHangs/NdtDelete/5 - Hiển thị trang xác nhận xóa
        public ActionResult NdtDelete(int id)
        {
            var donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại.");
            }
            return View(donHang); // Trả về view xác nhận xóa
        }

        // POST: NdtDonHangs/DeleteConfirmed/5 - Xử lý yêu cầu xóa đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var chiTietDonHangs = db.ChiTietDonHangs.Where(c => c.ID_DonHang == id).ToList();
                foreach (var chiTiet in chiTietDonHangs)
                {
                    db.ChiTietDonHangs.Remove(chiTiet);
                }

                var donHang = db.DonHangs.Find(id);
                if (donHang != null)
                {
                    db.DonHangs.Remove(donHang);
                    db.SaveChanges();
                }
                else
                {
                    return HttpNotFound("Đơn hàng không tồn tại.");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi xóa đơn hàng: " + ex.Message;
                return View("Error");
            }

            return RedirectToAction("NdtIndex");
        }

        // GET: NdtDonHangs/UpdateStatus/5 - Cập nhật trạng thái đơn hàng
        public ActionResult UpdateStatus(int id)
        {
            var donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại.");
            }

            // Cập nhật trạng thái đơn hàng
            donHang.TrangThai = "Đã thanh toán"; // Ví dụ: Cập nhật trạng thái thành "Đã thanh toán"
            db.SaveChanges();

            return RedirectToAction("NdtDetails", new { id = donHang.ID });
        }

        // POST: NdtDonHangs/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, string TrangThai)
        {
            // Lấy đơn hàng theo ID
            var donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại.");
            }

            // Cập nhật trạng thái đơn hàng
            donHang.TrangThai = TrangThai;
            db.SaveChanges();

            // Chuyển hướng về trang chi tiết đơn hàng sau khi cập nhật
            return RedirectToAction("NdtDetails", new { id = donHang.ID });
        }
    }
}
