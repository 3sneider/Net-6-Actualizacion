using WebApi;

var builder = WebApplication.CreateBuilder(args);

// instanciamos a startup pasandole el Iconfiguration como parametro
var startup = new Startup(builder.Configuration);

// invocamos el ConfigurationServices de starup y el parametro que solicita lo pasamos del builder
startup.ConfigurationServices(builder.Services);


var app = builder.Build();

// invocamos el configure y de igual manera le pasamos los parametros de el app ya construida
startup.Configure(app, app.Environment);

app.Run();
