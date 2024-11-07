namespace K22CNT4_NGUYENDANHTRUONG_2210900071.Models
{
    using System;
    using System.Collections.Generic;

    public partial class GioHang
    {
        // ID của giỏ hàng
        public int ID { get; set; }

        // ID người dùng (nếu người dùng chưa đăng nhập, giá trị có thể null)
        public Nullable<int> ID_NguoiDung { get; set; }

        // ID sản phẩm được thêm vào giỏ
        public int ID_SanPham { get; set; }

        // Số lượng sản phẩm trong giỏ
        public int SoLuong { get; set; }

        // Ngày sản phẩm được thêm vào giỏ
        public Nullable<System.DateTime> NgayThem { get; set; }

        // Thông tin về người dùng (quan hệ với bảng NguoiDung)
        public virtual NguoiDung NguoiDung { get; set; }

        // Thông tin về sản phẩm (quan hệ với bảng SanPham)
        public virtual SanPham SanPham { get; set; }

        // Tính tổng giá trị của sản phẩm trong giỏ hàng
        public decimal TongTien
        {
            get
            {
                return (SanPham != null ? SanPham.Gia : 0) * SoLuong;
            }
        }
    }
}
