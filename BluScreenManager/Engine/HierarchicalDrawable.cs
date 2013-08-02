using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.Engine
{
    public abstract class HierarchicalDrawable : HierarchicalObject
    {
        /// <summary>
        /// Draws itself then all children. Call this once on your base object.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object used for drawing.</param>
        public void DrawAll(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch);
            foreach (HierarchicalObject obj in Children)
            {
                HierarchicalDrawable drawable = obj as HierarchicalDrawable;
                if (drawable != null)
                    drawable.DrawAll(spriteBatch);
            }
        }

        /// <summary>
        /// Draws itself. Override this to implement your drawing functionality for this object.
        /// You do not need to call this directly; it is called hierarchically by the base object via DrawAll().
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object used for drawing.</param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
