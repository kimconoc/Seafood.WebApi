using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Domain.Common.Enum
{
    public enum ImageTypeEnum
    {
        [Description("Get list Image Product")]
        Product = 0,
        [Description("Get list Image ShopSeefood")]
        Shop = 1,
        [Description("Get list Image More")]
        More = 2,
    }
    public enum TypeAddressEnum
    {
        [Description("Địa chỉ shop Seafood")]
        Seafood = 0,
        [Description("Địa chỉ giao hàng người dùng")]
        User = 1,
    }
    public enum TypeAddressDetailEnum
    {
        [Description("Nhà riêng/Chung cư")]
        NhaRieng = 0,
        [Description("Cơ quan/Công ty")]
        CoQuan = 1,
    }
    public enum TypeVoucherEnum
    {
        [Description("Voucher Seafood")]
        Seafood = 0,
        [Description("Voucher User")]
        User = 1,
    }
    public enum StatusOrderEnum
    {
        [Description("Đang xử lý")]
        DangXuLy = 0,
        [Description("Đang vận chuyển")]
        DangVanChuyen = 1,
        [Description("Đơn đã giao")]
        DonDaGiao = 2,
        [Description("Đơn đã hủy")]
        DonDaHuy = -1,
    }

}
