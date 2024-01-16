using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Prohelika.Template.CleanArchitecture.Presentation.WebCSR;
using Prohelika.Template.CleanArchitecture.Presentation.WebCSR.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.ConfigureServices(builder.Configuration);

await builder.Build().RunAsync();