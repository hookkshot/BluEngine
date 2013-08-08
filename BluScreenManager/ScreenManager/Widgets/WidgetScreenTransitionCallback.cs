using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AurelienRibon.TweenEngine;
using BluEngine.ScreenManager.Screens;

namespace BluEngine.ScreenManager.Widgets
{
    
    
    public abstract class WidgetScreenTransitionCallback<T> : TweenCallback
        where T : WidgetScreen
    {
        protected T screen;
        public WidgetScreenTransitionCallback(T screen)
        {
            this.screen = screen;
        }
    }

    public class WidgetScreenTransitionEventCallback<T> : WidgetScreenTransitionCallback<T>
        where T : WidgetScreen
    {
        public delegate void GenericEventHandler();
        private event GenericEventHandler onFinishedEvent;

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
