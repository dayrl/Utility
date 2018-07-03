using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace Zdd.Utility
{
    public interface IFilterInformation
    {
        Dictionary<PixelFormat, PixelFormat> FormatTranslations { get; }
    }
}
