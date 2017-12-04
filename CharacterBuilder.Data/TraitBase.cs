using CharacterBuilder.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Data
{
    public abstract class TraitBase : DynamicDataModel, ITrait
    {
        private IDictionary<string, IModifier> _mods = new Dictionary<string, IModifier>();
        public IDictionary<string, IModifier> Mods => _mods;

        private IDictionary<string, ITag> _tags = new Dictionary<string, ITag>();
        public IDictionary<string, ITag> Tags => _tags;

        
    }
}
