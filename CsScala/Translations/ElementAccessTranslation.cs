using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CsScala.Translations
{
    class ElementAccessTranslation
    {
        public ElementAccessTranslation(XElement data)
        {
            TranslationManager.InitProperties(this, data);
        }

        public string Match { get; set; }
        public string ReplaceGet { get; set; }
        public string ReplaceAssign { get; set; }

        public static ElementAccessTranslation Get(string typeStr)
        {
            var match = TranslationManager.MatchString(typeStr);

            return TranslationManager.ElementAccesses.SingleOrDefault(o => o.Match == match);
        }
    }
}
