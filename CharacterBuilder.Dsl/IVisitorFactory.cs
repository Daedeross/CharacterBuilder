using CharacterBuilder.Foundation.Model;
using System.Linq.Expressions;

namespace CharacterBuilder.Dsl
{
    public interface IVisitorFactory
    {
        ICharacterBuilderVisitor<Payload<Expression>> ForExpression<T>();

        ICharacterBuilderVisitor<IntermediateParsedGives<T>> ForGives<T>();

        ICharacterBuilderVisitor<Result<T>> Create<T>();

        void Release<T>(T obj);
    }
}
