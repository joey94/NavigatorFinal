using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigator.Helpers
{
    public interface IGetPixel
    {
        int GetPixel(int x, int y);
    }


    public class WallCollision
    {
        private bool[,] isWall;
        private List<int> wallColors = new List<int>()
        {
            Color.FromArgb(255,255,255,255).ToArgb(),
            Color.FromArgb(0,0,0,0).ToArgb()
        };

        Func<int,int,int> pixelMethod;

        public WallCollision(Func<int,int,int> getPixel)
        {
            pixelMethod = getPixel;
        }

        public bool IsValidStep(int p1x, int p1y, int p2x, int p2y)
        {
            var x0 = p1x;
            var y0 = p1y;
            var x1 = p2x;
            var y1 = p2y;
            int x,
            cx,
            deltax,
            xstep,
            y,
            cy,
            deltay,
            ystep,
            error;
            bool st;
            st = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (st)
            {
                x0 ^= y0;
                y0 ^= x0;
                x0 ^= y0; // swap(x0, y0);

                x1 ^= y1;
                y1 ^= x1;
                x1 ^= y1; // swap(x1, y1);
            }
            deltax = Math.Abs(x1 - x0);

            deltay = Math.Abs(y1 - y0);

            error = deltax/2;

            y = y0;


            if (x0 > x1)
            {
                xstep = -1;
            }

            else
            {
                xstep = 1;
            }


            if (y0 > y1)
            {
                ystep = -1;
            }

            else
            {
                ystep = 1;
            }


            for (x = x0; x != x1 + xstep; x += xstep)

            {
                cx = x;
                cy = y;


                if (st)

                {
                    cx ^= cy;
                    cy ^= cx;
                    cx ^= cy;
                }

                if (wallColors.Contains(pixelMethod(cx, cy)))
                {
                    return false;
                }


                error -= deltay; // converge toward end of line

                if (error < 0)

                {
                    // not done yet

                    y += ystep;

                    error += deltax;
                }
            }
            return true;
        }

        enum CheckDirection
        {
            Up,
            Down,
            Right,
            Left,
            None
        }
    }
}