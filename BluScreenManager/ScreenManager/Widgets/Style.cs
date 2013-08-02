using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BluEngine.Engine;

namespace BluEngine.ScreenManager.Widgets
{
    public class Style : HierarchicalObject
    {
        new public Style Parent
        {
            get { return base.Parent == null ? null : base.Parent as Style; }
            set { base.Parent = value; }
        }

        private Texture2D fill = null;
        public Texture2D Fill
        {
            get { return fill == null ? (Parent == null ? null : Parent.Fill) : fill; }
            set { fill = value; }
        }

        public Style(Style parent) : base(parent) {}
        public Style() : base() { }
    }
}
