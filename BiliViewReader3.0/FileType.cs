using System;
using System.IO;

namespace BiliViewReader3
{
    class FileType
    {
        /// <summary>
        /// 判断文件是否为图片（jpg或png）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsImage(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    try
                    {
                        var buffer = reader.ReadByte();
                        var fileClass = buffer.ToString();
                        buffer = reader.ReadByte();
                        fileClass += buffer.ToString();
                        var fileEnum = int.Parse(fileClass);
                        if (fileEnum is (int)FileExtension.JPG
                            || fileEnum is (int)FileExtension.PNG)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }

        }
    }
    /// <summary>
    /// 文件类型的枚举
    /// </summary>
    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        BMP = 6677,
        PNG = 13780,
        COM = 7790,
        EXE = 7790,
        DLL = 7790,
        RAR = 8297,
        ZIP = 8075,
        XML = 6063,
        HTML = 6033,
        ASPX = 239187,
        CS = 117115,
        JS = 119105,
        TXT = 210187,
        SQL = 255254,
        BAT = 64101,
        BTSEED = 10056,
        RDP = 255254,
        PSD = 5666,
        PDF = 3780,
        CHM = 7384,
        LOG = 70105,
        REG = 8269,
        HLP = 6395,
        DOC = 208207,
        XLS = 208207,
        DOCX = 208207,
        XLSX = 208207
    }
}
