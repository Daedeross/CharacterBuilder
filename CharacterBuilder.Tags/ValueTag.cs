namespace CharacterBuilder.Tags
{
    using System;
    using System.ComponentModel;
    using CharacterBuilder.Tags.Contract;
    using CharacterBuilder.Foundation;
    using CharacterBuilder.Utilities;
    using System.Collections.Generic;
    using ExpressionEvaluator;
    using System.Linq.Expressions;

    public abstract class ValueTag<TValue, TScope> : NotificationObject, IValueTag<TValue>
        where TValue : IEquatable<TValue>
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

        public abstract bool ApplyBonus(IBonus<TValue> bonus);

        public abstract void ClearBonus();

        protected bool CompileExpression(string expr)
        {
            try
            {
                CompiledExpression<TValue> ce = new CompiledExpression<TValue>(expr);
                var del = ce.ScopeCompile<TScope>();
                var ex = ce.Expression as Expression<Func<TScope, TValue>>;
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
    }
}
