using CharacterBuilder.Data;
using System.Linq.Expressions;

namespace CharacterBuilder.Dsl
{
    public class ParsedScript<T>
    {
        public ScriptType Type { get; private set; }

        public IntermediateParsedGives<T> Gives { get; private set; }

        public Expression Expression { get; private set; }

        public ParsedScript(IntermediateParsedGives<T> gives)
        {
            this.Type = ScriptType.Gives;
            Gives = gives;
            Expression = null;
        }

        public ParsedScript(Expression expression)
        {
            this.Type = ScriptType.Expression;
            Gives = null;
            Expression = expression;
        }
    }
}
