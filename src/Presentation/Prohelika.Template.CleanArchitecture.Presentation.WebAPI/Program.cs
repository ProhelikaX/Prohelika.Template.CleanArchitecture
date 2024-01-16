using Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigurePipeline();
app.Run();