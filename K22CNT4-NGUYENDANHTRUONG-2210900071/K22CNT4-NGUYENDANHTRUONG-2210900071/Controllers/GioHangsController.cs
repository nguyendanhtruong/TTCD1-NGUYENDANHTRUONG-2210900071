using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using K22CNT4_NGUYENDANHTRUONG_2210900071.Models;

namespace K22CNT4_NGUYENDANHTRUONG_2210900071.Controllers
{
    public class GioHangsController : Controller
    {
        private Entities db = new Entities();

        // GET: GioHangs
        public ActionResult NdtIndex()
        {
            // Lấy giỏ hàng từ session
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs == null)
            {
                gioHangs = new List<GioHang>();
            }

            // Tính tổng tiền của giỏ hàng
            decimal tongTien = gioHangs.Sum(item => item.SanPham.Gia * item.SoLuong);

            // Truyền giỏ hàng và tổng tiền vào View
            ViewBag.TongTien = tongTien;
            return View(gioHangs);
        }

        // Thêm sản phẩm vào giỏ hàng
        public ActionResult NdtAddToCart(int id)
        {
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs == null)
            {
                gioHangs = new List<GioHang>();
            }

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            GioHang item = gioHangs.FirstOrDefault(g => g.ID_SanPham == id);
            if (item == null)
            {
                // Thêm sản phẩm mới vào giỏ hàng
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
                // Tăng số lượng sản phẩm trong giỏ hàng
                item.SoLuong++;
            }

            // Cập nhật giỏ hàng vào session
            Session["GioHang"] = gioHangs;

            // Quay lại trang sản phẩm
            return RedirectToAction("NdtIndex", "SanPhams");
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

            // Cập nhật giỏ hàng vào session
            Session["GioHang"] = gioHangs;

            // Quay lại trang giỏ hàng
            return RedirectToAction("NdtIndex");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int id)
        {
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            GioHang item = gioHangs.FirstOrDefault(g => g.ID_SanPham == id);
            if (item != null)
            {
                gioHangs.Remove(item);
            }

            // Cập nhật lại giỏ hàng trong session
            Session["GioHang"] = gioHangs;

            // Quay lại trang giỏ hàng
            return RedirectToAction("NdtIndex");
        }

        // Thanh toán giỏ hàng
        public ActionResult NdtCheckout()
        {
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs == null || !gioHangs.Any())
            {
                ViewBag.Message = "Giỏ hàng của bạn hiện tại trống!";
                return RedirectToAction("NdtIndex", "SanPhams");
            }

            // Tính tổng tiền từ giỏ hàng
            decimal tongTien = gioHangs.Sum(item => item.SanPham.Gia * item.SoLuong);

            // Tạo đơn hàng mới
            DonHang donHang = new DonHang
            {
                NgayDat = DateTime.Now,
                TongTien = tongTien,
                TrangThai = "Đang xử lý",  // Trạng thái khi đang chờ thanh toán
                ID_NguoiDung = 1 // Giả sử người dùng có ID là 1 (có thể lấy từ session)
            };

            try
            {
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
                return RedirectToAction("NdtCompleteCheckout", new { id = donHang.ID });
            }
            catch (Exception)
            {
                // Xử lý khi có lỗi xảy ra
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình thanh toán!";
                return RedirectToAction("NdtIndex");
            }
        }

        // Trang xác nhận thanh toán thành công
        public ActionResult NdtCompleteCheckout(int id)
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
