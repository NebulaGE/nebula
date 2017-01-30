using Nebula.Web.VideoHelper;
using System; 
using System.IO; 
using System.Net;
using System.Net.Http; 
using System.Web;
using System.Web.Http; 

namespace Nebula.Web.Controllers
{ 
    public class VideoStreamController : ApiController
    { 
        public HttpResponseMessage Get(string key, string ext)
        { 
            var video = new VideoStream(HttpContext.Current.Session[key].ToString(), ext);
            HttpContext.Current.Session.Remove(key);
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)video.WriteToStream);
           // response.Content = new PushStreamContent(video.WriteToStream, new MediaTypeHeaderValue("video/" + ext));

            return response;
        } 
    }
}
