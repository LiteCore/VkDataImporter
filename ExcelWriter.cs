using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Utils;
using OfficeOpenXml;

namespace VKDataImporter
{
    static class ExcelWriter
    {
        public static void CreatePackage(string path)
        {

        }
        public static void WriteUserInfo(string path, List<User> users)
        {
            using (var p = new ExcelPackage(new FileInfo(path)))
            {
                var ws = p.Workbook.Worksheets.Add("Users");
                var settings = Properties.Settings.Default;
                List<string> Fields = new List<string>()
                    {
                        "ID",
                        "Имя",
                        "Фамилия",
                        "Пол",
                        settings.DayOfBirth ? "Дата рождения" : null,
                        settings.City ? "Город" : null,
                        settings.PrivateMessages ? "ЛС" : null
                    }.Where(x => x != null).ToList();
                for (int field = 0; field < Fields.Count; field++)
                {
                    ws.Cells[1, field + 1].Value = Fields[field];
                }
                for (int row = 0; row < users.Count; row++)
                {
                    int cell = 0;
                    ws.Cells[row + 2, ++cell].Value = users[row].Id.ToString();
                    ws.Cells[row + 2, ++cell].Value = users[row].FirstName;
                    ws.Cells[row + 2, ++cell].Value = users[row].LastName;
                    ws.Cells[row + 2, ++cell].Value = users[row].Sex;
                    if(settings.DayOfBirth)
                        ws.Cells[row + 2, ++cell].Value = users[row].BirthDate;
                    if (settings.City)
                        ws.Cells[row + 2, ++cell].Value = users[row].City.Title;
                    if (settings.PrivateMessages)
                        ws.Cells[row + 2, ++cell].Value = users[row].CanWritePrivateMessage;
                }
            }
        }
    }
}
