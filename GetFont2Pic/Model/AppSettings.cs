using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetFont2Pic.Model
{
    public class AppSettings
    {

        public List<FontPathMap> FontPaths { get; set; }
        public List<AppSecret> AppSecrets { get; set; }
    }
    public class AppSecret {

        public string AppID { get; set; }
        public string AppSalt { get; set; }
    }
    public class FontPathMap
    {
        public string FontName { get; set; }
        public string FontUrl { get; set; }
    }
}
