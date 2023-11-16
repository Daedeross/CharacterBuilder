using CharacterBuilder.Foundation.Model;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CharacterBuilder.Dsl
{
    public class IntermediateParsedGives<T>
    {
        public List<PropertyReference> Targets { get; set; } = new List<PropertyReference>();
        public Expression Expression { get; set; }
    }
}
