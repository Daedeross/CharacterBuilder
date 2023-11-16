using System.Collections.Generic;
using System.Collections.Specialized;

namespace CharacterBuilder.Data
{
    public interface ITraitContainer<T>: IDictionary<string, T>, INotifyCollectionChanged
        where T: class, ITrait
    {
        public string Name { get; }
        bool OwnsObjects { get; set; }
        bool Valid { get; }
        string Summary { get; }
    }
}
