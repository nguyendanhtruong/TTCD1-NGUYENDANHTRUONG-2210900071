namespace TTCD1_NGUYENDANHTRUONG_2210900071.Models
{
    using System;
    using System.Collections.Generic;

    public partial class NguoiDung
    {
        public int ID { get; set; }
        public string TenNguoiDung { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string VaiTro { get; set; } // Vai trò: "Admin" hoặc "User"
        public Nullable<System.DateTime> NgayTao { get; set; }

        public virtual ICollection<DonHang> DonHangs { get; set; }
        public virtual ICollection<GioHang> GioHangs { get; set; }

        public NguoiDung()
        {
            this.DonHangs = new HashSet<DonHang>();
            this.GioHangs = new HashSet<GioHang>();
        }
    }
}
