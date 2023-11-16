using CharacterBuilder.Foundation;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CharacterBuilder.Data
{
    public abstract class TraitBase : DynamicDataModel, ITrait
    {
        private readonly IDictionary<string, IModifier> _mods = new ConcurrentDictionary<string, IModifier>();
        public IDictionary<string, IModifier> Mods => _mods;

        private readonly IDictionary<string, ITag> _tags = new ConcurrentDictionary<string, ITag>();
        public IDictionary<string, ITag> Tags => _tags;
    }
}
