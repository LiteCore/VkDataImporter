using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using VkNet;
using VkNet.Model;
using VkNet.Exception;
using VkNet.Enums.Filters;


namespace VKDataImporter
{
    static class Authorizator
    {
        static public VkApi Api { get; } = new VkApi();
        static public string Code { get; set; } = "";
        static private string GetAuth()
        {
            //new AuthentificationCodeWindow().ShowDialog();
            return Code;
        }
        
        //static public bool Authorize(string login, string password)
        //{
        //    try
        //    {
        //        Api.Authorize(new ApiAuthParams()
        //        {
        //            ApplicationId = 865490,
        //            Login = login,
        //            Password = password,
        //            TwoFactorAuthorization = (() => GetAuth()),
        //            Settings = Settings.All
        //        });
        //        return true;
        //    }
        //    catch(VkApiAuthorizationException)
        //    {
        //        MessageWindow window = new MessageWindow("Ошибка", "Неверный логин или пароль!");
        //        window.ShowDialog();
        //        return false;
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageWindow window = new MessageWindow("Ошибка", ex.Message);
        //        window.ShowDialog();
        //        return false;
        //    }
        //}
        static public bool Authorize(string Token)
        {
            try
            {
                Api.Authorize(new ApiAuthParams()
                {
                    AccessToken = Token
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
        static public bool Authorize()
        {
            try
            {
                Api.Authorize(new ApiAuthParams()
                {
                    AccessToken = Properties.Settings.Default.Token
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
