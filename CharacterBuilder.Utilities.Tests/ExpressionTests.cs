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
        private static int v1 = 3;
        private static int v2 = 11;

        public class TestScope: DynamicDataModel
        {
            private dynamic _tagi1;
            public dynamic TagI1
            {
                get { return _tagi1; }
                set
                {
                    if (_tagi1 != value)
                    {
                        _tagi1 = value;
                        RaisePropertyChanged(nameof(TagI1));
                    }
                }
            }
            private dynamic _tagi2;
            public dynamic TagI2
            {
                get { return _tagi2; }
                set
                {
                    if (_tagi2 != value)
                    {
                        _tagi2 = value;
                        RaisePropertyChanged(nameof(TagI2));
                    }
                }
            }

            private dynamic _tagf1;
            public dynamic TagF1
            {
                get { return _tagf1; }
                set
                {
                    if (_tagf1 != value)
                    {
                        _tagf1 = value;
                        RaisePropertyChanged(nameof(TagF1));
                    }
                }
            }


            public TestScope()
            {
                _tagi1 = new IntTag<TestScope>(this) { Text = v1.ToString() };
                _tagi2 = new IntTag<TestScope>(this) { Text = v2.ToString() };
                //_tagf1 = new FloatTag
            }
        }

        [Fact]
        public void TestIntTagDiscovery()
        {
            string exText = "TagI1 + TagI2.FinalValue";

            var scope = new TestScope();

            var ce = new CompiledExpression<int>(exText);
            var del = ce.ScopeCompile<TestScope>();
            var r = del(scope);

            var result = ExpressionUtilities.FindMembers(ce.Expression, scope);

            var key1 = scope.TagI1 as INotifyPropertyChanged;
            var key2 = scope.TagI2 as INotifyPropertyChanged;
            var result1 = result[key1];
            var result2 = result[key2];

            Assert.True(result.ContainsKey(key1), "tag1 not found");
            Assert.True(result.ContainsKey(key2), "tag2 not found");
            Assert.True(result[key1].Contains("Value"));
            Assert.True(result[key2].Contains("FinalValue"));
        }

        [Fact]
        public void TestIntTagMath()
        {
            string exAddText = "TagI1 + TagI2";
            string exSubText = "TagI1 - TagI2";
            string exMulText = "TagI1 * TagI2";
            string exDivText = "TagI1 / TagI2";

            var scope = new TestScope();

            var ce = new CompiledExpression<int>(exAddText);
            var del = ce.ScopeCompile<TestScope>();
            var r = del(scope);
            Assert.Equal(v1 + v2, r);

            ce = new CompiledExpression<int>(exSubText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 - v2, r);

            ce = new CompiledExpression<int>(exMulText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 * v2, r);

            ce = new CompiledExpression<int>(exDivText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 / v2, r);
        }

        [Fact]
        public void TestDivideIntTags()
        {

        }
    }
}
