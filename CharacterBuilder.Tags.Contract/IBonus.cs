using System;
using System.ComponentModel;

namespace CharacterBuilder.Tags.Contract
{
    public interface IBonus<TValue>: INotifyPropertyChanged
        where TValue : IEquatable<TValue>
    {
        TValue Value { get; }

        ITag Source { get; }
    }
}
