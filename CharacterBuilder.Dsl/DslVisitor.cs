using Antlr4.Runtime.Tree;
using CharacterBuilder.Foundation;
using CharacterBuilder.Foundation.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace CharacterBuilder.Dsl
{
    public class DslVisitor<T> : CharacterBuilderBaseVisitor<ParsedScript<T>>, IDslVisitor<T>
        where T : class, INamedItem
    {
        private readonly IDslExpressionVisitor<T> _expressionVisitor;
        private readonly IDslGivesVisitor<T> _augmentVisitor;

        public ICollection<PropertyReference> WatchedProperties => _expressionVisitor.WatchedProperties;

        public ParameterExpression Scope => _expressionVisitor.Scope;

        public DslVisitor(IDslExpressionVisitor<T> expressionVisitor, IDslGivesVisitor<T> augmentVisitor)
        {
            _expressionVisitor = expressionVisitor;
            _augmentVisitor = augmentVisitor;
        }

        public void Clear()
        {
            _expressionVisitor.WatchedProperties.Clear();
        }


        public override ParsedScript<T> Visit(IParseTree tree)
        {
            return base.Visit(tree);
        }

        public override ParsedScript<T> VisitScript([NotNull] CharacterBuilderParser.ScriptContext context)
        {
            var aug = context.gives();
            var expr = context.expression();

            if (aug is not null)
            {
                return new ParsedScript<T>(aug.Accept(_augmentVisitor));
            }
            else if (expr is not null)
            {
                return new ParsedScript<T>(expr.Accept(_expressionVisitor));
            }
            else
            {
                throw new InvalidOperationException("Unknown script context");
            }
        }
    }
}
