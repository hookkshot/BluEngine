using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
