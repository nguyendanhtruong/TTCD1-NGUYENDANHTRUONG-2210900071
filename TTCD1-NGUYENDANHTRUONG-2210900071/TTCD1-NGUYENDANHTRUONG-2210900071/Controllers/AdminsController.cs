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
    public class AdminsController : Controller
    {
        private Entities db = new Entities(); // Khởi tạo DbContext

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
            Admin admin = db.Admins.Find(id); // Tìm admin theo ID
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin); // Trả về view chi tiết admin
        }

        // GET: Admins/Create
        public ActionResult NdtCreate()
        {
            return View(); // Trả về view tạo admin mới
        }

        // POST: Admins/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtCreate([Bind(Include = "ID,TenAdmin,Email,MatKhau,SoDienThoai,NgayTao")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin); // Thêm admin mới vào DbSet
                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return RedirectToAction(" NdtIndex"); // Chuyển hướng về trang danh sách admin
            }

            return View(admin); // Trả về view nếu có lỗi
        }

        // GET: Admins/Edit/5
        public ActionResult NdtEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id); // Tìm admin theo ID
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin); // Trả về view chỉnh sửa admin
        }

        // POST: Admins/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NdtEdit([Bind(Include = "ID,TenAdmin,Email,MatKhau,SoDienThoai,NgayTao")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified; // Đánh dấu admin là đã được sửa đổi
                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return RedirectToAction(" NdtIndex"); // Chuyển hướng về trang danh sách admin
            }
            return View(admin); // Trả về view nếu có lỗi
        }

        // GET: Admins/Delete/5
        public ActionResult NdtDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id); // Tìm admin theo ID
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin); // Trả về view xác nhận xóa admin
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName(" NdtDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id); // Tìm admin theo ID
            db.Admins.Remove(admin); // Xóa admin khỏi DbSet
            db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            return RedirectToAction(" NdtIndex"); // Chuyển hướng về trang danh sách admin
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(); // Giải phóng tài nguyên của DbContext
            }
            base.Dispose(disposing); // Gọi phương thức Dispose của lớp cơ sở
        }
    }
}
