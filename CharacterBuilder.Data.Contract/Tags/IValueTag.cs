namespace CharacterBuilder.Data.Contract
{
    using System;

    public interface IValueTag<TValue>: ITag
        where TValue : IEquatable<TValue>
    {
        /// <summary>
        /// The primary Value the tag holds.
        /// It shall be deterermined by the <see cref="ITag.Text"/> property.
        /// Implementing class shall notify changes via PropertyChanged event.
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// The value of the tag after all bonuses.
        /// </summary>
        TValue FinalValue { get; }

        /// <summary>
        /// The default value to assume if Text does not parse.
        /// Implementing classes 
        /// </summary>
        TValue DefaultValue { get; }

        /// <summary>
        /// Is true if the Tag is in the process of refreshing its Value
        /// </summary>
        bool IsRefreshing { get; }

        /// <summary>
        /// 
        /// </summary>
        void ClearBonus();

        bool ApplyBonus(IBonusTag<TValue> bonus);
    }
}
