namespace CharacterBuilder.Data
{
    public interface IModifier: ITrait
    {
        ITrait Parent { get; }


    }
}
