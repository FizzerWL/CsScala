using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CsScala.Translations
{
    class NeedsClassTagTranslation
    {
        public NeedsClassTagTranslation(XElement data)
        {
            TranslationManager.InitProperties(this, data);
        }

        public string Match { get; set; }

        public string[] TypeParametersOpt { get; set; } //null means all type parameters, for convenience

        public static NeedsClassTagTranslation Get(string typeStr)
        {
            var match = TranslationManager.MatchString(typeStr);
            return TranslationManager.NeedsClassTags.SingleOrDefault(o => o.Match == match);
        }
    }
}
