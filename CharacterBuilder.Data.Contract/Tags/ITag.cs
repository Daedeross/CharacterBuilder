namespace CharacterBuilder.Data.Contract
{
    using System.ComponentModel;

    public interface ITag: INotifyPropertyChanged
    {
        string Text { get; set; }

        bool IsValid { get; }
    }
}
