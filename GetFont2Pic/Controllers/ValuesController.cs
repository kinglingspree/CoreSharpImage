using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using System.Numerics;
using System.Text;
using System.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.ImageSharp.Processing.Text;
using SixLabors.Fonts;
using System.IO;
using SixLabors.ImageSharp.Processing;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using SixLabors.Shapes;
using SixLabors.ImageSharp.Formats;
using Microsoft.Extensions.Options;
using GetFont2Pic.Model;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;


namespace GetFont2Pic.Controllers
{
    [Route("api/v1")]
    public class ValuesController : Controller
    {
        private IOptions<AppSettings> _config;
        private IMemoryCache _cache;
       // private ILogger _logger;
        public ValuesController(IOptions<AppSettings>  config, IMemoryCache cache)
        {
            _config = config;
            _cache = cache;
            //_logger = logger;

        }
     
        // GET api/values/5
       // [HttpGet]
        [HttpPost]
        [Route("GetPic")]
        public  FileResult MakePic([FromBody]Parameters param)
        {
          
            var path = "http://"+ Request.Host;
           
            var md5value = SignUrlAndRequestParams(path,GetParameterss(param));
            if (!param.Sign.Equals(md5value))
            {
                
                HttpContext.Response.Headers.Add("ErrorMessage","sign not match");
                return File("/Error.html", "text/html");
            }


            try {
                
               
                FontFamily fontfamily = null;
               // Rgba32 fontColor = new Rgba32(param.FontColor.red, param.FontColor.green, param.FontColor.blue);
               // Rgba32 background =  Rgba32.Gold;
              //  Rgba32 fontColor = new Rgba32(param.FontColor.red, param.FontColor.green, param.FontColor.blue);
                //Rgba32 background = rg;
                //Rgba32 fontColor = new SixLabors.ImageSharp.PixelFormats.Rgba32(255, 255, 255, 255);
                if (_config.Value.FontPaths.Select(t => t.FontName).Contains(param.FontName))
                {
                    StringBuilder url = new StringBuilder();
                    fontfamily = _cache.GetOrCreate(param.FontName, t =>
                    {
                        t.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                        return new FontCollection().Install(
                         url.Append(Directory.GetCurrentDirectory()).Append("/").Append(
                         _config.Value.FontPaths.Where(g => g.FontName.Equals(param.FontName)).FirstOrDefault().FontUrl).ToString()
                   
                    );
                    });
                }
                else {
                    HttpContext.Response.Headers.Add("ErrorMessage", "not find this font");
                    return File("/Error.html", "text/html");
                }
                //var install_Family = new FontCollection().Install(
                //    "C:/work/GetFont2Pic/GetFont2Pic/wwwroot/font/11.ttf"
                //    //@"C:\Windows\Fonts\STKAITI.TTF"   //字体文件
                //    );
                var font = new Font(fontfamily, 50);  //字体
                MemoryStream mem = new MemoryStream();
                
                using (var img = new Image<Rgba32>(param.Width, param.Height))
                {
                    img.Mutate(x => x.Fill(Rgba32.White)
                       .DrawText(param.DisplayText, font, Rgba32.White, new Vector2(0, 0)));
                    img.SaveAsJpeg(mem);
                    mem.Position = 0;


                    var resp = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(mem)
                        //或者
                        //  Content = new ByteArrayContent(mem)
                    };

                    return File(mem.ToArray(), "image/jpg");
                }
            }
            catch (Exception e)
            {
                HttpContext.Response.Headers.Add("ErrorMessage", "exception occurd");
                //_logger.LogError(e.ToString());
                return File("/Error.html", "text/html");
            }
               

            }
   




       public  string SignUrlAndRequestParams(string path, IDictionary<string, string> parameters)
        {
            var sign = new StringBuilder();
            sign.Append(path);
            var paramString = new StringBuilder();
            parameters = parameters.OrderBy(o => o.Key).ToDictionary(p => p.Key, o => o.Value);
            foreach (var item in parameters.Where(item => !item.Key.ToLower().StartsWith("sign")))
            {
                paramString.Append(item.Key).Append("=").Append(item.Value).Append("&");
            }
            if (paramString.Length > 0)
            {
                paramString.Length -= 1;
                sign.Append("?").Append(paramString);
            }
            else
            {
                sign.Append("?");
            }
           
            return GetMD5(sign.ToString());
        }




        private IDictionary<string, string> GetParameterss(Parameters obj)
        {
            var properties = obj.GetType().GetProperties();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            
            foreach (var propertity in properties)
            {
                var itemValue = propertity.GetValue(obj);
                if (itemValue != null)
                {
                    parameters.Add(propertity.Name, itemValue.ToString());
                }
            }
            var app = _config.Value.AppSecrets.Where(t => t.AppID.Equals(obj.AppID)).FirstOrDefault();
           // parameters.Add("AppID",app.AppID.ToString());
            parameters.Add("AppSalt", app.AppSalt.ToString());
            return parameters;
        }

        public static string GetMD5(string myString)
        {
            MD5 md5 = MD5.Create();
            var fromData = Encoding.UTF8.GetBytes(myString);
            var targetData = md5.ComputeHash(fromData);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < targetData.Length; i++)
            {
                result.Append(targetData[i].ToString("x2"));
            }
            return result.ToString();
        }
    }
}


   

