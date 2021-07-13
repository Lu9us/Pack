Pack is a simple event driven framework written for .net core
It uses the concept of Verticles that segregate system components and provides communcation via a shared context.
The framework includes an intergrated HTTP server that allows verticles to register for events based on URL paths and request types.

currently not implemented but planned: 
 multi-context enviroments with communcations over HTTP. This would allow the creation of clustered enviroments
 HTTP headers: I need to learn more about how the wrapped HTTP server represents them and write a translation for the wrapper.
 Custom Message types.
 Robust error checking. 
 TDD: This would be here but I forgot tests have to be part of a seprate projects in C#
 