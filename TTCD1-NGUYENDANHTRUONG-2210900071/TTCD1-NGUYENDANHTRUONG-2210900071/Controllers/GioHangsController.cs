using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TTCD1_NGUYENDANHTRUONG_2210900071.Models;

namespace TTCD1_NGUYENDANHTRUONG_2210900071.Controllers
{
    public class GioHangsController : Controller
    {
        private Entities db = new Entities();

        // GET: GioHangs
        public ActionResult Index()
        {
            // Lấy giỏ hàng từ session
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs == null)
            {
                gioHangs = new List<GioHang>();
            }

            return View(gioHangs);
        }

        // Thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(int id)
        {
            // Lấy giỏ hàng từ session
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs == null)
            {
                gioHangs = new List<GioHang>();
            }

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            GioHang item = gioHangs.FirstOrDefault(g => g.ID_SanPham == id);
            if (item == null)
            {
                // Thêm sản phẩm mới
                SanPham sanPham = db.SanPhams.Find(id);
                if (sanPham != null)
                {
                    gioHangs.Add(new GioHang
                    {
                        ID_SanPham = sanPham.ID,
                        SanPham = sanPham,
                        SoLuong = 1
                    });
                }
            }
            else
            {
                // Tăng số lượng sản phẩm đã có trong giỏ
                item.SoLuong++;
            }

            // Cập nhật session
            Session["GioHang"] = gioHangs;

            return RedirectToAction("Index", "SanPhams");
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        public ActionResult UpdateCart(int id, int quantity)
        {
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            GioHang item = gioHangs.FirstOrDefault(g => g.ID_SanPham == id);
            if (item != null)
            {
                item.SoLuong = quantity;
            }

            // Cập nhật lại session
            Session["GioHang"] = gioHangs;
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        // Thanh toán giỏ hàng
        public ActionResult Checkout()
        {
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs == null || !gioHangs.Any())
            {
                ViewBag.Message = "Giỏ hàng của bạn hiện tại trống!";
                return RedirectToAction("Index", "SanPhams");
            }

            // Tính tổng tiền từ giỏ hàng
            decimal tongTien = gioHangs.Sum(item => item.SanPham.Gia * item.SoLuong);

            // Tạo đơn hàng mới
            DonHang donHang = new DonHang
            {
                NgayDat = DateTime.Now,
                TongTien = tongTien,
                TrangThai = "Chờ xử lý",
                ID_NguoiDung = 1 // Giả sử người dùng có ID là 1
            };

            // Thêm đơn hàng vào cơ sở dữ liệu
            db.DonHangs.Add(donHang);
            db.SaveChanges();

            // Lưu chi tiết đơn hàng vào cơ sở dữ liệu
            foreach (var item in gioHangs)
            {
                ChiTietDonHang chiTiet = new ChiTietDonHang
                {
                    ID_DonHang = donHang.ID,
                    ID_SanPham = item.ID_SanPham,
                    SoLuong = item.SoLuong,
                    Gia = item.SanPham.Gia
                };
                db.ChiTietDonHangs.Add(chiTiet);
            }
            db.SaveChanges();

            // Xóa giỏ hàng sau khi thanh toán
            Session["GioHang"] = null;

            // Chuyển hướng đến trang xác nhận thanh toán
            return RedirectToAction("CompleteCheckout", new { id = donHang.ID });
        }

        // Xác nhận thanh toán thành công
        [HttpPost]
        public ActionResult ConfirmOrder(int id)
        {
            // Tìm đơn hàng theo ID
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang != null)
            {
                // Cập nhật trạng thái đơn hàng thành "Đã hoàn thành"
                donHang.TrangThai = "Đã hoàn thành";
                db.SaveChanges();
            }

            // Hiển thị thông tin đơn hàng sau khi xác nhận
            return RedirectToAction("OrderDetails", new { id = id });
        }

        public ActionResult CompleteCheckout(int id)
        {
            // Lấy đơn hàng từ cơ sở dữ liệu dựa trên ID
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound("Không tìm thấy thông tin đơn hàng!");
            }

            // Truyền thông tin đơn hàng sang view
            return View(donHang);
        }

    }
}
