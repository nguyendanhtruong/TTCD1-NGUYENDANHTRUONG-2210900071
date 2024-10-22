using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTCD1_NGUYENDANHTRUONG_2210900071.Models;

namespace TTCD1_NGUYENDANHTRUONG_2210900071.Controllers
{
    public class NguoiDungsController : Controller
    {
        private Entities db = new Entities();

        // GET: NguoiDungs
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

        // GET: NguoiDungs/Create (Đăng ký)
        public ActionResult Create()
        {
            return View();
        }

        // POST: NguoiDungs/Create (Đăng ký)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenNguoiDung,Email,MatKhau,SoDienThoai,DiaChi")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem email đã tồn tại trong hệ thống hay chưa
                var existingUser = db.NguoiDungs.FirstOrDefault(u => u.Email == nguoiDung.Email);
                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "Email này đã được sử dụng!";
                    return View(nguoiDung);
                }

                // Mã hóa mật khẩu trước khi lưu
                nguoiDung.MatKhau = HashPassword(nguoiDung.MatKhau);
                nguoiDung.NgayTao = DateTime.Now;
                nguoiDung.VaiTro = "User"; // Mặc định vai trò là người dùng

                db.NguoiDungs.Add(nguoiDung);
                db.SaveChanges();

                // Sử dụng TempData để truyền thông báo thành công sau khi đăng ký
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";

                return RedirectToAction("Login", "NguoiDungs");
            }

            return View(nguoiDung);
        }

        // Hàm băm mật khẩu (Hash password)
        private string HashPassword(string password)
        {
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
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

        // GET: NguoiDungs/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: NguoiDungs/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Email, string MatKhau)
        {
            // Tìm người dùng dựa trên email
            var hashedPassword = HashPassword(MatKhau);
            var user = db.NguoiDungs.FirstOrDefault(u => u.Email == Email && u.MatKhau == hashedPassword);

            if (user != null)
            {
                // Đăng nhập thành công
                Session["UserID"] = user.ID;
                Session["UserName"] = user.TenNguoiDung;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Đăng nhập thất bại
                ViewBag.ErrorMessage = "Email hoặc mật khẩu không đúng.";
                return View();
            }
        }

        // Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
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
