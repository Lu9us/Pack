Pack is a simple event driven framework written for .net core, designed to support quick prototyping of backend http systems with relatively good performance 
It uses the concept of Verticles that segregate system components and provides communcation via a shared context.
Event handling is multi-threaded and the number of workers can be scaled on startup.
The framework includes an intergrated HTTP server that allows verticles to register for events based on URL paths and request types.

As Pack is currently in its prototype stages please do not deploy it to any public facing environments or critical infrastructure.

currently not implemented but planned: 
	automatic cluster propergation
	 HTTP headers: I need to learn more about how the wrapped HTTP server represents them and write a translation for the wrapper.
	 Custom Message types.
	 Robust error checking. 
	 TDD: This would be here but I forgot tests have to be part of a seprate projects in C#
	 filebased configuration
 