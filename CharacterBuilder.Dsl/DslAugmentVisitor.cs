using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using CharacterBuilder.Foundation;
using CharacterBuilder.Foundation.Model;
using System;

namespace CharacterBuilder.Dsl
{
    public class DslGivesVisitor<T> : CharacterBuilderBaseVisitor<IntermediateParsedGives<T>>, IDslGivesVisitor<T>
        where T : class, INamedItem
    {
        private readonly IDslExpressionVisitor<T> _expressionVisitor;

        public DslGivesVisitor(IDslExpressionVisitor<T> expressionVisitor)
        {
            _expressionVisitor = expressionVisitor;
        }

        public override IntermediateParsedGives<T> Visit(IParseTree tree)
        {
            throw new NotSupportedException();
        }

        public override IntermediateParsedGives<T> VisitGives([NotNull] CharacterBuilderParser.GivesContext context)
        {
            var result = context.target().Accept(this);
            result.Expression = context.expression().Accept(_expressionVisitor);

            return result;
        }

        public override IntermediateParsedGives<T> VisitTarget([NotNull] CharacterBuilderParser.TargetContext context)
        {
            var result = new IntermediateParsedGives<T>();

            foreach (var target in context.variable())
            {
                result.Targets.Add(GetTarget(target));
            }

            return result;
        }

        public override IntermediateParsedGives<T> VisitVariable([NotNull] CharacterBuilderParser.VariableContext context)
        {
            throw new NotSupportedException();
        }

        private PropertyReference GetTarget([NotNull] CharacterBuilderParser.VariableContext context)
        {
            string property = context.COLON() is null ? "GivesedRating" : context.property().GetText();

            (string category, string name) = GetTrait(context.trait());

            return new(category, name, property);
        }

        private (string, string) GetTrait([NotNull] CharacterBuilderParser.TraitContext context)
        {
            if (context.self() != null)
            {
                return ("_me", "_me");
            }

            var category = context.trait_type().GetText();
            var name = context.trait_name().GetText();

            return (category, name);
        }
    }
}
