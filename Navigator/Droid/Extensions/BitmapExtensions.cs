using Android.Graphics;

namespace Navigator.Droid.Extensions
{
    public static class BitmapExtensions
    {
        public static int[,] ToColorArray(this Bitmap bmap)
        {
            var result = new int[bmap.Width, bmap.Height];
            for (var x = 0; x < bmap.Width; x++)
            {
                for (var y = 0; y < bmap.Height; y++)
                {
                    result[x, y] = bmap.GetPixel(x, y);
                }
            }
            return result;
        }
    }
}