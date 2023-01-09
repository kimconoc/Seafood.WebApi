using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amin.Controllers
{
    public class UploadFileController : Controller
    {
        // GET: UploadFile
        public ActionResult UpLoadFileExcel()
        {
            return View();
        }
        public ActionResult ExecuteReadFileExcel(HttpPostedFileBase executeReadFileExcel)
        {
            dynamic noti = null;
            List<string> data = new List<string>();
            DataTable dataTabData = new DataTable();
            if (executeReadFileExcel != null)
            {
                if (executeReadFileExcel.ContentType == "application/vnd.ms-excel" || executeReadFileExcel.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    try
                    {
                        Stream stream = executeReadFileExcel.InputStream;
                        IExcelDataReader reader = null;
                        if (executeReadFileExcel.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (executeReadFileExcel.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });
                        //Shee1
                        dataTabData = result.Tables[0];
                        if (dataTabData != null)
                        {
                            for (int row = 1; row < dataTabData.Rows.Count; row++)
                            {
                                int col = 0;
                                var valueCol1 = dataTabData.Rows[row][col].ToString();
                                var valueCol2 = dataTabData.Rows[row][col + 1].ToString();
                                var valueCol3 = dataTabData.Rows[row][col + 2].ToString();
                                var valueCol4 = dataTabData.Rows[row][col + 3].ToString();
                                var valueCol5 = dataTabData.Rows[row][col + 4].ToString();
                            }
                        }

                        reader.Close();
                        return Json(true);
                    }
                    catch
                    {
                        return Json(false);
                    }

                }
                else
                {
                    noti = new
                    {
                        Message = "Thất bại"
                    };
                    return Json(noti);
                }
            }
            else
            {
                noti = new
                {
                    Message = "Bạn chưa upload file"
                };
                return Json(noti);
            }
        }
        public ActionResult ExecuteSaveFile(HttpPostedFileBase executeSaveFile)
        {
            dynamic noti = null;
            List<string> data = new List<string>();
            DataTable dataTabData = new DataTable();
            if (executeSaveFile != null)
            {
                string filePath = string.Empty;
                filePath = Server.MapPath("~/FileUpload/AvartaUser/");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + Path.GetFileName(executeSaveFile.FileName);
                executeSaveFile.SaveAs(filePath);
            }
            else
            {
                noti = new
                {
                    Message = "Bạn chưa upload file"
                };
                return Json(noti);
            }
            noti = new
            {
                Message = "Bạn đã lưu file thành công"
            };
            return Json(noti);
        }
    }
}