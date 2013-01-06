using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security;
using System.Text;
using System.Web;
using System.Web.Routing;
using NSubstitute;

namespace Glass.Mapper.Umb.Integration
{
    /// <summary>
    /// Creates a mock http context with supporting other contexts to test against
    /// </summary>
    public class FakeHttpContextFactory
    {

        [SecuritySafeCritical]
        public FakeHttpContextFactory(Uri fullUrl)
        {
            CreateContext(fullUrl);
        }

        [SecuritySafeCritical]
        public FakeHttpContextFactory(string path)
        {
            if (path.StartsWith("http://") || path.StartsWith("https://"))
                CreateContext(new Uri(path));
            else
                CreateContext(new Uri("http://mysite" + VirtualPathUtility.ToAbsolute(path, "/")));
        }

        [SecuritySafeCritical]
        public FakeHttpContextFactory(string path, RouteData routeData)
        {
            if (path.StartsWith("http://") || path.StartsWith("https://"))
                CreateContext(new Uri(path), routeData);
            else
                CreateContext(new Uri("http://mysite" + VirtualPathUtility.ToAbsolute(path, "/")), routeData);
        }

        public HttpContextBase HttpContext { get; private set; }
        public RequestContext RequestContext { get; private set; }

        /// <summary>
        /// Mocks the http context to test against
        /// </summary>
        /// <param name="fullUrl"></param>
        /// <param name="routeData"></param>
        /// <returns></returns>
        private void CreateContext(Uri fullUrl, RouteData routeData = null)
        {
            //Request context

            RequestContext = Substitute.For<RequestContext>();

            //Request

            var request = Substitute.For<HttpRequestBase>();
            request.AppRelativeCurrentExecutionFilePath.Returns("~" + fullUrl.AbsolutePath);
            request.PathInfo.Returns(string.Empty);
            request.RawUrl.Returns(VirtualPathUtility.ToAbsolute("~" + fullUrl.AbsolutePath, "/"));
            request.RequestContext.Returns(RequestContext);
            request.Url.Returns(fullUrl);
            request.ApplicationPath.Returns("/");
            request.Cookies.Returns(new HttpCookieCollection());
            request.ServerVariables.Returns(new NameValueCollection());
            var queryStrings = HttpUtility.ParseQueryString(fullUrl.Query);
            request.QueryString.Returns(queryStrings);
            request.Form.Returns(new NameValueCollection());

            //Cache
            var cache = Substitute.For<HttpCachePolicyBase>();

            //Response 
            //var response = new FakeHttpResponse();
            var response = Substitute.For<HttpResponseBase>();
            response.ApplyAppPathModifier(null).ReturnsForAnyArgs(info => info.Arg<string>());
            response.Cache.Returns(cache);

            //Server

            var server = Substitute.For<HttpServerUtilityBase>();
            server.MapPath(Arg.Any<string>()).Returns(Environment.CurrentDirectory);

            //HTTP Context

            HttpContext = Substitute.For<HttpContextBase>();
            HttpContext.Cache.Returns(HttpRuntime.Cache);
            HttpContext.Items.Returns(new Dictionary<object, object>());
            HttpContext.Request.Returns(request);
            HttpContext.Server.Returns(server);
            HttpContext.Response.Returns(response);

            RequestContext.HttpContext.Returns(HttpContext);

            if (routeData != null)
            {
                RequestContext.RouteData.Returns(routeData);
            }
        }

    }
}
