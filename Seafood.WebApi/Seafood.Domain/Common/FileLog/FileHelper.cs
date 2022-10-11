using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Domain.Common.FileLog
{
    public class FileHelper
    {
        public static bool isStopped = false;
        public const string Format = "{0}{1}{2}----------------------------{3}";
        public const string Log = "Log";
        public const string Error = "Error";
        public const string Utf8 = "utf-8";
        public const string FormatContent = "{0}:{1} - {2}";
        public const string HHmmss = "HH-mm-ss";
        public const string HHmm = "HH-mm";
        public const string ddMMyyyy = "dd-MM-yyyy";
        public const string FormatFilePath = "{0}{1}.txt";
        public const string FormatPath = @"{0}\{1}\";
        private const string SystemFolder = "System";
        private static object objWrite = true;
        public static bool isWriteLogfile = false;
        public static int countThread = 0;
        public static int Version = 0;
        public static bool WriteFile(string pathWrite, string content)
        {
            var result = false;
            lock (objWrite)
            {
                // Tạo thư mục nếu như thư mục chưa tồn tại
                var folder = Path.GetDirectoryName(pathWrite);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                // Tạo file nếu như file chưa tồn tại
                if (!File.Exists(pathWrite))
                    using (var fs = new FileStream(pathWrite, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)) fs.Close();
                // Thực hiện ghi file
                using (var sw = new StreamWriter(pathWrite, true, Encoding.GetEncoding(Utf8)))
                {
                    sw.WriteLine(content);
                    result = true;
                }
            }
            return result;
        }
        public static void GeneratorFileByDay(string content, string folderName = "")
        {
            if (string.IsNullOrEmpty(folderName))
                folderName = SystemFolder;
            DateTime time = DateTime.Now;
            var fullPath = AppDomain.CurrentDomain.BaseDirectory + string.Format(@"{0}\{1}\{2}\{3}\{4}", "LogError", folderName, time.Year, time.Month, time.Day + ".txt");
            content = $"[{time.ToString("HH:mm:ss")}] " + content;
            WriteFile(fullPath, content);
        }
    }
}
