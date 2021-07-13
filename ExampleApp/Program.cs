using System;
using System.Collections.Generic;
using Pack.src.framework.context;
using Pack.src.framework.http;
using Pack.src.framework.verticle;

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
            response.body = "{{\"HelloJSON\":\"this is a test\"}}";
            response.contentType = "application/json";
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
          
            WolftexContext context = new WolftexContext(8);
            HTTPHandler handler = new HTTPHandler(context, 8080);
            Vert vert = new Vert();
            context.RegisterVerticle(vert);
            handler.RegisterEndpoint("/test", "GET", vert);
        }
    }
}
