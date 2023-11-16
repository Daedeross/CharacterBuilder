using Antlr4.Runtime.Misc;
using CharacterBuilder.Data;
using CharacterBuilder.Foundation;
using CharacterBuilder.Foundation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CharacterBuilder.Dsl
{
    public class DslExpressionVisitor<TTrait> : CharacterBuilderBaseVisitor<Payload<Expression>>, IDslExpressionVisitor<TTrait>
        where TTrait : class, INamedItem
    {
        private static readonly Expression DefaultNumericValue = Expression.Constant(0d);

        private readonly IReadOnlyDictionary<string, MethodInfo> _functions;
        private readonly ParameterExpression _scope;
        private readonly Expression _me;
        private readonly Expression _categories;
        private readonly PropertyInfo _categoriesIndexer;
        private readonly PropertyInfo _traitsIndexer;
        private readonly IEnumerable<Type> _traitTypes;

        private HashSet<PropertyReference> _watchedProperties = new();

        public ICollection<PropertyReference> WatchedProperties => _watchedProperties;

        public DslExpressionVisitor()
            : this(new Dictionary<string, MethodInfo>(),
                  new Type[] { /*typeof(ILeveledTrait), typeof(IAttribute)*/ } )
        {
        }

        public DslExpressionVisitor(IReadOnlyDictionary<string, MethodInfo> functions,
            Type[] otherTypes)
        {
            _functions = functions;
            var type = typeof(IScope<TTrait>);
            _scope = Expression.Parameter(type, "scope");
            _me = Expression.Property(_scope, type.GetProperty(nameof(IScope<TTrait>.Me)));
            _categories = Expression.Property(_scope, type.GetProperty(nameof(IScope<TTrait>.Traits)));
            _categoriesIndexer = typeof(IDictionary<string, ITraitContainer>).GetProperty("Item");
            _traitsIndexer = typeof(IDictionary<string, ITrait>).GetProperty("Item");

            _traitTypes = otherTypes;
        }

        public ParameterExpression Scope => _scope;

        public override Payload<Expression> VisitScript([NotNull] CharacterBuilderParser.ScriptContext context)
        {
            var expr = context.expression();
            if (expr is null)
            {
                throw new NotSupportedException($"Use {typeof(DslVisitor<TTrait>)} to parse the root script.");
            }
            else
            {
                return expr.Accept(this);
            }
        }

        public Payload<Expression> VisitExpression([NotNull] CharacterBuilderParser.ExpressionContext context)
        {
            return context.Accept(this);
        }

        public override Payload<Expression> VisitParentheticalExpression([NotNull] CharacterBuilderParser.ParentheticalExpressionContext context)
        {
            return VisitExpression(context.expression());
        }

        public override Payload<Expression> VisitFunctionCall([NotNull] CharacterBuilderParser.FunctionCallContext context)
        {
            return context.function().Accept(this);
        }

        public override Payload<Expression> VisitAtomicExpression([NotNull] CharacterBuilderParser.AtomicExpressionContext context)
        {
            return VisitAtom(context.atom());
        }

        public override Payload<Expression> VisitPowerExpression([NotNull] CharacterBuilderParser.PowerExpressionContext context)
        {
            var left = VisitExpression(context.expression(0));
            var right = VisitExpression(context.expression(1));
            return Expression.Power(left.Value, right.Value);
        }

        public override Payload<Expression> VisitNumericUnaryExpression([NotNull] CharacterBuilderParser.NumericUnaryExpressionContext context)
        {
            var expr = VisitExpression(context.expression());

            return context.op.Type switch
            {
                CharacterBuilderParser.PLUS => expr,
                CharacterBuilderParser.MINUS => Expression.Negate(expr),
                _ => throw new InvalidOperationException($"Unrecognized unary operator {context.op.Text}")
            };
        }

        public override Payload<Expression> VisitBooleanUnaryExpression([NotNull] CharacterBuilderParser.BooleanUnaryExpressionContext context)
        {
            return Expression.Not(VisitExpression(context.expression()));
        }

        private static Expression MakeDouble(Expression expression)
        {
            if (expression.Type == typeof(double))
            {
                return expression;
            }

            return Expression.Convert(expression, typeof(double));
        }

        public override Payload<Expression> VisitMulDivExpression([NotNull] CharacterBuilderParser.MulDivExpressionContext context)
        {
            var op = context.op;
            var left = MakeDouble(VisitExpression(context.expression(0)));
            var right = MakeDouble(VisitExpression(context.expression(1)));

            return op.Type switch
            {
                CharacterBuilderParser.TIMES => Expression.Multiply(left, right),
                CharacterBuilderParser.DIV => Expression.Divide(left, right),
                _ => throw new InvalidOperationException($"Unrecognized binary operator {op.Text}")
            };
        }

        public override Payload<Expression> VisitAddSubExpression([NotNull] CharacterBuilderParser.AddSubExpressionContext context)
        {
            var op = context.op;
            var left = MakeDouble(VisitExpression(context.expression(0)));
            var right = MakeDouble(VisitExpression(context.expression(1)));

            return op.Type switch
            {
                CharacterBuilderParser.PLUS => Expression.Add(left, right),
                CharacterBuilderParser.MINUS => Expression.Subtract(left, right),
                _ => throw new InvalidOperationException($"Unrecognized binary operator {op.Text}")
            };
        }

        public override Payload<Expression> VisitComparisonExpression([NotNull] CharacterBuilderParser.ComparisonExpressionContext context)
        {
            var op = context.op;
            var left = VisitExpression(context.expression(0));
            var right = VisitExpression(context.expression(1));

            return op.Type switch
            {
                CharacterBuilderParser.EQ => Expression.Equal(left, right),
                CharacterBuilderParser.GT => Expression.GreaterThan(left, right),
                CharacterBuilderParser.LT => Expression.LessThan(left, right),
                CharacterBuilderParser.GEQ => Expression.GreaterThanOrEqual(left, right),
                CharacterBuilderParser.LEQ => Expression.LessThanOrEqual(left, right),
                _ => throw new InvalidOperationException($"Unrecognized binary operator {op.Text}")
            };
        }

        public override Payload<Expression> VisitBooleanBinaryExpression([NotNull] CharacterBuilderParser.BooleanBinaryExpressionContext context)
        {
            var op = context.op;
            var left = VisitExpression(context.expression(0));
            var right = VisitExpression(context.expression(1));

            return op.Type switch
            {
                CharacterBuilderParser.AND => Expression.AndAlso(left, right),
                CharacterBuilderParser.OR => Expression.OrElse(left, right),
                _ => throw new InvalidOperationException($"Unrecognized binary operator {op.Text}")
            };
        }

        public override Payload<Expression> VisitTernaryExpression([NotNull] CharacterBuilderParser.TernaryExpressionContext context)
        {
            var condition = VisitExpression(context.expression(0));
            var if_true = VisitExpression(context.expression(1));
            var if_false = VisitExpression(context.expression(2));

            return Expression.IfThenElse(condition, if_true, if_false);
        }

        public override Payload<Expression> VisitAtom([NotNull] CharacterBuilderParser.AtomContext context)
        {
            return context.children[0].Accept(this);
            //return context.children[0] switch
            //{
            //    CharacterBuilderParser.Numeric_literalContext ctx => VisitNumeric_literal(ctx),
            //    CharacterBuilderParser.String_literalContext ctx => VisitString_literal(ctx),
            //    CharacterBuilderParser.VariableContext ctx => VisitVariable(ctx),
            //    _ => throw new InvalidOperationException($"Unknown atomic expression '{context.GetText()}'")
            //};
        }

        public override Payload<Expression> VisitString_literal([NotNull] CharacterBuilderParser.String_literalContext context)
        {
            var str = context.GetText().Trim('\'', '"');

            return Expression.Constant(str, typeof(string));
        }

        public override Payload<Expression> VisitNumeric_literal([NotNull] CharacterBuilderParser.Numeric_literalContext context)
        {
            var text = context.GetText();

            /// All numeric types are treated as <see cref="double"/> until needed otherwise.
            if (double.TryParse(text, out var val))
            {
                return Expression.Constant(val, typeof(double));
            }
            else
            {
                throw new InvalidCastException($"Unable to parse {text} as a number");
            }
        }

        public override Payload<Expression> VisitBoolean_literal([NotNull] CharacterBuilderParser.Boolean_literalContext context)
        {
            return context.TRUE() != null
                ? Expression.Constant(true)
                : Expression.Constant(false);
        }

        public override Payload<Expression> VisitFunction([NotNull] CharacterBuilderParser.FunctionContext context)
        {
            var fName = context.NAME().GetText();

            if (!_functions.TryGetValue(fName, out var methodInfo))
            {
                throw new IndexOutOfRangeException($"'{fName}' is not a defined function");
            }

            var args = GetArgList(context.argList());

            return Expression.Call(methodInfo, args);
        }

        public override Payload<Expression> VisitArgList([NotNull] CharacterBuilderParser.ArgListContext context)
        {
            throw new NotSupportedException();
        }

        private Expression[] GetArgList([NotNull] CharacterBuilderParser.ArgListContext context)
        {
            var args = new List<Expression>();

            foreach (var arg in context.expression())
            {
                args.Add(arg.Accept(this));
            }

            return args.ToArray();
        }

        public override Payload<Expression> VisitVariable([NotNull] CharacterBuilderParser.VariableContext context)
        {
            string property = context.COLON() is null ? "GivesedRating" : context.property().GetText();

            var trait = context.trait().Accept(this);

            var (category, name) = ((string category, string name))trait.Extra;

            _watchedProperties.Add(new PropertyReference(category, name, property));

            if (trait.Value.Type.GetProperty(property) is null)
            {
                foreach (var type in _traitTypes.Where(t => typeof(ITrait).IsAssignableFrom(t)))
                {
                    var pi = type.GetProperty(property);
                    if (pi is not null)
                    {
                        return WrapInTryFault(
                            MakeDouble(
                                Expression.Property(
                                    Expression.Convert(trait, type), pi)), DefaultNumericValue);
                    }
                }
            }

            return WrapInTryFault(
                MakeDouble(
                    Expression.Property(trait, property)), DefaultNumericValue);
        }

        public override Payload<Expression> VisitTrait([NotNull] CharacterBuilderParser.TraitContext context)
        { 
            var self = context.self();
            if (self != null)
            {
                return self.Accept(this);
            }

            var ancestor = context.ancestor();
            if (ancestor != null)
            {
                return ancestor.Accept(this);
            }

            var category = context.trait_type().Accept(this);
            var name = context.trait_name().Accept(this);

            return new(Expression.MakeIndex(category, _traitsIndexer, new[] { name.Value } ), ((string)category.Extra, (string)name.Extra));
        }

        public override Payload<Expression> VisitTrait_type([NotNull] CharacterBuilderParser.Trait_typeContext context)
        {
            var name = context.GetText();
            var expr = Expression.Constant(name, typeof(string));
            return new(Expression.MakeIndex(_categories, _categoriesIndexer, new[] { expr }), name);
        }

        public override Payload<Expression> VisitTrait_name([NotNull] CharacterBuilderParser.Trait_nameContext context)
        {
            var name = context.GetText();
            var expr = Expression.Constant(name, typeof(string));
            return new(expr, name);
        }

        public override Payload<Expression> VisitSelf([NotNull] CharacterBuilderParser.SelfContext context)
        {
            return new Payload<Expression>(_me, ("_me", "_me"));
        }

        public override Payload<Expression> VisitAncestor([NotNull] CharacterBuilderParser.AncestorContext context)
        {
            throw new NotImplementedException("Targeting ancestor trait not implemented yet.");
        }

        public override Payload<Expression> VisitGives([NotNull] CharacterBuilderParser.GivesContext context)
        {
            throw new NotSupportedException();
        }

        public override Payload<Expression> VisitHasExpression([NotNull] CharacterBuilderParser.HasExpressionContext context)
        {
            return context.has().Accept(this);
        }

        public override Payload<Expression> VisitHas([NotNull] CharacterBuilderParser.HasContext context)
        {
            var category = context.trait_type().GetText();
            var name = context.trait_name().GetText();

            _watchedProperties.Add(new(category, name, "_has"));
            var lambda = MakeHas(category, name);

            return Expression.Invoke(Expression.Constant(lambda), _scope);
        }

        #region Helpers

        private static Func<IScope<TTrait>, bool> MakeHas(string category, string trait)
        {
            return new Func<IScope<TTrait>, bool>(
                s =>
                {
                    return s.Traits.TryGetValue(category, out var t) && t.ContainsKey(trait);
                }
                );
        }

        /// <summary>
        /// Wraps an expression with a try..catch block.
        /// </summary>
        /// <param name="tryExpression">The expression to wrap.</param>
        /// <param name="faultExpression">The expression to use if an exception is caught. Result type must match <paramref name="tryExpression"/></param>
        /// <param name="exceptionType">Thy type of exception to catch. Defaults to <see cref="Exception"/>.</param>
        /// <returns>The wrapped expression tree.</returns>
        private static Expression WrapInTryFault(Expression tryExpression, Expression faultExpression, Type exceptionType = null)
        {
            return Expression.TryCatch(
                tryExpression,
                Expression.Catch(exceptionType ?? typeof(Exception), faultExpression)
                );
        }

        #endregion
    }
}
