﻿using Newtonsoft.Json;
using Pack.src.framework.context;
using Pack.src.framework.eventData;
using Pack.src.framework.http;
using Pack.src.framework.verticle;
using PackCore.src.framework.clustering;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PackCore.src.framework.clustering
{
    public class ClusteringVerticle : AbstractVerticle
    {
        public readonly string REGISTER_ENDPOINT = "/registerContext";
        public readonly string UPDATE_ENDPOINT = "/updateContext";
        public readonly string MESSAGE_ENDPOINT = "/onMessage";
        public List<string> registeredWith = new List<string>();
        private int port;
        public ClusteringVerticle(int port) {
            this.port = port;
            httpClient = new HttpClient();
        }

       
        ConcurrentDictionary<string, List<string>> addressMap = new ConcurrentDictionary<string, List<string>>();
        HttpClient httpClient;
        public override void ProcessHTTPRequest(HTTPRequest request, HTTPResponse response)
        {
            if (request.splitUrl[request.splitUrl.Length - 1] == REGISTER_ENDPOINT.Substring(1))
            {
                RegisterRequestBody body = JsonConvert.DeserializeObject<RegisterRequestBody>(request.body);
                if (!addressMap.ContainsKey(body.address))
                {
                    addressMap.TryAdd(body.address, body.verticles);
                    response.statusCode = 200;
                    response.body = "";
                    response.end();
                    if (!registeredWith.Contains(body.address.ToLower()))
                    {
                        SendRegistration(body.address);
                    }
                }
                
            }
            else if (request.splitUrl[request.splitUrl.Length - 1] == UPDATE_ENDPOINT.Substring(1))
            {
                RegisterRequestBody body = JsonConvert.DeserializeObject<RegisterRequestBody>(request.body);
                if (addressMap.ContainsKey(body.address)) {
                    List<string> value = new List<string>();
                    addressMap.TryRemove(body.address, out value);
                    addressMap.TryAdd(body.address, body.verticles);
                    response.statusCode = 200;
                    response.end();
                }
            }
            else if (request.splitUrl[request.splitUrl.Length - 1] == MESSAGE_ENDPOINT.Substring(1))
            {
                MessageEvent messageEvent = JsonConvert.DeserializeObject<MessageEvent>(request.body);
                context.EnqueEvent(messageEvent);
                response.statusCode = 200;
                response.end();
            }

        }

        public bool HasRemoteVerticle(string name) {
            return addressMap.Any(value => value.Value.Contains(name));
        }

        public void SendRegistration( string targetAddress) {
            String baseAddress = "http://localHost:" + port;
            List<string> verts = context.getVerticles();
            RegisterRequestBody body = new RegisterRequestBody(baseAddress, verts);
            HttpContent content = new StringContent(JsonConvert.SerializeObject(body));
            registeredWith.Add(targetAddress);
            Task data = httpClient.PostAsync(targetAddress + REGISTER_ENDPOINT, content);
            while (!data.IsCompleted) {
            }
            if (!data.IsCompletedSuccessfully) {
                registeredWith.Remove(targetAddress);
            }
            System.Console.WriteLine("Sent Registration");
        }

        public void SendUpdate() {
            List<string> verts = context.getVerticles();
            String baseAddress = "http://localHost:" + port;
            RegisterRequestBody body = new RegisterRequestBody(baseAddress, verts);
            HttpContent content = new StringContent(JsonConvert.SerializeObject(body));
  
            foreach (string key in addressMap.Keys)
            {   
                Task data = httpClient.PostAsync(key + UPDATE_ENDPOINT, content);
            }
        }
        public void SendMessage(MessageEvent messageEvent) {
            string address = addressMap.First(value => value.Value.Contains(messageEvent.target)).Key;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(messageEvent));
            Task data = httpClient.PostAsync(address + MESSAGE_ENDPOINT, content);


        }

        public override void ReciveMessage(Message message)
        {
            
        }

        public override void Start()
        {
            httpClient = new HttpClient();
        }
    }
}
