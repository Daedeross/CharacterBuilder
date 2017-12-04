namespace CharacterBuilder.Utilities.Tests
{
    using CharacterBuilder.Foundation;
    using CharacterBuilder.Data;
    using ExpressionEvaluator;
    using System;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq.Expressions;
    using Xunit;

    public class ExpressionTests
    {
        private static double v1 = 3;
        private static double v2 = 11;

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

            private dynamic _tagf2;
            public dynamic TagF2
            {
                get { return _tagf2; }
                set
                {
                    if (_tagf2 != value)
                    {
                        _tagf2 = value;
                        RaisePropertyChanged(nameof(TagF2));
                    }
                }
            }

            public TestScope()
            {
                _tagf1 = new FloatTag<TestScope>(this) { Text = v1.ToString() };
                _tagf2 = new FloatTag<TestScope>(this) { Text = v2.ToString() };
                _tagi1 = new FloatTag<TestScope>(this) { Text = v1.ToString(), Truncate = true };
                _tagi2 = new FloatTag<TestScope>(this) { Text = v2.ToString(), Truncate = true };
            }
        }

        [Fact]
        public void TestIntTagDiscovery()
        {
            string exText = "TagI1 + TagI2.FinalValue";

            var scope = new TestScope();

            var ce = new CompiledExpression<double>(exText);
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

            var ce = new CompiledExpression<double>(exAddText);
            var del = ce.ScopeCompile<TestScope>();
            var r = (double)del(scope);
            Assert.Equal(v1 + v2, r);

            ce = new CompiledExpression<double>(exSubText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 - v2, r);

            ce = new CompiledExpression<double>(exMulText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 * v2, r);

            ce = new CompiledExpression<double>(exDivText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 / v2, r);
        }

        [Fact]
        public void TestFloatTagMath()
        {
            string exAddText = "TagF1 + TagF2";
            string exSubText = "TagF1 - TagI2";
            string exMulText = "TagF1 * TagF2";
            string exDivText = "TagF1 / TagF2";

            var scope = new TestScope();

            var ce = new CompiledExpression<double>(exAddText);
            var del = ce.ScopeCompile<TestScope>();
            var r = del(scope);
            Assert.Equal(v1 + v2, r);

            ce = new CompiledExpression<double>(exSubText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 - v2, r);

            ce = new CompiledExpression<double>(exMulText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 * v2, r);

            ce = new CompiledExpression<double>(exDivText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 / v2, r);
        }

        [Fact]
        public void TestIntFloatTagMath()
        {
            string exAddText = "TagI1 + TagF2";
            string exSubText = "TagI1 - TagF2";
            string exMulText = "TagI1 * TagF2";
            string exDivText = "TagI1 / TagF2";

            var scope = new TestScope();

            var ce = new CompiledExpression<double>(exAddText);
            var del = ce.ScopeCompile<TestScope>();
            var r = del(scope);
            Assert.Equal(v1 + v2, r);

            ce = new CompiledExpression<double>(exSubText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 - v2, r);

            ce = new CompiledExpression<double>(exMulText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 * v2, r);

            ce = new CompiledExpression<double>(exDivText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 / v2, r);
        }

        [Fact]
        public void TestFloatIntTagMath()
        {
            string exAddText = "TagF1 + TagI2";
            string exSubText = "TagF1 - TagI2";
            string exMulText = "TagF1 * TagI2";
            string exDivText = "TagF1 / TagI2";

            var scope = new TestScope();

            var ce = new CompiledExpression<double>(exAddText);
            var del = ce.ScopeCompile<TestScope>();
            var r = del(scope);
            Assert.Equal(v1 + v2, r);

            ce = new CompiledExpression<double>(exSubText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 - v2, r);

            ce = new CompiledExpression<double>(exMulText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 * v2, r);

            ce = new CompiledExpression<double>(exDivText);
            del = ce.ScopeCompile<TestScope>();
            r = del(scope);
            Assert.Equal(v1 / v2, r);
        }
    }
}
