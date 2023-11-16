using CharacterBuilder.Dsl.Model;
using CharacterBuilder.Foundation;
using CharacterBuilder.Foundation.Model;

namespace CharacterBuilder.Dsl
{
    public interface IDslParser<T>
        where T : class, INamedItem
    {
        Result<ParsedExpression<T, TRet>> ParseExpression<TRet>(string script, IScope<T> scope = null);

        Result<ParsedGives<T>> ParseAgument(string script, IScope<T> scope = null);
    }
}
