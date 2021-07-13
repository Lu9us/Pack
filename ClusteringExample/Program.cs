using Pack.src.framework.context;
using Pack.src.framework.http;
using Pack.src.framework.verticle;
using System;

namespace ClusteringExample
{
    class Program
    {
        class TestVert : AbstractVerticle
        {
            public override void ProcessHTTPRequest(HTTPRequest request, HTTPResponse response)
            {
                
            }

            public override void ReciveMessage(Message message)
            {
                Console.WriteLine("message: " + message.data + " recived from: " + message.senderId);
                if (message.data == "Hello there!") {
                    Message messageResponse = new Message();
                    messageResponse.data = "General Kenobi!";
                    SendMessage(messageResponse, message.senderId);

                }
            }

            public override void Start()
            {
            }

            public void sendMessageTo(string id) {
                Message message = new Message();
                message.data = "Hello there!";
                SendMessage(message, id);

            }
        }
        static void Main(string[] args)
        {
            TestVert vert = new TestVert();
            PackContext contextA = new PackContext(2);
            contextA.RegisterVerticle(vert, "vertA");
            contextA.createHTTPHandler(80);
            contextA.EnableClustering();
            PackContext contextB = new PackContext(2);
            contextB.RegisterVerticle(new TestVert(), "vertB");
            contextB.createHTTPHandler(81);
            contextB.EnableClustering();

            contextA.ConnectToContext("http://localhost:81");
            vert.sendMessageTo("vertB");

        }
    }
}
