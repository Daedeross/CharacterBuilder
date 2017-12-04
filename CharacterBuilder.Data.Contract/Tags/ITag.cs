namespace CharacterBuilder.Data
{
    using System.ComponentModel;

    public interface ITag: INotifyPropertyChanged
    {
        int Id { get; }

        string Text { get; set; }

        bool IsValid { get; }
    }
}
