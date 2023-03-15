namespace KitchenChaos
{
    /// <summary>
    /// Interface used on objects able to be highlighted/unhighlighted.
    /// </summary>
    public interface IHighlightable
    {
        void Highlight();
        void UnHighlight();
    }
}