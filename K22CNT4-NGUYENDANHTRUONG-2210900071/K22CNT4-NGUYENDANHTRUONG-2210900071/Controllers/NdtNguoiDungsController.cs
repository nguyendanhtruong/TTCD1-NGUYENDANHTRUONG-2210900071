using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using K22CNT4_NGUYENDANHTRUONG_2210900071.Models;

namespace K22CNT4_NGUYENDANHTRUONG_2210900071.Controllers
{
    public class NdtNguoiDungsController : Controller
    {
        private Entities db = new Entities();

        // GET: NguoiDungs/NdtIndex
        public ActionResult NdtIndex()
        {
            return View(db.NguoiDungs.ToList());
        }

        // GET: NguoiDungs/NdtDetails/5
        public ActionResult NdtDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }

            return View(nguoiDung);
        }

        // GET: NguoiDungs/NdtRegister
        public ActionResult NdtRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtRegister([Bind(Include = "ID,TenNguoiDung,Email,MatKhau,SoDienThoai,DiaChi")] NguoiDung nguoiDung)
        {
            if (!ModelState.IsValid)
            {
                return View(nguoiDung);
            }

            // Kiểm tra email đã tồn tại
            if (db.NguoiDungs.Any(u => u.Email == nguoiDung.Email))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                return View(nguoiDung);
            }

            try
            {
                nguoiDung.MatKhau = HashPassword(nguoiDung.MatKhau);
                nguoiDung.NgayTao = DateTime.Now;
                nguoiDung.VaiTro = "User";  // Mặc định đăng ký là người dùng thường

                db.NguoiDungs.Add(nguoiDung);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("NdtLogin");
            }
            catch (DbUpdateException dbEx)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra khi đăng ký: " + (dbEx.InnerException?.Message ?? dbEx.Message);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra khi đăng ký: " + ex.Message;
            }

            return View(nguoiDung);
        }

        // GET: NguoiDungs/NdtLogin
        public ActionResult NdtLogin()
        {
            return View();
        }

        // POST: NguoiDungs/NdtLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtLogin(string email, string matKhau)
        {
            var hashedPassword = HashPassword(matKhau);

            try
            {
                var user = db.NguoiDungs.FirstOrDefault(u => u.Email == email && u.MatKhau == hashedPassword);
                if (user != null)
                {
                    SetUserSession(user);

                    return RedirectToAction(user.VaiTro == "Admin" ? "Dashboard" : "NdtIndex", user.VaiTro == "Admin" ? "Admin" : "NdtHome");
                }

                ViewBag.ErrorMessage = "Email hoặc mật khẩu không đúng.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra khi đăng nhập: " + ex.Message;
            }

            return View();
        }

        // Đăng xuất người dùng
        public ActionResult NdtLogout()
        {
            Session.Clear();
            return RedirectToAction("NdtLogin");
        }

        // GET: NguoiDungs/NdtEdit/5
        public ActionResult NdtEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }

            return View(nguoiDung);
        }

        // POST: NguoiDungs/NdtEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtEdit([Bind(Include = "ID,TenNguoiDung,Email,MatKhau,SoDienThoai,DiaChi,VaiTro,NgayTao")] NguoiDung nguoiDung)
        {
            if (!ModelState.IsValid) return View(nguoiDung);

            try
            {
                if (!string.IsNullOrEmpty(nguoiDung.MatKhau))
                {
                    nguoiDung.MatKhau = HashPassword(nguoiDung.MatKhau);
                }

                db.Entry(nguoiDung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra khi cập nhật: " + ex.Message;
            }

            return View(nguoiDung);
        }

        // GET: NguoiDungs/NdtDelete/5
        public ActionResult NdtDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }

            return View(nguoiDung);
        }

        // POST: NguoiDungs/NdtDelete/5
        [HttpPost, ActionName("NdtDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var nguoiDung = db.NguoiDungs.Find(id);
                db.NguoiDungs.Remove(nguoiDung);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra khi xóa: " + ex.Message;
                return RedirectToAction("NdtDelete", new { id });
            }

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

        // Hàm băm mật khẩu (Hash password)
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Mật khẩu không được để trống.");

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        // Thiết lập phiên người dùng
        private void SetUserSession(NguoiDung user)
        {
            Session["UserID"] = user.ID;
            Session["UserName"] = user.TenNguoiDung;
            Session["UserRole"] = user.VaiTro;
        }
    }
}
