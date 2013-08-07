
namespace BluEngine.ScreenManager
{
    public interface IInvalidatable
    {
        /// <summary>
        /// If true, this object is in need of refreshing.
        /// </summary>
        bool Invalidated { get; }

        /// <summary>
        /// Flags this object as in need of refreshing before next redraw.
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Refreshes this object.
        /// </summary>
        void Refresh();
    }
}
