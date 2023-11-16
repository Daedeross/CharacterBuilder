namespace CharacterBuilder.Foundation
{
    /// <summary>
    /// For all objects which have a Name
    /// </summary>
    public interface INamedItem
    {
        /// <summary>
        /// The entity's name. Depending on the context, could be unique.
        /// </summary>
        string Name { get; }
    }
}
