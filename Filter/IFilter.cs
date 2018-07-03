using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Zdd.Utility
{
    public interface IFilter
    {
        Bitmap Apply(Bitmap image);
        Bitmap Apply(BitmapData imageData);
    }
}
