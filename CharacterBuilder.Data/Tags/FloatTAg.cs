namespace CharacterBuilder.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FloatTag<TScope> : ValueTag<double, TScope>, IFloatTag
        where TScope : INotifyPropertyChanged
    {
        public override double DefaultValue { get { return 0f; } }

        protected double bonusValue;
        public override double FinalValue
        {
            get
            {
                return Value + bonusValue;
            }
        }
        private bool _truncate;

        public bool Truncate
        {
            get { return _truncate; }
            set
            {
                if (value != _truncate)
                {
                    var final = FinalValue;
                    _truncate = value;
                    if (! Equals(FinalValue, final))
                    {
                        RaisePropertyChanged(nameof(FinalValue));
                    }
                }
            }
        }

        public override bool ApplyBonus(IBonusTag<double> bonus)
        {
            bonusValue += bonus.Value;
            return true;
        }

        public override void ClearBonus()
        {
            bonusValue = 0f;
        }

        public FloatTag(TScope scope)
        {
            this.scope = scope;
        }

        #region Opperator Overloads

        public static double operator +(FloatTag<TScope> t1, IFloatTag t2)
        {
            return t1.FinalValue + t2.FinalValue;
        }
        //public static double operator +(FloatTag<TScope> t1, IIntTag t2)
        //{
        //    return t1.FinalValue + t2.FinalValue;
        //}

        public static double operator -(FloatTag<TScope> t1, IFloatTag t2)
        {
            return t1.FinalValue - t2.FinalValue;
        }
        //public static double operator -(FloatTag<TScope> t1, IIntTag t2)
        //{
        //    return t1.FinalValue - t2.FinalValue;
        //}

        public static double operator *(FloatTag<TScope> t1, IFloatTag t2)
        {
            return t1.FinalValue * t2.FinalValue;
        }
        //public static double operator *(FloatTag<TScope> t1, IIntTag t2)
        //{
        //    return t1.FinalValue * t2.FinalValue;
        //}

        public static double operator /(FloatTag<TScope> t1, IFloatTag t2)
        {
            return t1.FinalValue / t2.FinalValue;
        }
        //public static double operator /(FloatTag<TScope> t1, IIntTag t2)
        //{
        //    return t1.FinalValue / t2.FinalValue;
        //}
        #endregion // Opperator Overloads
    }
}
