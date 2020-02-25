using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Utils;
using OfficeOpenXml;

namespace VKDataImporter
{
    static class DataImporter
    {
        static public Task<bool> ImportDataAsync(string group, string path, IProgress<Tuple<int,int>> progress)
        {
            return Task.Run(() => ImportData(group, path, progress));
        }
        static public bool ImportData(string group, string path, IProgress<Tuple<int, int>> progress)
        {
            Authorizator.Authorize();
            if (!Authorizator.Api.IsAuthorized)
            {
                try
                {
                    if (!Authorizator.Authorize(Properties.Settings.Default.Token))
                    {
                        throw new Exception("Требуется авторизация");
                    }
                }
                catch
                {
                    throw new Exception("Не удалось авторизироваться");
                }
            }

            try
            {
                GetDataAndWrite(group, path, progress);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //ExcelWriter.WriteUserInfo(saveFile.FileName, users);
            return true;
        }

        static private void GetDataAndWrite(string group, string path, IProgress<Tuple<int, int>> progress)
        {
            var _api = Authorizator.Api;
            ulong totalCount = 0;
            ulong offset = 0;
            int wsCounter = 1;
            using (var p = new ExcelPackage(new System.IO.FileInfo(path)))
            {
                for(int i = p.Workbook.Worksheets.Count; i > 0; i--)
                {
                    p.Workbook.Worksheets.Delete(i);
                }
                    
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

                do
                {
                    var users = _api.Groups.GetMembers(new GroupsGetMembersParams()
                    {
                        GroupId = group,
                        Fields = UsersFields.CanWritePrivateMessage | UsersFields.City | UsersFields.BirthDate | UsersFields.Sex,
                        Offset = (long)offset
                    });
                    for (int row = 0; row < users.Count; row++)
                    {
                        int cell = 0;
                        int rowRes = row + 2 + (int)offset - (wsCounter - 1) * 10000;
                        ws.Cells[rowRes, ++cell].Value = users[row].Id.ToString();
                        ws.Cells[rowRes, ++cell].Value = users[row].FirstName;
                        ws.Cells[rowRes, ++cell].Value = users[row].LastName;
                        ws.Cells[rowRes, ++cell].Value = users[row].Sex.ToString();
                        if (settings.DayOfBirth)
                            ws.Cells[rowRes, ++cell].Value = users[row].BirthDate ?? "";
                        if (settings.City)
                            ws.Cells[rowRes, ++cell].Value = users[row].City == null ? "" : users[row].City.Title;
                        if (settings.PrivateMessages)
                            ws.Cells[rowRes, ++cell].Value = users[row].CanWritePrivateMessage;
                    }
                    totalCount = users.TotalCount;
                    offset += 1000;
                    progress.Report(new Tuple<int, int>((int)offset, (int)totalCount));
                    if(offset % 10000 == 0)
                    {
                        ws = p.Workbook.Worksheets.Add($"Users{wsCounter}");
                        for (int field = 0; field < Fields.Count; field++)
                        {
                            ws.Cells[1, field + 1].Value = Fields[field];
                        }
                        wsCounter++;
                    }
                } while (totalCount > offset);
                p.Save();
            }
        }
    }
}
