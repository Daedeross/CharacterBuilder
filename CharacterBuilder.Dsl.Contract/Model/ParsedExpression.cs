using CharacterBuilder.Foundation;
using CharacterBuilder.Foundation.Model;
using System;
using System.Collections.Generic;

namespace CharacterBuilder.Dsl.Model
{
    public class ParsedExpression<TTrait, TOut>
        where TTrait : class, INamedItem
    {
        public List<PropertyReference> WatchedProperties { get; set; }
        public Func<IScope<TTrait>, TOut> Delegate { get; set; }
        public Func<TOut> Scoped { get; set; }
    }
}
