Apuntes tutorial y novedades sobre .net 6

-----------------------------------------------------------------------------------------------------------------------------

Este codigo esta totalmente basado en los ejercicios y en la explicacion dada por felipe gavilasn en su curos de udemy 
Construyendo Web APIs con ASP.NET Core 6. 
[https://www.udemy.com/share/101qLg3@2sHbteVaczfi3P4D1VCDJh9fhVCdSWgGBP5AA5NYF5sva48H9sJjNEjn12EYKUO8/]

Iniciamos con una carpeta que va a ser la contenedora del codigo y sus explicaciones, dentro como primera carpeta 
encontraremos unas ayudasendocker, un compose para levantar una base de datos local en sql server y un dockerfile 
con el que podremos arrancar el proyecto con docker.

Para iniciar con .Net necesitamos primero tener configurado el ID con el que vamos a trabajar, segundo tener la version 
de .Net con el que vamos a trabajar, si trabajamos desde visual studio es muy probable que en la instalacion ya se tenga
el sdk de net core instalado, de no se asi primero validamos que version tenemos ejecutando el siguiente comando.

*** dotnet --version

de no tener una version instalada esta se puede descargar desde la pagina [dot.net], si ya lo tenemos instalado podremos
validar las posibles plantillas a implementar cone el siguiente comando.

*** dotnet new --list

si ya tenemos clara la plantilla que queremos implementar entonces lo que nos queda es crear el proyecto usando el nombre
de la plantilla y el nombre que le queremos dar al proyecto.

*** dotnet new webapi -o [nombre]

si lo ejecutaste desde una consola puedes dirigirte a visual studi code con el comando [code .], una vez se eset ubicado
el lo que sera el folder raiz de este tutorial y proyecto podrmos corrlo con el siguiente comando.

*** dotnet run

esta ejecucion desplegara la aplicacion pero si nosotros quisieramos hacer una depuracion sobre la aplicacion y valernos
de puntos de interrupcion y demas herramientas de depuracion, partiendo de que estamostrabajando desde visual code, vamos
a la seccion de [run an debugg] en la parte lateral izquierda de visual code.

ya por ultimo creamos un archivo .gitignore para no subir cosas innecesarias a nuestro github

-----------------------------------------------------------------------------------------------------------------------------

junto a la nueva version de .net con encontramos con la desaparicin de la clase startup, chora trae una nueva "reorganizacion"
la cual agrupa todo en la clase program, en lo personal no me gusta, se ve muy desordenado, para ello podemos crear una clase
Startup.cs para pasar alli los servicios y los midleware por defecto, seguimos los siguientes pasos.

- primero creamos la clase Startup.cs

- creamos el constructor de la clase y este por defecto va a recibir un parametro de tipo [Iconfiguration] el cual encapsulamos en 
como propiedad.

- vamos a crear dos metodos de tipo void, el primero sera el [ConfigurationServices] el cual recibira un parametro  de tipo
[IServiceCollection], y un segundo metodo llamado [Configure] el cual recibira dos parametros, uno de tipo [IApplicationBuilder] 
y otro de Tipo [IWebHostEnviroment]

- todos los servicios que se encuentran en el prgram creados por el builder los pasamos a ConfigurationServices y los 
invocamos desde la propiedad services del metodo [ConfigurationServices]

- todos los middleware inyectados por app en program son pasados a la clase configure, a excepcion del app.run; 
implementamos el [app.UseRouting] que ubicamos despues de [app.UseHttpRedirection] y remplazamos a [app.MapControllers] por
[useEndpoint] que colocamos al final.

- una vez tenemos todos los servicios y los middleware en el startup nos queda solo instanciar e inicializar los metodos desde la 
clase program

una configuracion adicional que podemos ejecutar para notener quepreocuparnos por los tipos de referencias nulas lo hacemos desde
el [.csproj] cambiando la propiedad [Nulleable] a disable

-----------------------------------------------------------------------------------------------------------------------------

para empezar a entrar en materia lo primero de lo que partimos es de un controlador y una entidad, este controlador se va a encargar
de administrar o de controlar las acciones que puedan realizar con una entidad, acciones crud, y que es una entidad, es una
representacion de una tabla en una base de datos

para crear un controlador se debe tener encuenta que, el nombre del controlador debe ir acompañado de la palabra Controller
[ClaseController], debe implementar la clase [ControllerBase] y debe tener los decoradores [ControllerApi] y del decorador [Route()] que me
ayudara a ubicar el endpoint con mayor facilidad

si ya tenemos un controlador y una entidad ahora lo que necesitamos es generar la relacion a una base de datos, para ello vamos
a usar el ORM de entity framework, para ello mediante el administrador de paquetes nugget de visual studio o mediante el cli de
.Net vamos a instalas las siguientes librerias.

antes de las librerias la instalacion por medio de linea d e comandos se hace por medio de algun administrador de paquetes en
visual code o por medio de los siguientes comandos.

*** nuget install [paquete]
*** dotnet add package [paquete]

los paquetes a instalar serian:

*** Microsoft.EntityFrameworkCore.sqlServer
*** Microsoft.EntityFrameworkCore.desing

cuando los instalas desde visual studio no es necesario isntalar el desing, el primer paquete lo instala por añadidura, una
vez tengamos instalados los paquetes necesarios podemos crear nuestro dbcontext el cual es quien nos ayudara a establecer la 
coneccion con nuestra base de datos para ello creamos una clase preferiblemente en la carpeta entidades y que herede de la clase
[DbContext] de entity framework

en el dbContext vamos a recibir diferentes configuracines de nuestro entity framewor por medio de inyeccion de dependencias pero
para ello lo primero con lo que debemos contar es con una cadena de conexion a una base de datos en nuestro proveedor de configuraciones
appsetings

si ya tenemos un controlador, una entidad, un dbcontext con un dbset a una tabla y una configuracion en nuestro proveedor de 
configuraciones, lo que nos queda es inyectar esa configuracion en nuestro startup, cuando ya lo tengamos inyectado podremos
generar migraciones, las cuales van a crear todo lo que nosotros codifiquemos en una base de datos, para crear una migracion 
pdemos hacer uso de los siguientes comandos.

*** dotnet tool install --global dotnet-ef // se instala una unica vez
*** Add-Migration Inicial 
*** dotnet-ef migrations add Inicial

cuando se genera la migracion tenemos que ejecutarla para que se haga efectiva, para ello podemos ejecutar  los siguientes.

*** Update Database 
*** dotnet-ef database update

cabe anotar que no hay necesidad ni de crer la base de datos, con migration la query se encarga de crear todo el esquema 
de bases de datos, tablas, llaves foraneas, primaris etc etc si ya crea mos la tabla ahora podemos hacer consultas 
directamente a la tabla usando linq, para lograr esto primero debemos inyectar el dbcontext que inicializamos 
en el startup a cada uno de los controladores que hagan una consulta a una tabla de bases de datos y ejecutamos cada una de
las acciones crud

*** [T1AutoresController] es el controlador con el resumen de esta parte del tutorial

ahora vamos a crear una relacion uno a muchos en donde vamos a decir que
un autor puede corresponder con varios libros, primero como dato debemos tener encuenta que todas las entidades que yo 
quiera representar en mi modelo de bases de datos deben estar en el DBSet del contexto.

vamos a la clase 1 osea a la clase que va a tener mucho de algo, en este caso un autor puede tener muchos libros entonces 
le agregamos a la clase autor una propiedad de navegacion donde decimos que va a tener un listado de libros.

luego vamos a la clase muchos osea a la clase que va a depender de autor, en este caso la clase libros debe tener una 
propiedad que indique que pertenece a un autor y a su vez una variable de navegacion que apunte a la entidad

luego generamos una migracion para actualizar el modelo de datos y poder empezar a trabajar con data realcionada de forma 1 .. *

al haber creado el DBSet para cada entidad esto me brinda la flexibilidad de poder trabajar con cada una de las tabls por
separado y no dependiendo de ninguna otra como podria ser el caso en el que no pusieramos libro y para acceder a el 
obligatoriamente tendriamos que hacerlo por medio del Autor

ya desde cada uno de los controladores independientes podremos acceder a la data relacionada por medio de linq e include

por ultimo debemos realizar una configuracion para que la data no se vuelba ciclica y esto nos genere errores de relacion
en services.addControllers() agregamos la siguiente configuracion

*** .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

-----------------------------------------------------------------------------------------------------------------------------











