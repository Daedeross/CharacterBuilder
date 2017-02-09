namespace CharacterBuilder.Utilities.Tests
{
    using CharacterBuilder.Tags;
    using CharacterBuilder.Tags.Contract;
    using ExpressionEvaluator;
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using Xunit;

    public class ExpressionTests
    {
        private class TestScope
        {
            public dynamic itag1;
            public dynamic itag2;

            public TestScope()
            {
                itag1 = new IntTag<TestScope>();
                ((IntTag<TestScope>)itag1).Text = "1";
                itag2 = new IntTag<TestScope>();
                ((IntTag<TestScope>)itag2).Text = "3";
            }
        }

        [Fact]
        public void TestIntTagDiscovery()
        {
            string exText = "itag1 + itag2.FinalValue";

            var scope = new TestScope();

            var ce = new CompiledExpression<int>(exText);
            var del = ce.ScopeCompile<TestScope>();
            Assert
            var ex = ce.Expression as Expression<Func<TestScope, int>>;

            var result = ExpressionUtilities.FindMembers(ex, scope);

            var key1 = scope.itag1 as INotifyPropertyChanged;
            var key2 = scope.itag1 as INotifyPropertyChanged;

            Assert.True(result.ContainsKey(key1));
            Assert.True(result.ContainsKey(key2));
            Assert.True(result[key1].Contains("Value"));
            Assert.True(result[key2].Contains("FinalValue"));
        }
    }
}
