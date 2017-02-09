using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CharacterBuilder.Tags.Contract;

namespace CharacterBuilder.Utilities
{
    public static class ExpressionUtilities
    {
        public static Dictionary<INotifyPropertyChanged, HashSet<string>> FindMembers<TScope, TResult>(Expression<Func<TScope, TResult>> ex, TScope scope)
        {
            var set = FindMembersTraverse(ex, scope);
            var dict = new Dictionary<INotifyPropertyChanged, HashSet<string>>();
            foreach ((INotifyPropertyChanged sender, string property) in set)
            {
                if (dict.ContainsKey(sender))
                {
                    dict[sender].Add(property);
                }
                else
                {
                    dict.Add(sender, new HashSet<string>(new string[] { property }));
                }
            }
            return dict;
        }

        private static HashSet<(INotifyPropertyChanged sender, string property)> FindMembersTraverse<TScope>(
            Expression ex, TScope scope)
        {

            if (ex is MemberExpression)
            {
                var me = ex as MemberExpression;
                string memberName = null;
                INotifyPropertyChanged container = null;

                if (me.Member.MemberType == MemberTypes.Property)
                {
                    var pi = me.Member as PropertyInfo;
                    if (typeof(IIntTag).IsAssignableFrom(pi.PropertyType))
                    {
                        var outerContainer = EvaluateContainer(me.Expression, scope);
                        container = pi.GetValue(outerContainer) as INotifyPropertyChanged;
                        memberName = "Value";
                    }
                    else
                    {
                        memberName = me.Member.Name;
                        container = EvaluateContainer(me.Expression, scope);
                    }
                }

                if (container != null)
                {
                    return new HashSet<(INotifyPropertyChanged, string)>(
                        new(INotifyPropertyChanged, string )[]
                        { (container, memberName) }
                        );
                }
            }
            else if (ex is BinaryExpression)
            {
                var be = ex as BinaryExpression;
                var left = FindMembersTraverse(be.Left, scope);
                var right = FindMembersTraverse(be.Right, scope);
                left.UnionWith(right);
                return left;
            }
            else if (ex is UnaryExpression)
            {
                var ue = ex as UnaryExpression;
                return FindMembersTraverse(ue.Operand, scope);
            }
            else if (ex is DynamicExpression)
            {
                var de = ex as DynamicExpression;

                if (typeof(GetMemberBinder).IsAssignableFrom(de.Binder.GetType()))
                {
                    var binder = de.Binder as GetMemberBinder;
                    string memberName = null;
                    INotifyPropertyChanged container = null;

                    object result = EvaluateMember(ex, scope);

                    if (   typeof(IIntTag).IsAssignableFrom(result.GetType()) 
                        || typeof(IFloatTag).IsAssignableFrom(result.GetType())
                        || typeof(IStringTag).IsAssignableFrom(result.GetType()))
                    {
                        var outerContainer = EvaluateContainer(de.Arguments[0], scope);
                        container = (INotifyPropertyChanged)result;
                        memberName = "Value";
                    }
                    else
                    {
                        memberName = binder.Name;
                        container = EvaluateContainer(de.Arguments[0], scope);
                    }


                    if (container != null)
                    {
                        return new HashSet<(INotifyPropertyChanged, string)>(
                            new(INotifyPropertyChanged, string)[]
                            { (container, memberName) }
                            );
                    }
                }
                else
                {
                    var set = new HashSet<(INotifyPropertyChanged, string)>();

                    foreach (var arg in de.Arguments)
                    {
                        set.UnionWith(FindMembersTraverse(arg, scope));
                    }
                    return set;
                }
            }
            return new HashSet<(INotifyPropertyChanged, string)>();
        }

        static INotifyPropertyChanged EvaluateContainer<TScope>(Expression ex, TScope scope)
        {
            var scopeParam = GetParameterExpression(ex);
            try
            {
                var lambda = Expression.Lambda<Func<TScope, INotifyPropertyChanged>>(
                    WrapExpression<INotifyPropertyChanged>(ex),
                    new ParameterExpression[] { scopeParam });
                var del = lambda.Compile();
                return del(scope);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        static object EvaluateMember<TScope>(Expression ex, TScope scope)
        {
            var scopeParam = GetParameterExpression(ex);
            try
            {
                var lambda = Expression.Lambda<Func<TScope, object>>(
                    WrapExpression<object>(ex),
                    new ParameterExpression[] { scopeParam });
                var del = lambda.Compile();
                return del(scope);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        static ParameterExpression GetParameterExpression(Expression ex)
        {
            if (ex is MemberExpression)
            {
                var me = ex as MemberExpression;
                return GetParameterExpression(me.Expression);
            }
            else if (ex is BinaryExpression)
            {
                var be = ex as BinaryExpression;
                var left = GetParameterExpression(be.Left);
                var right = GetParameterExpression(be.Right);
                return left ?? right;
            }
            else if (ex is ParameterExpression)
            {
                return ex as ParameterExpression;
            }
            else if (ex is DynamicExpression)
            {
                var de = ex as DynamicExpression;

                foreach (var arg in de.Arguments)
                {
                    var param = GetParameterExpression(arg);
                    if (param != null) return param;
                }
            }
            return null;
        }

        static Expression WrapExpression<TResult>(Expression ex)
        {
            if (!typeof(TResult).IsAssignableFrom(ex.Type))
            {
                return Expression.Convert(ex, typeof(TResult));
            }
            return ex;
        }
    }
}
