using CharacterBuilder.Foundation;
using CharacterBuilder.Foundation.Model;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CharacterBuilder.Dsl
{
    public interface IDslVisitor<T> : ICharacterBuilderVisitor<ParsedScript<T>>
        where T : class, INamedItem
    {
        ParameterExpression Scope { get; }

        public ICollection<PropertyReference> WatchedProperties { get; }

        void Clear();
    }
}
