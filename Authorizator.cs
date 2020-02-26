using VkNet;
using VkNet.Model;

namespace VKDataImporter
{
    internal static class Authorizator
    {
        static public VkApi Api { get; } = new VkApi();

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