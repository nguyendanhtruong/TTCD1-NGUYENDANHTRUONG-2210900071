using System;
using System.Collections.Generic;
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
            // Giả sử bạn có người dùng đăng nhập và lấy đơn hàng của người đó
            int userId = 1; // Thay thế bằng ID người dùng thực tế
            var donHangs = db.DonHangs.Where(d => d.ID_NguoiDung == userId).ToList();

            return View(donHangs); // Trả về danh sách đơn hàng qua View
        }

        // GET: DonHangs/Details/5 - Hiển thị chi tiết đơn hàng
        public ActionResult Details(int id)
        {
            // Lấy đơn hàng theo ID
            var donHang = db.DonHangs.FirstOrDefault(d => d.ID == id);

            if (donHang == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy đơn hàng, trả về lỗi 404
            }

            // Lấy chi tiết đơn hàng từ bảng ChiTietDonHangs
            var chiTietDonHang = db.ChiTietDonHangs.Where(c => c.ID_DonHang == id).ToList();
            ViewBag.ChiTietDonHang = chiTietDonHang; // Truyền chi tiết đơn hàng vào ViewBag

            return View(donHang); // Trả về View hiển thị chi tiết đơn hàng
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang != null)
            {
                db.DonHangs.Remove(donHang);
                db.SaveChanges();
            }
            return RedirectToAction("Index"); // Hoặc quay lại trang danh sách đơn hàng
        }

    }
}
