using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PathPlan.HelperClasses
{
    public class FloatRectangle
    {
        public Vector2 position;
        /// <summary>
        /// X is the Width,
        /// Y is the Height
        /// </summary>
        public Vector2 size;

        public float Left
        {
            get
            {
                return this.position.X;
            }
        }
        public float Top
        {
            get
            {
                return this.position.Y;
            }
        }
        public float Right
        {
            get
            {
                return this.position.X + this.size.X;
            }
        }
        public float Bottom
        {
            get
            {
                return this.position.Y+this.size.Y;
            }
        }

        public FloatRectangle(float x, float y, float width, float height)
        {
            position = new Vector2(x, y);
            size = new Vector2(width, height);
        }

        public FloatRectangle(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }
    }
}
