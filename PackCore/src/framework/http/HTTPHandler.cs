using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Wolftex.src.framework.context;
using Wolftex.src.framework.eventData;
using Wolftex.src.framework.verticle;

namespace Wolftex.src.framework.http
{
   public class HTTPHandler
    {
        public HTTPHandler(IWolftexContext context, int port) {
            this.context = context;
            this.prefix = "http://localhost:" + port.ToString() + "/";
            this.listener = new HttpListener();
            this.listener.Prefixes.Add(prefix);
            this.listener.Start();
            this.id = Guid.NewGuid();
            thread = new Thread(HandleRequest);
            thread.Start();
        }
        Guid id;
        Dictionary<HTTPUriContext, AbstractVerticle> endpoints = new Dictionary<HTTPUriContext, AbstractVerticle>();
        String prefix;
        HttpListener listener;
        IWolftexContext context;
        Thread thread;
        bool running = true;
        public void RegisterEndpoint( String uri, String verb, AbstractVerticle verticle ) {
            HTTPUriContext context = new HTTPUriContext(uri, verb);
            context.path = uri;
            context.verb = verb;
            endpoints.Add(context, verticle);
        }

        public void HandleRequest() {
            while (running) {

           
                HttpListenerContext ctx = listener.GetContext();
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse res = ctx.Response;
                Dictionary<string, string> headers = new Dictionary<string, string>();
                List<string> uriParamaters = new List<string>();
                string verb = req.HttpMethod;
                string uri = req.Url.ToString();
                String body = "";
                if (req.HasEntityBody) {
                    StreamReader bodyReader = new StreamReader(req.InputStream, req.ContentEncoding);
                    body = bodyReader.ReadToEnd();
                }


                HTTPRequest request = new HTTPRequest(headers, uri, body, verb);
                HTTPResponse response = new HTTPResponse(this);


               
                response.senderDelegate = () => {
                    try
                    {
                        res.StatusCode = response.statusCode;
                        byte[] data = Encoding.UTF8.GetBytes(String.Format(response.body));
                        res.ContentType = response.contentType;
                        res.OutputStream.Write(data);
                        res.Close();
                    }
                    catch (Exception e) {
                        System.Console.WriteLine(e.Message);
                    }
                };

                bool foundHandler = false;  
                foreach (HTTPUriContext context in endpoints.Keys)
                {
                    String[] splitPath = SplitRequestURI(request);
                    if (context.MatchesQuery(splitPath, request.verb))
                    {
                        List<string> urlWildCards = context.GetWildCardValues(splitPath);
                        request.wildCardValues = urlWildCards;
                        AbstractVerticle verticle = endpoints[context];
                        HttpEvent message = new HttpEvent(request, response, id.ToString(), verticle.getAddress()  );
                        this.context.EnqueEvent(message);
                        foundHandler = true;
                    }
                }
                if (!foundHandler) {
                    res.StatusCode = 404;
                    res.Close();
                }
            }
        }

        public static string[] SplitRequestURI(HTTPRequest request)
        {
            String urlParams = request.uri.Split("?").Length > 1 ? request.uri.Split("?")[1] : null;
            String[] queryData = request.uri.Split("?")[0].Split("//");
            String query = queryData[1];
            String requestPath = query.Substring(query.IndexOf("/"));
            String[] splitPath = requestPath.Substring(1).Split("/");
            return splitPath;
        }
    }
}
