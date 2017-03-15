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
        public string TypeParametersOpt { get; set; } //null means all type parameters, for convenience

        HashSet<string> _types;
        public HashSet<string> TypesHashSet
        {
            get
            {
                if (TypeParametersOpt == null)
                    return null;
                if (_types == null)
                    _types = this.TypeParametersOpt.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToHashSet(true);
                return _types;
            }
        }


        public static NeedsClassTagTranslation Get(string typeStr)
        {
            var match = TranslationManager.MatchString(typeStr);
            return TranslationManager.NeedsClassTags.SingleOrDefault(o => o.Match == match);
        }
    }
}
