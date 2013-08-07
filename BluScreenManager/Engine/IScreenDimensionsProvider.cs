
namespace BluEngine.Engine
{
    public interface IScreenDimensionsProvider
    {
        float ScreenX { get; }
        float ScreenY { get; }
        float ScreenWidth { get; }
        float ScreenHeight { get; }
        float ScreenRatio { get; }
    }
}
