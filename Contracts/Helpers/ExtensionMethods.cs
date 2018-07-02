using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricWCF.Common.Helpers
{
    public static class ExtensionMethods
    {
        public static bool Contains(this string heystack, string value, bool matchCase = true)
        {
            if (string.IsNullOrWhiteSpace(heystack) || string.IsNullOrWhiteSpace(value)) return false;

            if (matchCase)
                return heystack.Contains(value);
            else
            {
                 return heystack.ToUpper().Contains(value.ToUpper());
            }
        }

        public static string HtmlDecode(this string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText)) return null;
            return System.Web.HttpUtility.HtmlDecode(htmlText);
        }

        public static byte[] DecodeDataUri(this Uri uri)
        {
            var localPath = uri.LocalPath;
            var contentType = localPath.Substring(0, localPath.IndexOf(","));
            var content = localPath.Substring(contentType.Length + 1);
            return Convert.FromBase64String(content);
        }
    }
}
