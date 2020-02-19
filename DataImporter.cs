using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Enums.Filters;

namespace VKDataImporter
{
    class DataImporter
    {
        static VkApi api = new VkApi();

        static public void Authorize(string login, string password)
        {
            api.Authorize(new ApiAuthParams()
            {
                ApplicationId = 865490,
                Login = login,
                Password = password,
                Settings = Settings.All
            });
        }
    }
}
