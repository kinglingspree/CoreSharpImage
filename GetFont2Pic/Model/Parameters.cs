using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GetFont2Pic.Model
{
    public class Parameters
    {
        [Required]
        public string TimeStamp { get; set; }
        [Required]
        public string AppID { get; set; }
        [Required]
        public string DisplayText { get; set; }
        
        public rgba BackgroundColor { get; set; }
        [Required]
        public string FontName { get; set; }
     
        public rgba FontColor { get; set; }
        [Required]
        public int Width { get; set; }
        [Required]
        public int Height { get;  set; }
        [Required]
        public string Sign { get; set; }
     
    }
    public class  rgba
    {
        public Byte red { get; set; }
        public Byte green { get; set; }

        public Byte blue { get; set; }
        public Byte a { get; set; }
    }
}
