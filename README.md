# Simple chat
Another one example of text messages exchange service. Based on the next libs list:
1. ASP.NET Core 2.0;
1. Yeoman generator;
1. SignalR 1.0.0-preview1-final for ASP.NET Core 2.0;
1. Angular 4.1.2;
1. Typescript 2.7.2;
1. Swagger 2.2.0 with Autorest API client codegen.
## How to
To implement SignalR usage on ASP.NET Core app you should:
1. Create ASP.NET Core project:
```
dotnet new angular
```
2. Download NPM packages:
```
npm install
```
3. Add NuGet references to the next libs:
    1. `Microsoft.AspNetCore.All`;
    1. `Microsoft.AspNetCore.SignalR`.
4. Implement nested from `Hub` or `Hub<T>` class:
```csharp
using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

using SimpleChat.Models;

namespace SimpleChat.Hubs
{
	public sealed class ChatHub: Hub<IChatHub>
	{
		public async Task Send(ChatMessageModel chatMessage)
		{
			if (chatMessage == null) throw new ArgumentNullException(nameof(chatMessage));

			await Clients.All.SendAsync(chatMessage);
		}
	}
}
```
5. Add support for the SignalR in ```Startup.cs```. Enable SignalR for our application:
```csharp
services.AddSignalR();
```
Create route mapping for our SignalR's chat Hub implementation:
    
```csharp
app.UseSignalR(routes =>
	// Note: chat hub name should start with '/'
	routes.MapHub<ChatHub>("/chathub")
);
```
*Note:*: hub name should start with '/'.
6. Install the latest version of the ASP.NET Core SignalR NPM package:
```
npm install @aspnet/signalr
```
7. Now we can create, setup and start SignalR's HubConnection in a Typescript component:
```typescript
this.chatHub = new HubConnection(originUrl + "/chathub");

this.chatHub.on(
  "Send",
  data => this.receiveMessage(data));

this.chatHub
  .start()
  .catch(error => console.log(error));
```
8. Profit! That is all. Now we can start our chat:
```
dotnet run
```
## Authentication
Note: Windows authentification is used by default.
## Swagger
1. Install Swagger NuGet package:
```
Install-package Swashbuckle.AspNetCore
```
2. Initalize Swagger UI module in your Startup.cs ```Configure()``` method:
```csharp
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API V1");
});
```
3. Enable XML documentation generation in your project settings;
4. Specify Swagger generation settings in ```Startup.cs``` class ```ConfigureServices()``` method:
```csharp
services.AddSwaggerGen(c =>
{
	c.SwaggerDoc(
		"v1",
		new Info
		{
			Title = "SimpleChat API",
			Version = "v1"
		});

	var basePath = PlatformServices.Default.Application.ApplicationBasePath;
	var xmlPath = Path.Combine(basePath, "SimpleChat.xml");
	c.IncludeXmlComments(xmlPath);
});
```
5. OpenAPI SimpleChat API definition can be found:
```
https://localhost:44395/swagger/
```
## Useful links
* Yeoman's home page on GitHub: https://github.com/OmniSharp/generator-aspnet
	* Angular 2 SPA: https://blogs.msdn.microsoft.com/webdev/2017/02/14/building-single-page-applications-on-asp-net-core-with-javascriptservices/
* SignalR home page on GitHub: https://github.com/aspnet/SignalR
* SignalR for ASP.NET Core 2.0: https://blogs.msdn.microsoft.com/webdev/2017/09/14/announcing-signalr-for-asp-net-core-2-0/
* SignalR with Windows authentication: http://msharonov.blogspot.com/2017/10/signalr-windows-authentication.html
* Swagger: https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?tabs=visual-studio
