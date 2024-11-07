﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace K22CNT4_NGUYENDANHTRUONG_2210900071.Models
{
    using System;
    using System.Collections.Generic;

    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            this.ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            this.GioHangs = new HashSet<GioHang>();
        }

        // ID sản phẩm
        public int ID { get; set; }

        // Tên sản phẩm
        public string TenSanPham { get; set; }

        // Mô tả chi tiết về sản phẩm
        public string MoTa { get; set; }

        // Giá của sản phẩm
        public decimal Gia { get; set; }

        // Số lượng sản phẩm hiện có trong kho
        public int SoLuong { get; set; }

        // Đường dẫn đến hình ảnh của sản phẩm
        public string HinhAnh { get; set; }

        // ID danh mục của sản phẩm (nếu có)
        public Nullable<int> ID_DanhMuc { get; set; }

        // Chi tiết các đơn hàng có sản phẩm này
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        // Danh mục chứa sản phẩm này
        public virtual DanhMuc DanhMuc { get; set; }

        // Giỏ hàng có sản phẩm này
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHangs { get; set; }
    }
}
