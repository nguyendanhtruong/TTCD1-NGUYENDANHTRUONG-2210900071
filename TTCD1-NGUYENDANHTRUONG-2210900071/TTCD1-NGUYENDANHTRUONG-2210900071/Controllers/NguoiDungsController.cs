using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TTCD1_NGUYENDANHTRUONG_2210900071.Models;

namespace TTCD1_NGUYENDANHTRUONG_2210900071.Controllers
{
    public class NguoiDungsController : Controller
    {
        private Entities db = new Entities();

        // GET: NguoiDungs/Index
        public ActionResult Index()
        {
            return View(db.NguoiDungs.ToList());
        }

        // GET: NguoiDungs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }

            return View(nguoiDung);
        }

        // GET: NguoiDungs/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ID,TenNguoiDung,Email,MatKhau,SoDienThoai,DiaChi")] NguoiDung nguoiDung)
        {
            if (!ModelState.IsValid)
            {
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
                return RedirectToAction("Login");
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

        // GET: NguoiDungs/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: NguoiDungs/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string matKhau)
        {
            var hashedPassword = HashPassword(matKhau);

            try
            {
                // Tìm người dùng với email và mật khẩu phù hợp
                var user = db.NguoiDungs.FirstOrDefault(u => u.Email == email && u.MatKhau == hashedPassword);
                if (user != null)
                {
                    SetUserSession(user);

                    // Phân quyền dựa trên vai trò của người dùng
                    if (user.VaiTro == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");  // Chuyển hướng đến trang Admin
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");  // Chuyển hướng đến trang người dùng thường
                    }
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
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: NguoiDungs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }

            return View(nguoiDung);
        }

        // POST: NguoiDungs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenNguoiDung,Email,MatKhau,SoDienThoai,DiaChi,VaiTro,NgayTao")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(nguoiDung.MatKhau))
                {
                    nguoiDung.MatKhau = HashPassword(nguoiDung.MatKhau);
                }

                db.Entry(nguoiDung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nguoiDung);
        }

        // GET: NguoiDungs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }

            return View(nguoiDung);
        }

        // POST: NguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            db.NguoiDungs.Remove(nguoiDung);
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
