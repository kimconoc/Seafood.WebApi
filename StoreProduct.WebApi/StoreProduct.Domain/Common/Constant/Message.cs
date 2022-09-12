using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProduct.Domain.Common.Constant
{
    public class Message
    {
        #region General
        public readonly static dynamic Un_Au_Thorized = new 
        { 
            ViMessage = "Bạn chưa đăng nhập", 
            EnMessage = "Please login" 
        };
        public readonly static dynamic Forbidden = new
        {
            ViMessage = "Bạn không có quyền truy cập",
            EnMessage = "You do not have permission to access",
            ActionCode = ""
        };
        public readonly static dynamic Successful = new
        {
            ViMessage = "Thành công",
            EnMessage = "Successful"
        };
        public readonly static dynamic Bad_Request = new
        {
            ViMessage = "Có lỗi trong quá trình xử lý",
            EnMessage = "There was an error in processing"
        };
        public readonly static dynamic DATA_NOT_FOUND = new 
        { 
            ViMessage = "Dữ liệu không tồn tại", 
            EnMessage = "Internal server error" 
        };
        public readonly static dynamic INTERAL_SERVER_ERROR = new 
        { 
            ViMessage = "Có lỗi xảy ra", 
            EnMessage = "Internal server error" 
        };
        #endregion General

        #region Account
        public readonly static dynamic LOGIN_ERROR = new
        {
            ViMessage = "Thông tin đăng nhập chưa đúng",
            EnMessage = "Login information is incorrect"
        };
        public readonly static dynamic Account_LOCKED = new
        {
            ViMessage = "Tài khoản đã bị khóa",
            EnMessage = "Account locked"
        };
        #endregion Account
    }
}
