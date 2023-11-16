using System.Collections.Generic;
using System.Collections.Specialized;

namespace CharacterBuilder.Data
{
    public interface ITraitContainer : IDictionary<string, ITrait>, INotifyCollectionChanged
    {
        public string Name { get; }
        bool OwnsObjects { get; set; }
        bool Valid { get; }
        string Summary { get; }
    }
}
