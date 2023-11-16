using CharacterBuilder.Foundation;
using CharacterBuilder.Foundation.Model;
using System.Collections.Generic;

namespace CharacterBuilder.Dsl.Model
{
    public class ParsedGives<T>
        where T : class, INamedItem
    {
        public List<PropertyReference> Targets { get; set; } = new List<PropertyReference>();
        public ParsedExpression<T, double> Expression { get; set; }
    }
}
