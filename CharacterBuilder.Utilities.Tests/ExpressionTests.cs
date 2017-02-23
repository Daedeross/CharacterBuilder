namespace CharacterBuilder.Utilities.Tests
{
    using CharacterBuilder.Foundation;
    using CharacterBuilder.Data;
    using CharacterBuilder.Data.Contract;
    using ExpressionEvaluator;
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using Xunit;

    public class ExpressionTests
    {
        public class TestScope: DynamicDataModel
        {
            public dynamic itag1;
            public dynamic itag2;

            public TestScope()
            {
                itag1 = new IntTag<TestScope>(this);
                itag1.Text = "1";
                itag2 = new IntTag<TestScope>(this);
                itag2.Text = "3";
            }
        }

        [Fact]
        public void TestIntTagDiscovery()
        {
            string exText = "itag1 + itag2.FinalValue";

            var scope = new TestScope();
            var x = scope.itag1.FinalValue;

            var ce = new CompiledExpression<int>(exText);
            var del = ce.ScopeCompile<TestScope>();
            var r = del(scope);

            //var lam = Expression.Lambda<Func<TestScope, int>>(Expression.Constant(1), Expression.Parameter(typeof(TestScope), "scope"));

            //var ex = ce.Expression as Expression<Func<TestScope, int>>;

            var result = ExpressionUtilities.FindMembers(ce.Expression, scope);

            var key1 = scope.itag1 as INotifyPropertyChanged;
            var key2 = scope.itag2 as INotifyPropertyChanged;
            var result1 = result[key1];
            var result2 = result[key2];

            Assert.True(result.ContainsKey(key1), "tag1 not found");
            Assert.True(result.ContainsKey(key2), "tag2 not found");
            Assert.True(result[key1].Contains("Value"));
            Assert.True(result[key2].Contains("FinalValue"));
        }
    }
}
