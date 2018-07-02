using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricWCF.Common.Objects
{
    public class LocaleInfo
    {
        public string LangCode { get; set; }
        public string RegionCode { get; set; }
        public string Region { get; set; }
        public string Language { get; set; }

        private LocaleInfo() { }

        public static IEnumerable<LocaleInfo> Locations
        {
            get
            {
                return GetLocaleInfos();
            }
        }

        private static IEnumerable<LocaleInfo> GetLocaleInfos()
        {
            yield return new LocaleInfo { RegionCode = "au", Region = "Australia", Language = "", LangCode = "" };
            yield return new LocaleInfo { RegionCode = "bd", Region = "Bangladesh", Language = "", LangCode = "bn" };
            yield return new LocaleInfo { RegionCode = "cn", Region = "China", Language = "", LangCode = "" };
            yield return new LocaleInfo { RegionCode = "hk", Region = "Hong Kong", Language = "", LangCode = "" };
            yield return new LocaleInfo { RegionCode = "in", Region = "India", Language = "English", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "in", Region = "India", Language = "Hindi", LangCode = "hi" };
            yield return new LocaleInfo { RegionCode = "in", Region = "India", Language = "Malayalam", LangCode = "ml" };
            yield return new LocaleInfo { RegionCode = "in", Region = "India", Language = "Tamil", LangCode = "ta" };
            yield return new LocaleInfo { RegionCode = "in", Region = "India", Language = "Telugu", LangCode = "te" };
            yield return new LocaleInfo { RegionCode = "id", Region = "Indonesia", Language = "", LangCode = "id" };
            yield return new LocaleInfo { RegionCode = "il", Region = "Israel", Language = "English", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "il", Region = "Israel", Language = "Hebrew", LangCode = "iw" };
            yield return new LocaleInfo { RegionCode = "jp", Region = "Japan", Language = "", LangCode = "" };
            yield return new LocaleInfo { RegionCode = "lb", Region = "Lebanon", Language = "", LangCode = "ar" };
            yield return new LocaleInfo { RegionCode = "my", Region = "Malaysia", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "nz", Region = "New Zealand", Language = "", LangCode = "" };
            yield return new LocaleInfo { RegionCode = "pk", Region = "Pakistan", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "ph", Region = "Philippines", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "sa", Region = "Saudi Arabia", Language = "", LangCode = "ar" };
            yield return new LocaleInfo { RegionCode = "sg", Region = "Singapore", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "kr", Region = "South Korea", Language = "", LangCode = "" };
            yield return new LocaleInfo { RegionCode = "tw", Region = "Taiwan", Language = "", LangCode = "" };
            yield return new LocaleInfo { RegionCode = "th", Region = "Thailand", Language = "", LangCode = "th" };
            yield return new LocaleInfo { RegionCode = "ae", Region = "United Arab Emirates", Language = "", LangCode = "ar" };
            yield return new LocaleInfo { RegionCode = "vn", Region = "Vietnam", Language = "", LangCode = "vi" };
            yield return new LocaleInfo { RegionCode = "at", Region = "Austria", Language = "", LangCode = "de" };
            yield return new LocaleInfo { RegionCode = "be", Region = "Belgium", Language = "Dutch", LangCode = "nl" };
            yield return new LocaleInfo { RegionCode = "be", Region = "Belgium", Language = "French", LangCode = "fr" };
            yield return new LocaleInfo { RegionCode = "bw", Region = "Botswana", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "bg", Region = "Bulgaria", Language = "", LangCode = "bg" };
            yield return new LocaleInfo { RegionCode = "cs", Region = "Czech Republic", Language = "", LangCode = "cs_cz" };

            yield return new LocaleInfo { RegionCode = "eg", Region = "Egypt", Language = "", LangCode = "ar" };
            yield return new LocaleInfo { RegionCode = "et", Region = "Ethiopia", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "", Region = "France", Language = "", LangCode = "fr" };
            yield return new LocaleInfo { RegionCode = "", Region = "Germany", Language = "", LangCode = "de" };
            yield return new LocaleInfo { RegionCode = "gh", Region = "Ghana", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "gr", Region = "Greece", Language = "", LangCode = "el" };
            yield return new LocaleInfo { RegionCode = "hu", Region = "Hungary", Language = "", LangCode = "hu" };
            yield return new LocaleInfo { RegionCode = "ie", Region = "Ireland", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "", Region = "Italy", Language = "", LangCode = "it" };
            yield return new LocaleInfo { RegionCode = "ke", Region = "Kenya", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "lv", Region = "Latvia", Language = "", LangCode = "lv" };
            yield return new LocaleInfo { RegionCode = "lt", Region = "Lithuania", Language = "", LangCode = "lt" };
            yield return new LocaleInfo { RegionCode = "ma", Region = "Morocco", Language = "", LangCode = "fr" };
            yield return new LocaleInfo { RegionCode = "na", Region = "Namibia", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "nl", Region = "Netherlands", Language = "", LangCode = "nl" };
            yield return new LocaleInfo { RegionCode = "ng", Region = "Nigeria", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "no", Region = "Norway", Language = "", LangCode = "no" };
            yield return new LocaleInfo { RegionCode = "pl", Region = "Poland", Language = "", LangCode = "pl" };
            yield return new LocaleInfo { RegionCode = "pt", Region = "Portugal", Language = "", LangCode = "PT_pt" };

            yield return new LocaleInfo { RegionCode = "ro", Region = "Romania", Language = "", LangCode = "ro" };
            yield return new LocaleInfo { RegionCode = "ru", Region = "Russia", Language = "", LangCode = "ru" };
            yield return new LocaleInfo { RegionCode = "sn", Region = "Senegal", Language = "", LangCode = "fr" };
            yield return new LocaleInfo { RegionCode = "rs", Region = "Serbia", Language = "", LangCode = "sr" };
            yield return new LocaleInfo { RegionCode = "si", Region = "Slovenia", Language = "", LangCode = "sl" };
            yield return new LocaleInfo { RegionCode = "sk", Region = "Slovakia", Language = "", LangCode = "sk" };
            yield return new LocaleInfo { RegionCode = "za", Region = "South Africa", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "se", Region = "Sweden", Language = "", LangCode = "sv" };
            yield return new LocaleInfo { RegionCode = "ch", Region = "Switzerland", Language = "", LangCode = "fr-CH" };

            yield return new LocaleInfo { RegionCode = "ch", Region = "Switzerland", Language = "", LangCode = "de" };
            yield return new LocaleInfo { RegionCode = "tz", Region = "Tanzania", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "tr", Region = "Turkey", Language = "", LangCode = "tr" };
            yield return new LocaleInfo { RegionCode = "ug", Region = "Uganda", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "ua", Region = "Ukraine", Language = "Russian", LangCode = "ru" };
            yield return new LocaleInfo { RegionCode = "ua", Region = "Ukraine", Language = "Ukranian", LangCode = "uk" };
            yield return new LocaleInfo { RegionCode = "uk", Region = "United Kingdom", Language = "", LangCode = "" };
            yield return new LocaleInfo { RegionCode = "zw", Region = "Zimbabwe", Language = "", LangCode = "en" };
            yield return new LocaleInfo { RegionCode = "ar", Region = "Argentina", Language = "", LangCode = "es" };

            yield return new LocaleInfo { RegionCode = "br", Region = "Brazil", Language = "", LangCode = "pt-BR" };
            yield return new LocaleInfo { RegionCode = "ca", Region = "Canada", Language = "English", LangCode = "" };
            yield return new LocaleInfo { RegionCode = "ca", Region = "Canada", Language = "French", LangCode = "fr" };
            yield return new LocaleInfo { RegionCode = "cl", Region = "Chile", Language = "", LangCode = "es" };
            yield return new LocaleInfo { RegionCode = "co", Region = "Colombia", Language = "", LangCode = "es" };
            yield return new LocaleInfo { RegionCode = "cu", Region = "Cuba", Language = "", LangCode = "es" };
            yield return new LocaleInfo { RegionCode = "mx", Region = "Mexico", Language = "", LangCode = "es" };
            yield return new LocaleInfo { RegionCode = "pe", Region = "Peru", Language = "", LangCode = "es" };
            yield return new LocaleInfo { RegionCode = "us", Region = "United States", Language = "English", LangCode = "" };
            yield return new LocaleInfo { RegionCode = "us", Region = "United States", Language = "Spanish", LangCode = "es" };
            yield return new LocaleInfo { RegionCode = "ve", Region = "Venezuela", Language = "", LangCode = "es" };
        }
    }
}
