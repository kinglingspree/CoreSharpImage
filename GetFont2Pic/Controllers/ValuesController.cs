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
namespace GetFont2Pic.Controllers
{
    [Route("api/v1")]
    public class ValuesController : Controller
    {
        private IOptions<AppSettings> _config;
        private IMemoryCache _cache;
        public ValuesController(IOptions<AppSettings>  config, IMemoryCache cache)
        {
            _config = config;
            _cache = cache;

        }
        // GET api/values
        public ContentResult Error(string message)
        {

            return Content(JsonConvert.SerializeObject(new ResultResponsecs { IsSuccess = false, ErrorMessage = message }));
        }

        // GET api/values/5
       // [HttpGet]
        [HttpPost]
        //[Route("/GetPic")]
        public  FileResult Get(GetParameters param)
        {
            try {
                param = new GetParameters {
                    DisplayText = "22",
                    BackgroundColor = new rgba { red = 0, green = 0, blue = 0, a = 9 },
                    FontColor = new rgba { red = 255, green = 255, blue = 255, a = 9 },
                    FontName = "StandBold",
                    Width = 200,
                    Height = 100
                };
                var seobj = JsonConvert.SerializeObject(param);
                FontFamily fontfamily = null;
                // var fontobj = new FontCollection();
                Rgba32 background = new Rgba32(param.BackgroundColor.red, param.BackgroundColor.green, param.BackgroundColor.blue, param.BackgroundColor.a / 10);
                Rgba32 fontColor = new Rgba32(param.FontColor.red, param.FontColor.green, param.FontColor.blue, param.FontColor.a / 10);
                if (_config.Value.FontPaths.Select(t => t.FontName).Contains(param.FontName))
                {
                    fontfamily = _cache.GetOrCreate(param.FontName, t =>
                    {
                        t.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                        return new FontCollection().Install(
                            
                         _config.Value.FontPaths.Where(g => g.FontName.Equals(param.FontName)).FirstOrDefault().FontUrl
                    //@"C:\Windows\Fonts\STKAITI.TTF"   //字体文件
                    );
                    });
                }
                else {
                    Response.Redirect("Error");
                }
                //var install_Family = new FontCollection().Install(
                //    "C:/work/GetFont2Pic/GetFont2Pic/wwwroot/font/11.ttf"
                //    //@"C:\Windows\Fonts\STKAITI.TTF"   //字体文件
                //    );
                var font = new Font(fontfamily, 50);  //字体
                MemoryStream mem = new MemoryStream();
                using (var img = new Image<Rgba32>(param.Width, param.Height))
                {
                    img.Mutate(x => x.Fill(background)
                       .DrawText(param.DisplayText, font, fontColor, new Vector2(0, 0)));
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

                return null;
            }
                //  FileContentResult files=new FileContentResult(mem.read,"")
                // resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                // return ;

            }
            //using (Image<Rg32> image = new Image<Rg32>(200, 200))
            //{
            //    image.Mutate(x => x.DrawText("你们好，我是神牛", font,
            //             new Rg32(),
            //           new SixLabors.Primitives.PointF())
            //         );

            //    image.SaveAsJpeg(mem);
            //    //从图片中读取byte
            //    //  byte[] buffer = new byte[200];
            //    //  var a = System.IO.File.ReadAllBytes("8-2.jpeg");
            //    //从图片中读取流
            //    //      var imgStream = new MemoryStream(a);

            //    var resp = new HttpResponseMessage(HttpStatusCode.OK)
            //    {
            //        Content = new StreamContent(mem)
            //        //或者
            //        //Content = new ByteArrayContent(imgByte)
            //    };
            //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            //    return resp;

            //}



        }

    private bool ParamCheck(GetParameters param) {

        return false;
    }

     
    }

