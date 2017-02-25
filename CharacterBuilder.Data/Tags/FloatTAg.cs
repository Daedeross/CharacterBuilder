namespace CharacterBuilder.Data
{
    using CharacterBuilder.Data.Contract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FloatTag<TScope> : ValueTag<float, TScope>, IFloatTag
        where TScope : INotifyPropertyChanged
    {
        public override float DefaultValue { get { return 0f; } }

        protected float bonusValue;
        public override float FinalValue
        {
            get
            {
                return Value + bonusValue;
            }
        }

        public override bool ApplyBonus(IBonusTag<float> bonus)
        {
            bonusValue += bonus.Value;
            return true;
        }

        public override void ClearBonus()
        {
            bonusValue = 0f;
        }
    }
}
