using AurelienRibon.TweenEngine;
using BluEngine.ScreenManager.Screens;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// A general subclass of TweenCallback from which to subclass your own custom WidgetScreen-related callback handlers.
    /// </summary>
    /// <typeparam name="T">The Type of your WidgetScreen.</typeparam>
    public abstract class WidgetScreenTransitionCallback<T> : TweenCallback
        where T : WidgetScreen
    {
        protected T screen;

        /// <summary>
        /// Create a new instance of WidgetScreenTransitionCallback.
        /// </summary>
        /// <param name="screen">The screen this belongs to.</param>
        public WidgetScreenTransitionCallback(T screen)
        {
            this.screen = screen;
        }
    }

    /// <summary>
    /// A specialized subclass of WidgetScreenTransitionCallback that will take a paramaterless void function as a parameter, which will be executed when the callback is fired.
    /// </summary>
    /// <typeparam name="T">The Type of your WidgetScreen.</typeparam>
    public sealed class WidgetScreenTransitionEventCallback<T> : WidgetScreenTransitionCallback<T>
        where T : WidgetScreen
    {
        public delegate void GenericEventHandler();
        private event GenericEventHandler onFinishedEvent;

        /// <summary>
        /// Create a new instance of WidgetScreenTransitionEventCallback.
        /// </summary>
        /// <param name="screen">The screen this belongs to.</param>
        /// <param name="finishedEvent">The function to call when the callback is fired.</param>
        public WidgetScreenTransitionEventCallback(T screen, GenericEventHandler finishedEvent)
            : base(screen)
        {
            onFinishedEvent += finishedEvent;
        }

        public override void onEvent(int type, BaseTween source)
        {
            if (onFinishedEvent != null)
                onFinishedEvent();
        }
    }
}
