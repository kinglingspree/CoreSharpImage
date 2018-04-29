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

namespace GetFont2Pic.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
   

        // GET api/values/5
        [HttpGet]
        public FileResult Get(Rgba32 backColor,string cheFont,string enFont,string ImgTp)
        {

            var install_Family = new FontCollection().Install(
                "C:/work/GetFont2Pic/GetFont2Pic/wwwroot/font/11.ttf"
                //@"C:\Windows\Fonts\STKAITI.TTF"   //字体文件
                );
            var font = new Font(install_Family, 50);  //字体
            MemoryStream mem = new MemoryStream();
            using (var img = new Image<Rgba32>(100, 200))
            {
                img.Mutate(x => x.Fill(Rgba32.DarkBlue)
                   .DrawText("我速度", font, Rgba32.Black, new Vector2(10, 0)));
                img.SaveAsJpeg(mem);
                mem.Position = 0;

                
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                   Content = new StreamContent(mem)
                    //或者
                  //  Content = new ByteArrayContent(mem)
                };

                return File(mem.ToArray(), "image/jpg");
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

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
