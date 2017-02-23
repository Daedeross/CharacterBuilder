namespace CharacterBuilder.Data
{
    using System;
    using System.ComponentModel;
    using CharacterBuilder.Data.Contract;
    using CharacterBuilder.Foundation;
    using CharacterBuilder.Utilities;
    using System.Collections.Generic;
    using ExpressionEvaluator;
    using System.Linq.Expressions;

    public abstract class ValueTag<TValue, TScope> : DynamicDataModel, IValueTag<TValue>
        where TValue : IEquatable<TValue>
        where TScope : INotifyPropertyChanged
    {
        protected TScope scope;
        protected string currenntText;
        protected string lastValidText;
        protected CompiledExpression<TValue> valueExpression;
        protected Func<TScope, TValue> valueDelegate;
        protected TValue cachedValue;

        protected bool isValid;
        protected bool pending;

        public abstract TValue DefaultValue { get; }

        public bool IsRefreshing { get; }

        public bool IsValid { get; }

        public string Text
        {
            get { return currenntText; }
            set
            {
                if (CompileExpression(value))
                {
                    lastValidText = value;
                }
            }
        }

        public TValue Value
        {
            get
            {
                if (pending)
                {
                    try
                    {
                        TValue val = valueDelegate(scope);
                        cachedValue = val;
                    }
                    catch
                    {
                        cachedValue = DefaultValue;
                    }
                }
                return cachedValue;
            }
        }

        public abstract TValue FinalValue { get; }

        public abstract bool ApplyBonus(IBonusTag<TValue> bonus);

        public abstract void ClearBonus();

        protected bool CompileExpression(string expr)
        {
            try
            {
                CompiledExpression<TValue> ce = new CompiledExpression<TValue>(expr);
                var del = ce.ScopeCompile<TScope>();
                //var ex = ce.Expression as Expression<Func<TScope, TValue>>;
                var ex = ce.Expression;
                var set = ExpressionUtilities.FindMembers(ex, scope);
                var val = del(scope);
                var oldVal = cachedValue;

                // If we get to this point, everything worked so we can assign the vars
                valueDelegate = del;
                valueExpression = ce;
                cachedValue = val;
                pending = false;
                SetWatchedSubscriptions(set);
                if (!oldVal.Equals(cachedValue))
                {
                    RaisePropertyChanged(nameof(Value));
                }
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }
        }

        #region Watching

        protected Dictionary<INotifyPropertyChanged, HashSet<string>> watched;

        private void OnWatchedChanged(object sender, PropertyChangedEventArgs e)
        {
            INotifyPropertyChanged obj = sender as INotifyPropertyChanged;
            if (watched.TryGetValue(obj, out HashSet<string> set))
            {
                if (set.Contains(e.PropertyName))
                {
                    pending = true;
                    var oldVal = cachedValue;
                    if (!oldVal.Equals(Value))
                    {
                        RaisePropertyChanged(nameof(Value));
                    }
                }
            }
        }

        private void SetWatchedSubscriptions(Dictionary<INotifyPropertyChanged, HashSet<string>> newWatched)
        {
            if (watched != null)
            {
                foreach (var kvp in watched)
                {
                    kvp.Key.PropertyChanged -= OnWatchedChanged;
                }
            }
            watched = newWatched;
            foreach (var kvp in watched)
            {
                kvp.Key.PropertyChanged += OnWatchedChanged;
            }
        }

        #endregion
        #region Implicit Conversion Methods

        public static implicit operator bool(ValueTag<TValue, TScope> t)
        {
            bool result;
            try
            {
                result = Convert.ToBoolean(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = false;
            }
            return result;
        }

        public static implicit operator short(ValueTag<TValue, TScope> t)
        {
            short result;
            try
            {
                result = Convert.ToInt16(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        public static implicit operator int(ValueTag<TValue, TScope> t)
        {
            int result;
            try
            {
                result = Convert.ToInt32(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        public static implicit operator long(ValueTag<TValue, TScope> t)
        {
            long result;
            try
            {
                result = Convert.ToInt64(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        public static implicit operator float(ValueTag<TValue, TScope> t)
        {
            float result;
            try
            {
                result = Convert.ToSingle(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        public static implicit operator double(ValueTag<TValue, TScope> t)
        {
            double result;
            try
            {
                result = Convert.ToDouble(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        public static implicit operator decimal(ValueTag<TValue, TScope> t)
        {
            decimal result;
            try
            {
                result = Convert.ToDecimal(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }
        #endregion
    }
}
