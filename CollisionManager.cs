using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeTheSquares
{
    public static class CollisionManager
    {
        // Method to check if two rectangles are colliding
        public static bool IsColliding(Rectangle rect1, Rectangle rect2)
        {
            return rect1.Intersects(rect2);  // Checks if the bounding boxes intersect
        }

    }

}
