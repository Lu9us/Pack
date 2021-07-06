using System;

namespace Wolftex
{
    // designing WolfTex
    // components: context, verticle, messages, router
    // context holds all verticles, provides services and message passing
    // router decides message direction
    // verticles recive messages and process data based on those messages
    // intergrate a http and json provider for message passing
    // verticles sould be able to send and receive http requests and responses
    // threads context 1 thread, each verticle 1 thread, router 1 thread
    //possibly a thread for the HTTP handler and message passing

   
}
