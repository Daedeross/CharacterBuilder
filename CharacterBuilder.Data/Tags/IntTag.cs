namespace CharacterBuilder.Data
{
    using CharacterBuilder.Data.Contract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IntTag<TScope> : ValueTag<int, TScope>, IIntTag
        where TScope : INotifyPropertyChanged
    {
        public override int DefaultValue { get { return 0; } }

        protected int bonusValue;
        public override int FinalValue
        {
            get
            {
                return Value + bonusValue;
            }
        }

        public override bool ApplyBonus(IBonusTag<int> bonus)
        {
            bonusValue += bonus.Value;
            return true;
        }

        public override void ClearBonus()
        {
            bonusValue = 0;
        }

        public IntTag(TScope scope)
        {
            this.scope = scope;
        }

        public IntTag(TScope scope, string text)
            : this(scope)
        {
            this.Text = text;
        }
    }
}
