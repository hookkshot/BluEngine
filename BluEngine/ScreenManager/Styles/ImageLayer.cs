﻿using System;
using BluEngine.ScreenManager.Widgets;
using Marzersoft.CSS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Marzersoft.CSS.Interpreters;

namespace BluEngine.ScreenManager.Styles
{
    /// <summary>
    /// An image layer within a widget style. Use these to build up multi-layer widgets.
    /// </summary>
    public class ImageLayer : Named, IProperty, IValue<ImageLayer>
    {
        public virtual Texture2D Texture
        {
            get { return texture; }
        }
        private Texture2D texture = null;

        private Vector4 bounds = new Vector4(0.0f,0.0f,1.0f,1.0f); //percentages of the parent control (W = Width, Z = Height);

        /// <summary>
        /// The percentage Left of this layer.
        /// </summary>
        public virtual float Left
        {
            get { return bounds.X; }
            set { bounds.X = value; }
        }

        /// <summary>
        /// The percentage Top of this layer.
        /// </summary>
        public virtual float Top
        {
            get { return bounds.Y; }
            set { bounds.Y = value; }
        }

        /// <summary>
        /// The percentage Width of this layer.
        /// </summary>
        public virtual float Width
        {
            get { return bounds.W; }
            set { bounds.W = Math.Max(value, 0.0f); }
        }

        /// <summary>
        /// The percentage Height of this layer.
        /// </summary>
        public virtual float Height
        {
            get { return bounds.Z; }
            set { bounds.Z = Math.Max(value, 0.0f); }
        }

            /// <summary>
        /// The percentage Right of this layer.
        /// </summary>
        public virtual float Right
        {
            get { return bounds.X + bounds.W; }
            set { float diff = value - bounds.W; bounds.W += diff; bounds.X -= diff; }
        }

        /// <summary>
        /// The percentage Bottom of this layer.
        /// </summary>
        public virtual float Bottom
        {
            get { return bounds.Y + bounds.Z; }
            set { float diff = value - bounds.Z; bounds.Z += diff; bounds.Y -= diff; }
        }

        /// <summary>
        /// This fill layer's bounds in percentages, relative to the widget (W = Width, Z = Height).
        /// </summary>
        public virtual Vector4 Bounds
        {
            get { return bounds; }
            set
            {
                bounds.X = value.X;
                bounds.Y = value.Y;
                bounds.W = Math.Max(value.W, 0.0f);
                bounds.Z = Math.Max(value.Z, 0.0f);
            }
        }

        public ImageLayer(String name, Texture2D tex) : base(name)
        {
            this.texture = tex;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Widget widget, Color col)
        {
            if (texture == null || col.A == 0)
                return;

            spriteBatch.Draw(
                texture,
                new Rectangle(
                (int)((float)widget.CalculatedBoundsI.X * bounds.W),
                (int)((float)widget.CalculatedBoundsI.Y * bounds.Z),
                (int)((float)widget.CalculatedBoundsI.Width * bounds.W),
                (int)((float)widget.CalculatedBoundsI.Height * bounds.Z)
                ),
                col);
        }
        
        public static ImageLayer FromColor(String name, Color color)
        {
            return new ImageLayer(name, SolidColours.TexFromColor(color));
        }

        /// <summary>
        /// The integer Type of the value when accessed through the ICSSProperty interface. For easier casting.
        /// </summary>
        public int PropertyType
        {
            get { return StyleSheet.TYPE_IMAGELAYER; }
        }

        /// <summary>
        /// Outputs the CSS-style value of this ImageLayer. Meaningless if this was not created from CSS.
        /// </summary>
        public override String ToString()
        {
            return base.ToString() + "[ImageLayer]";
        }

        public ImageLayer Value
        {
            get { return this; }
        }
    }
}
