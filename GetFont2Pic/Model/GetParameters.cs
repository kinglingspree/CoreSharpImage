using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetFont2Pic.Model
{
    public class GetParameters
    {
        public string TimeStamp { get; set; }
        public string DisplayText { get; set; }
        public rgba BackgroundColor { get; set; }
        public string FontName { get; set; }
        public rgba FontColor { get; set; }
        public int Width { get; set; }
        public int Height { get;  set; }
     
    }
    public class  rgba
    {
        public int red { get; set; }
        public int green { get; set; }

        public int blue { get; set; }
        public int a { get; set; }
    }
}
