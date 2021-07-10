using System;
using System.Collections.Generic;
using Wolftex.src.framework.context;
using Wolftex.src.framework.http;
using Wolftex.src.framework.verticle;

namespace ExampleApp
{

    public class Vert : AbstractVerticle
    {
        public override void Start()
        {
            System.Console.WriteLine("New verticle created");
        }

        public override void ProcessHTTPRequest(HTTPRequest request, HTTPResponse response)
        {
            System.Console.WriteLine("Recivied http message from handler");
            response.body = "This is a happy response";
            response.contentType = "text/html";
            response.statusCode = 200;
            response.end();


        }

        public override void ReciveMessage(Message message)
        {
            System.Console.WriteLine("recived message");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
          
            WolftexContext context = new WolftexContext(2);
            HTTPHandler handler = new HTTPHandler(context, 8080);
            Vert vert = new Vert();
            context.RegisterVerticle(vert);
            handler.RegisterEndpoint("/test", "GET", vert);
        }
    }
}
