namespace CharacterBuilder.Data
{
    public interface IModifierScope
    {
        ICharacter Character { get; }
        ITrait Parent { get; }
        IModifier Owner { get; }
    }
}
