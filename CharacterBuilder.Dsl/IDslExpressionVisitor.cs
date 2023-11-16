using CharacterBuilder.Foundation;
using CharacterBuilder.Foundation.Model;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CharacterBuilder.Dsl
{
    public interface IDslExpressionVisitor<T>: ICharacterBuilderVisitor<Payload<Expression>>
        where T : class, INamedItem
    {
        ICollection<PropertyReference> WatchedProperties { get; }

        ParameterExpression Scope { get; }
    }
}
