using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seefood.Domain.Common.Constant
{
    public class Constant
    {
        #region ConstAuthentication
        public const string AuthenticationType = "ApplicationCookie";
        #endregion ConstAuthentication

        public const string CodeTom = "SEATOM";
        public const string CodeCua = "SEACUA";
        public const string CodeCa = "SEACA";
        public const string CodeMuc = "SEAMUC";
        public const string CodeGhe = "SEAGHE";
        public const string CodeBeBe = "SEABEBE";
        public const string CodeHau = "SEAHAU";
        public const string CodeNgao = "SEANGAO";
        public const string CodeDoKho = "SEADOKHO";
        public const string CodeNuocMam = "SEANUOCMAM";

        #region Khu vực Hà Nội
        //Ông
        public const string Region_HaNoi = "SEAHANOI";
        //Cha
        public const string RegionDistrict_ThanhXuan = "SEAHANOITHANHXUAN";
        public const string RegionDistrict_ThanhTri = "SEAHANOITHANHTRI";
        //Con
        public readonly static string[] ShopSeafood_HaNoi = { "SEAFOODNGOCHOI"};
        #endregion Khu vực Hà Nội
    }
}
