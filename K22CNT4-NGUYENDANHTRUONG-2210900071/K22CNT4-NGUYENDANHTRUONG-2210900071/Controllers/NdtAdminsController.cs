using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using K22CNT4_NGUYENDANHTRUONG_2210900071.Models;
using K22CNT4_NGUYENDANHTRUONG_2210900071.Filters; // Thêm namespace cho bộ lọc

namespace K22CNT4_NGUYENDANHTRUONG_2210900071.Controllers
{
    [AuthorizeAdmin] // Đảm bảo chỉ Admin đã đăng nhập mới truy cập được các action
    public class NdtAdminsController : Controller
    {
        private Entities db = new Entities(); // Khởi tạo DbContext

        // GET: Admins/Login
        [AllowAnonymous] // Cho phép truy cập không cần đăng nhập
        public ActionResult NdtLogin()
        {
            return View(); // Trả về view đăng nhập
        }

        // POST: Admins/Login
        [HttpPost]
        [AllowAnonymous] // Cho phép truy cập không cần đăng nhập
        [ValidateAntiForgeryToken]
        public ActionResult NdtLogin(string email, string matKhau)
        {
            var admin = db.Admins.FirstOrDefault(a => a.Email == email && a.MatKhau == matKhau);
            if (admin != null)
            {
                Session["AdminID"] = admin.ID; // Lưu ID admin vào session
                Session["AdminName"] = admin.TenAdmin; // Lưu tên admin vào session
                return RedirectToAction("NdtIndex"); // Đăng nhập thành công, chuyển hướng đến trang quản lý admin
            }
            ModelState.AddModelError("", "Thông tin đăng nhập không đúng.");
            return View();
        }

        // GET: Admins/Logout
        public ActionResult NdtLogout()
        {
            Session.Clear(); // Xóa tất cả dữ liệu session
            return RedirectToAction("NdtLogin"); // Chuyển hướng về trang đăng nhập
        }

        // GET: Admins
        public ActionResult NdtIndex()
        {
            var admins = db.Admins.ToList(); // Lấy danh sách admin
            var sanphams = db.SanPhams.ToList(); // Lấy danh sách sản phẩm
            var model = new Tuple<IEnumerable<Admin>, IEnumerable<SanPham>>(admins, sanphams); // Tạo Tuple chứa danh sách admin và sản phẩm
            return View(model); // Trả về view với model
        }

        // GET: Admins/Details/5
        public ActionResult NdtDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound("Admin not found with the specified ID.");
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult NdtCreate()
        {
            return View();
        }

        // POST: Admins/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtCreate([Bind(Include = "ID,TenAdmin,Email,MatKhau,SoDienThoai,NgayTao")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }
            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult NdtEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound("Admin not found with the specified ID.");
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtEdit([Bind(Include = "ID,TenAdmin,Email,MatKhau,SoDienThoai,NgayTao")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NdtIndex");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult NdtDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound("Admin not found with the specified ID.");
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("NdtDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            if (admin != null)
            {
                db.Admins.Remove(admin);
                db.SaveChanges();
            }
            return RedirectToAction("NdtIndex");
        }

        // Dispose the context properly
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
