Apuntes tutorial y novedades sobre .net 6

-----------------------------------------------------------------------------------------------------------------------------

Este codigo esta totalmente basado en los ejercicios y en la explicacion dada por felipe gavilan en su curso de udemy 
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

para trabajar todo el tema sobre controladores , como lo es reglas de ruteo, respuestas de las peticiones nos valimos de el 
controlador [T1AutoresController]

-----------------------------------------------------------------------------------------------------------------------------

Model binding es una opcion que nos permite mapear los datos de una peticion http a los parametros de un metodo o accion
pueden ser los siguientes, cada uno de estos se puede marcar en los parametros del metodo ejemplo
funcion([FromQuery] string parametro)

[FromRoute] - para obtenerlos de la ruta / [HttpGet("{id:int}")]
[FromBody] - para obtenerlos del cuerpo del mensaje / es lo que recibimos en los parametros de unmetodo
[FromHeader] - para obtenerlos de la cabecera de la peticion // podemos recibir los datos de los header, como el contentype
[FromQuery] - para obtenerlos de la ruta pero por query ?var=dato&var2=dato2 / cuando los parametros vienen desd la url
[FromServices] - 
[FromForm] - para obtenerlos desde un formulario

-----------------------------------------------------------------------------------------------------------------------------

para trabajar todo el tema sobre validaciones personalizadas, validaciones por modelo y validaciones por dataannotation nos 
valimos de la entidad [testEntity] la cual no es mapeada a la base de datos al no tener un dataset en el dbContext

-----------------------------------------------------------------------------------------------------------------------------

inyectar dependencias lo podemos apreciar en la interfaz [IServicio] donde se explica y se resume como se inyecta una 
dependencia en una clase, pero existe un concepto de mas alto nivel llamado el sistema de inyeccion de dependencias, para 
configurar el sistema de inyeccion de dependencia debemos primero ubicarnos en la clase [startup] y desder el metodo 
[configureServices] donde podemos configurar los servicios

los servicios son los que resuelben un adependencia en el sistema de inyeccion de dependecias, con el sistema de inyeccion
de dependencias centralizamos las dependencias para no tener que instanciarlas cada vez que instanciamos una clase que tiene
dependencias, existen tres tipos de servicios

AddTransient - un servicio transistorio quiere decir que cuando se requiera una instancia siempre se va a dar una nueva 
instancia - para funciones siempre te va a dar una neueva instancia

AddScoped - el tiempo de vida del servicio aumenta, ahora en ves de ser una instancia distinta va a ser dentro del mismo 
contexto la misma instancia - aplicationdbcontext por ejemplo, es la misma instancia en la misma peticion http
siempre te va a dar la misma instancia en la misma peticion http

AddSingleton - con este siempre tenemos la misma instancia incluso en diferentes instancias del servicio - como capa de cache 
con data de memoria para servir la misma data a todos los usuarios siempre te va a dar la misma instancia 
inclusio en diferentes epticiones http

un ejemplo claro y facil es la inyeccion de dbContext en el sistema de inyeccion de dependencias, como lo pueden notar en el 
[startup] yo inyecto el dbcontext en los servicios de la aplicacion, esto automaticamente me habilita para que yo lo pueda
recibir como parametro en el constructor de la clase que lo necesite, como en nuestros controladores, en el startup se inicializa 
y ahi ya queda listo para poder ser usado

en el controlador [T1LibrosController] vamos a ejemplificar la inyeccion de un adependencia en el sistema de inyeccion de 
dependencias, como lo explicamos en la clase [Iservice] nosotros podemos inyectar un dato abstracto o uno concreto, por buenas
practicas y para mayor flexibilidad vamos a inyectar una interfaz(dato abstracto), una vez ya inyectado en la clase a implementar
en el constructor, vamos ahora a inyectarlo en el sitema de inyeccion de dependencias en el [startup]

los loggers podemos colocar los emnsajes en bases de datos o controlarlos de alguna forma, para implementarlos es necesario 
inyectar a la clase el servicio [ILogger<T>] donde T hace referencia a la misma clase donde se eesta inyectando, para ejemplificarlo
de mejor manera podemos verlo en accion en la clase [servicioA] de [IServicio]

existen 6 categorias de logers, estos se miden de mayor a menor nivel de severidad, segun el nivel que 
elijamos el va a mostrar ese y los de menor severidad a el.
Critical
Error
Warning
Information
debug
Trace

estos niveles pueden ser administrados desde el appsettings, en donde en loglevel le decimos desde que nivel queremos que nos 
administre los logs (), desde que nivel y para que mnamespace queremos aplicar un log

-----------------------------------------------------------------------------------------------------------------------------

Middleware :
una tuberia es una cadema de procesos conetados de tal forma que la salida de cada elemento de la cadena es la entrada del proximo, 
cada uno de estos procesos se llama Middleware
los middleware se pueden crear de dos formas, con codigo hardcodeado o podmos aislar su logica a una clase independiente

los middleware son los que configuramos en nuestra clase startup en el metodo configure, es muy importante el orden ya que como 
lo comentamos la salida de uno va a ser la entrada de el otro, estos se configuran mediante [IApplicationBuilder] que viene de la 
clase program, desde el startup hardcodeado podemos crear un servicio con app.run() y dentro una funcion de flecha ahora, tambien podemos
saltarnos la linealidad de una libreria anidandola a un map, osea a una ruta, lo podriamos ver en el startup.



vamos a crear un middleware y a insetarlo en el startup para ello creamos la carpeta de middlewares y trabajamos desde 
[LoguearRespuestaHTTPMiddleware.cs] y lo implementamos igual en el startup.

-----------------------------------------------------------------------------------------------------------------------------

Filtros:
los filtros nos ayudan a correr codigo en determinados momentos del ciclo de vida del procesamiento de una peticion HTTP

los filtros son utiles cuando tenemos la necesidad de ejecutar una logica en varias acciones de varios controladores y queremos
tener que evitar tener que repetir codigo.

filtros de autorizacion : si un usuario puede hacer una accion
filtros de recursos : validaciones generales o una capa de cache
filtros de accion : antes o despues de eejecutar una accion, para manipula los parametros de entrada o salida
filtros de excepcion : captura excepciones no cacheados
filtros de resultados ; despues de ejecutar un controlador

pueden ejecutarse en tres niveles diferentes

a nivel deaccion
a nivel del controlador
a nivel global

un ejemplo de filtros de recursos es un filtro de cache que ya viene predefinido por le framework y que solo es necesario implementarlos
para ello debemos primero que todo inyectar los servicios de cache en configureServices, este seria [services.AddResponseCaching();]
luego implementamos le filtro [app.UseResponseCaching();] en un punto donde ya se halla retornado la informacion de los endpoints.

si ya tenemos el servicio y el filtro implementados ahora vamos a invocarlos, estos se invocan como sifueran DataAnnotations en los 
metodos, para verlo enaccion vamos a implementarlo en el metodo Get de [T1LibrosController]

el filtro de authorize tambien lo podemos usar como el de cache, para ello perimero inyectamos el servicio 
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();] y el filtro  [app.UseAuthorization();] y el 
dataAnnotation seria [Authorize], pero esto es muy generico, mas adelante veremos un ejemplo mas completo.

ya entendido comom funcionan cache y athorize podremos crear tambien filtros personalizados y filtrosd globales
los filtros globales son clases que se programan mediante el uso de una interfaz que corresponde a alguno de los
tippos de filtros mensionados, se inyectan en el sistema de inyeccion de dependencias y ya pueden ser usados como decorador
en una accion o a nivel global

para la creacion de un filtro personalizado y para tenerlos de forma ordenada los vamos a crear en la carpeta [Filtros], siempre que 
creemos un filtro este debe implementar la interfaz [IActionFilter] o de cualquier otra implementacion de la libreria
[Microsoft.AspNetCore.Mvc.Filters] segun el tipo de filtro que queramos inplementar, lo podemos ver ejemplificado en nuestro 
filtro [MiFiltroDeAccion] o en [FiltroDeExcepcion]

con el filtro de excecion que implementamos podemos cachear todas aquellas esxcepciones que precisamente no estan controladores
para verlo es accion basta con forzar una exce con trwo

una vez tengamos creados nuestros filtros lo unico que debemos hacer como indique antes es inyectarlo en el 
sistema de inyeccion de dependencia y ya lo podremos usar coomo lo hicimos con el filtro de cache o authorize, la estructura del dataannotation
seria [ServiceFilter(typeof(MiFiltroDeAccion))], lo vamos a usar de forma generica para el metodo post en [T1LibrosController]

la unica diferencia es que ssi lo queremos generico se lo agregamos al metodo y si lo queremos global tenemos que agregarlo en la configuracion
del servicio de controles en el ConfigureService en el sevicio AddControllers, lo agregamos como una funcion de flecha y quedaria algo asi

services.AddControllers(opciones => { opciones.Filters.Add(typeof(FiltroDeExcepcion))}).....

por ultimo para cerrar el tema de filtros tenemos en ocaciones la necesidad de ejecutar tareass recurrentes, esto se puede
lograr de varias maneras, una es un hostedServics, el que se ejecutara al inicio y al final del tiempo de vida de neustro webapi.

esto la vamos a ejemplificar en lo que a su ves se considera un servicio y por ende lo organizamos en la carpeta Servicios, 
el archivo se llamara [EscribirEnArchivo]

-----------------------------------------------------------------------------------------------------------------------------

como manipular recursos con entity framework, a profundidad
entity framework es un orm que te permite a partir dfe clases de c# interactura con una tabla de bases de datos, recordemos que 
para trabajar con cualquier base de datos podemos buscarlo desde el administrador de paquetes nugget e instalarlo, como usualmente
trabajamos es con mssql server mostramos ese paquete, pero para los demas motores de bases de datos es exactamente igual.

*** Microsoft.EntityFrameworkCore.sqlServer
*** Microsoft.EntityFrameworkCore.desing

el primero es el paquete de sql server para entity framework y el segundo es el desing que nos ayuda con librerias que hacen la 
implementacion mas sencilla.

una vez instalados los paquetes necesarios debemos crear nuestro DbContext que es quien nos va a generar esa coneccion entre 
nuestras entidades/clases con las tablas de la base de datos, usualmente se sugiere ordenarlo dentro de la carpeta entities ya 
que ahi tambien vamos a tener las clases a referenciar

esta clase se puede llamar como guste, solo se recomienda que le agregues al final la palabra DbContext para que sea mas sencillo
de ubicar, tambien esta clase debe heredar la clase DbContext ya que es la clase central de todo entity framework.

en este ejemplo usamos el esquema codigo primero, donde apartir de una codificacion bamos a crear unas tablas para ello cada 
entidad que queramos establecer como una tabla la debemos agregar como una propiedad de tipo DbSet<T> donde T es la entidad, el 
nombre que le asignemos a esta propiedad sera como se llamara la tabla en la base de datos, cabe anotar que todas las validaciones
que agreguemos a las propiedades de nuestra entidad seran las mismas que se pasaran a la tabla, como el max lengt, require etc.

cuando se tiene creado el dbContext lo siguiente es establecer la cadena de conneccion en nuestro proveedor de configuraciones,
esto lo que nos dira es en donde vamos a crear esa base de datos y por ende las tablas y todo su contenido

por ultimo para tener una configuracion basica es inyectar nuestro dbcontext en el sistema de inyeccion de dependencias para no 
tener que declararlo en cada sitio donde se requiera si no que simplemente se inyecte en el constructor de cada clase que lo implemente

ya teniendo la configuracion lo que procede es ejecutar migraciones para trasladar la logica del dbContext a una base de datos.
para ello uamos los siguientes comandos.

*** crear la migracion
*** dotnet tool install --global dotnet-ef // se instala una unica vez
*** Add-Migration Inicial 

*** ejecutar la migracion
*** dotnet-ef migrations add Inicial
*** Update Database 
*** dotnet-ef database update

ya realizada la migracion podemos empezar a ejecutar acciones crud sobre nuestroas entidades y nuestros controladores, para ejemplificar
vamos a hacer uso de [AutoresController] y [LibrosController] puesto que son los que tenemos limpios ya que los controladores
T los usamos solo para tomar nota y aplicar ejemplos de los anteriores conceptos.

para remasterizar lo desarrollado tengamos encuenta que no es optimo ni seguro exponer nuestras entidades, para ello nos valemos de un 
recurso llamado DTO's y de automapper lo cual nos ayuda a maperar nuestras entidades a dtos ecxponiendo solo la data que nos interece mostrar, 
para lograr esto una vez creadas nuestras entidades y nuestros dtos hacemos uso de la libreria [AutoMapper.Extensions.Microsoft.DependencyInjection] 
e inyectamos el sevicio en el sistema de inyeccion de dependencias en el startup por medio del comando [services.AddAutoMapper(typeof(Startup));]

Entoncs, tenemos que podemos crear dto de nuestras entidades para no afectar a las entidades que mapean a la base de datps, o pata no exponer
data que pueda sedr sencible, en fin, la finalidad depende de la necesidad, paar ejmplificar creamos DTOs de nuestras entidades que reponden
a cada una de las necesidades del Crud.

una ves tengamos nuestro dto vamos a necesitar mapear el dto a la entidad, para ello nos apoyamos en la libreria de automapper
que instalamos anteriormente  y que inyectamos en el sistema de inteccion de pependencias, ahora necesitamos configurar los mapeos
para ello creamos una clase [AutomMapperProfiles.cs] como una utilidad os segun el orden que se le este dando, en esta clase se 
configuraran todos los mapeos de entidades a Dtos y visebersa.

relaciones

para generar una relacion uno a muchos debemos tener dos propiedades de navegacion en cada una de las clases a relacionar, 
como por ejemplo en libros y comentarios, un libro puede tener multiples comentarios pero un comentario solo puede pertenecer 
a un unico libro, para ello en la entidad dependiente en este caso comentarios agregamos una propiedad id que haga referencia 
al libro que pertenece junto a una porpiedad que haga referencia al objeto que queremos anidar, en este caso un libro; 
agregamos una propiedad de navegacion a libros donde especificamos una lista de comentarios, en el momento de generar una 
migracion con este tipo de referencia en nuestras entidades, el modelo se encargara de generar las llaves foraneas;

DSebemos evaluar muy bien nuestros dtos en caso de que no queramos devolver informacion innecesarioa a nuestros usuarios finales, por 
ejemplo los comentarios de un libro, tenemos un controlador que se encarga solamente de los ocmentarios de esta manera le 
retornamos al uaurio la informacion del libro y luego si quiere los comentarios.

para generar una relacion muchos a muchos decimos que un autor puede haber escrito muchos libros y un libro puede seer escrito 
por muchos autores, para esta relacion mechos a muchos la modelamos en una clase intermediaria, AutoresLibros.cs con los id de 
ambas tablas relacionadas y propiedades de navegacionn a ambas entidades, luego en cada una de las entidades relacionadas vamos 
a tener tambien propiedades de navegacion que referencien a la clase intemrediaria, por ultimo agregamos el dbset en el dbcontext

cuando hacemos una inserccion con una entidad que tiene una relacion muchos a muchos debemos tener cual es la entidad de mas 
relevancia para lo que solicitamos en este caso como lo que queremos crear es un libro y desde ahi trabajar los autores vamos 
a agregfar als propiedades que queraramos jalar de los autores en libros en el dto

se ha trabajado las acciones crud de manera mas explicita en los controladores liblros, comentarios y autores, ahora vamos a 
hacer HTTP patcj solo para actualizaciones parcioales a un recurso para ello nos guiamos con la estructura que define el estandar 
Jsonpatch

para poder hacer uso del json path necesitamos tener la siguiente libreira instalada

*** Microsoft.AspNetCore.Mvc.NewtonsoftJson

-----------------------------------------------------------------------------------------------------------------------------

las configuraciones son datos externos de nuestra aplicacion que ayudan a nuestra aplicacion a funcionar correctamente, estos
datos tienden a variar entre ambientes, como por ejemplo la cadena de coneccion que tiene informacion de la base de datos, esta
no es info que querramos tener en nuestro codigo fuente, es mejor tener estas y otras informaciones en funentes externas, para
comunicarnos con esas fuentes externas usamos proveedores de configuracion, el framework de .net nos ofrece el iconfiguration 
el ccual nos ofrece esa coneccion con la confiiguracion.

si nos paramos en el appsetings y nos ubicamos el json,  y agragamos una nueva propiedad a dicho json por medio de nuestro 
proveedor de configuraciones podremos acceder a esa propiedad como por ejemplo agregar una propiedad ["nombre": "esneider"], 
por medio del iconfiguration desde cualquier parte de nuestro codigo lo podemos accesar de la siguiente maneras

en nuestro controlador de comentarios vamos a tener un endpoint llamado configuraciones donde vamos a ejempliificar como vamos
a traer la data de nuestro proveedor de configuracions appsettings

existen diferentes proveedores de configuracion, el mas comun es el appsettings y el appsettingsDevelopment; para poder seleccionar
con que ambiente queremos trabajar lo podemos seleccionar desde ñas propiedades del proyecto, en vs code lo podemos hacer cambiando
las variables de ambiente, accedemos a esas propiedades desde la carpeta properties en el archivo json que ahi se hospeda en 
la propiedad [ASPNETCORE_ENVIRONMENT].

una variable de ambiente es un valor que uno puede acceder desde un ambiente especifico como proveedor de configuraciones, estas 
se pueden agregar al mismo nivel donde encontramos la variable de nombre [ASPNETCORE_ENVIRONMENT]

estas variables de ambiente tambien se pueden crear en azure o en aws o en cualquier proveedor de nube que me lo permita.

otro dato que debemos tener en cuenta a la hora de declarar nuestras variables de ambiente es el orden, si uno tiene mas de un
proveedor de configuraciones el valor siempre del ultimo valor configurado, si configuras primero uno en el lounch y luego una
en el appsetings, este ultimo sera el que va a tener encuenta primero,  esto por el orden en que se compila el codigo de asp.net
y lo podemos ver en el codigo fuente en la clase program.

otra forma muy popular para tener estas variables aisladas es el uso de secretos, estos son variables declaradas en un archivo
de nombre secrets.json, este archivo no se encuentra en el proyecto, este genera un id unico que serializa en la configuracion
de la aplicacion, esta configurado para trabajar unicamente en el ambiente de desarrollo (equipo) donde se crea el secrets.json.
para crear el user secret usamos el comando

*** dotnet user-secrets init // en visual studio normal es click derecho user manage secrets

y agregamos secretos con el comando

*** dotnet user-secrets set "Movies:ServiceApiKey" "12345"

y para ver los secretos usamos el comando

*** dotnet user-secrets list

si queremos eliminar los secretos ejecurtamos

*** dotnet user-secrets clear

si queremos eliminar un secreto en especifico ejecutamos

*** dotnet user-secrets remove "Movies:ConnectionString"

un ultimo proveedor de configuracion es la terminal de linea de comandos por donde posdremos pasarle parametros a nuestra 
aplicacion, segun el orden este estara  por ensima de cualquier otro proveedor de configuracions, desde una terminal que 
apunte a la carpeta delproyectocuando ejecutemos la aplicacion podemos arrancar nuestra aplicacion con parametros ejemplo
[ dotnet run -- "apellido=apellido desde linea de comandos"]

-----------------------------------------------------------------------------------------------------------------------------

authenticacion y autorizathion

la autenticacion es la validacion por credenciales y la autorizacin son las cosas que el usuario puede hacer, los tipos de 
autenticacion son 

anonimo = cualquier persona
basic = encryp en base 64 con ssl
bearer = es basado en tokens

para configurar el sitema de identity y tener unas tablas base para la autenticacion lo hacemos de la siguiente forma
desde el explorador de soluciones instalamos el paquete [Microsoft.AspNetCore.Identity.EntityFrameworkCoree] esto ya que 
estamos usando esntityframework

ya instalado el paquete nos diriguimos a nuestra repositorio dbcontext y en vez de heredar de dbcontext vamos a heredar de la 
clase [IdentityDbContext], cuando generemos una migracion este va a crear las clases necesarias para nuestra autenticacion;

ya teniendo las tablas a configurar desde el startup vamos a inyectar los servicios necesario para poder implementar identity
un avez inyectado en el startup podemos decorar un metodo o un controlador con el modificado autorize, esto para que cuando 
intentemos ingrezar si no estamos authorizados no nos deje avanzar, como en get de AutoresController, si no estamos registrados
nos arrojara un error 401 unauthorize

el mecanismo por el cual los usuarios van a poder registrarse y loguearse lo haremos en el controlador cuentas, podemos ir
probando los registros de usuario y autenticacion para que nos retorne el barer o token con el cual podremos acceder a los 
demas recursos del aplicatiovo.

para poder consumir nuestro servicio con autheticacion desde nuestro cliente de suqger configuramos las opciones del servicio 
en Startup, lo que hacemos en esta configuracion es lo que tipicamente hacemos en una peticion con el encabezado
'Authorization: Bearer TOKEN,

 ya habiamos visto antes que podiamos agregar el decorador Authorize en el controlado y no en los end point para hacerlo global,
 pero tambien existe un decorador contrario al authorize que lo que hace es que un endpoint que esta bajo en controlador con authorize
 se lo salte, este decorador es el [AllowAnonymous]

 la autorizacion basada en claims me permite aparte de loguear a una persona dar permisos sobre recursos, puede que qeuramos 
 que una persona logueada haga unas tareas pero otras no, esto configurando el servicio addAuthorization en el appstarup

 luego de agregar nuestra politica la podemos aplicar en nuestros controladores como en el endpoint de autoresController de no tener
 el claim necesario retornara un erro 403 prohibido, reconoce que esta autenticado  per esa accion esta prohibida

-----------------------------------------------------------------------------------------------------------------------------

 el intercambio de recursos de origenes curzados es muy comun cuando hago una peticion desde el mismo origen , para configurar
 el cors agregamos el servicio AddCors en el startup

-----------------------------------------------------------------------------------------------------------------------------

 para enciptar en asp net core se nos ofrece el servicio de enciptacion de datosm, para acceder a los servicios de proteccion
 de datos ofercidos por .net agregamos el servicio services.AddDataProtection(); y luego inyectamos e instanciamos el servicio
 dentro del constructor de el controlador

-----------------------------------------------------------------------------------------------------------------------------

HATEOAS: con swagger tenemos como navegar nueestros endpoints, hateoas nos permite navegar por las acciones que puede realizar 
el cliente pero de manera mas especifica y detallada que swagger, para implementarlo a nivel de codigo lo que hacemos es
crear una clase que tenga propiedade de enlace, descriocion y metodo, para ello hacemos uso del dto DatoHATEOAS y Recurso

lo primero que hacemos en retornar desde una ruta las distintas rutas de nuestra api, sera como una raiz que reotornara todo lo 
que el usuario puede hacer, para ello creamos el controlador rootcontroller donde vamos a registrar todas las url de los end points
del cliente 

para que se agreguen dede autores un dto tenemos que hacer que nuestros dtohereden de recurso y crear un metoso para que asigne
las url del revurso 

como ya lo vimos en el controlador de autores en su metodo get podemos agregar y condicionar la generacion de los links pero 
en caso de que queramos minimizar el codigo que nos esta quedando en el metodo para una simple consulta GET lo que podemos hacer
es valernos de los filtros [HATEOASFiltroAttribute].

en conclucin el tema de hateoas es bueno en cuanto sea necesario retornar data detallada de las url o de los endpoints y no 
queramos hacer uso de swagger, el tema es muy denso en caso de que se quiera estudiar a profuncidad

-----------------------------------------------------------------------------------------------------------------------------

Varsionamiento Swagger

ya que las aplicacines pueden cambiar en el tiempo y no podemos dejar al cliente sin una vercion estable lo que se busca es
ofrecerles diferentes versiones de los controladores para ello lo que hacemos es crear una versin diferente de cada controlador
podemos comenzar creando las diferenctes carpetas de las versions y agrupar los controladores en ca carpeta de la version a trabajar

recuerde que los nombres de los metodos tambien deben cambiar por que si no van a generar conflicto en el despliegue

si ya tenemos los controladores separados por versiones, en la configuracion del swagger podremos seleccionar con que version del 
api queremos trabajar

es necesario primero en los servicios agregar una referencia a otro documento de swagger como version v2, adicional en el configure
configuramos para que dependiendo la ruta el swagger me muestre el codumetno al que corresponde

nos valemos de un utility [SwaggerAgrupaPorVersion] para poder agrupar los controladores por namespace para las diferentes versiones,
este lo agregamos como un servicio

si no queremos alterar los nombres de nuestras rutas tambien lo podemos hacer por medio de un filtro de cabecera 
[CabeceraEstaPresenteAttribute("header","valor")] y con esto como un filtro podremos definir si esta usando una version u otra
aunque de todas maneras necesitamremos cambiar los nombres de los metodos por lo que se hace redundante

de igual manera de esta manera no tenemos la ipcion de independendencia en el contenido del controlador, sencillamente si le envio
en la cabecera una version de controlador que no existe reventaria ya qaue no tenemos visibilidad de si el controlador existe o no


-----------------------------------------------------------------------------------------------------------------------------

Pruebas automaticas

consiste en automatizar las pruebas del desarrollo, se puede resumir como una funcion que prueba tu codigo, consta de tres partes
prepara, probar y verificar.

las pruebas unitarias se encargan de probar una unidad de trabajo, son muy rapidas y tienden a probar una funcion de una clase 

para crear una prueba unitaria creamos un nuevo proyecto de tipo test con el siguiente  comando

*** dotnet new mstest -o WebApi.Tests

para empezar a hacer referencia a los metodos de nuestro webapi ejecutamos el siguiente comando

*** dotnet add WebApi.Tests.csproj reference ../WebApi/WebApi.csproj

cuando tengamos construido el metodo de prueba podemos ejecutar nuestros test con el siguiente comando

*** dotnet test

aqui ya podemos probar los diferentes escenarios de nuestra aplicacion

a la hora de hacer pruebas no todas la clases que tenemos no tienen dependencia, existen casos como el de rootController
para poder controlar esas dependencias usamos un concepto llamado Moks para pderme concentrar en la funcionalidad real de lo
que quiero probar

para crear moks mas estandart y no tener que crear tantos por condicion nos valemos de la libreria muq, con esta libreria podemos 
abstenernos de crear muck por funcionalidad, ya depende de la necesidad la eleccion


-----------------------------------------------------------------------------------------------------------------------------

Despliegues

con este proceso llevamos el aplicactivo desde nuestro pc a un servidor de produccion, en esta caso vamos a ver en iis y azure app services

cabe anotar que no es un curso de azuro por lo que no entramos a detalle en las app services

desde vs damos en publish, luego escogemos azure - azure windows - creamos una insrtancioa de azure services que es donde va a vivir
el aplicativo, llenamos los datos de azure, uns vrx se crea el lugar donde vamos a poner la aplicacion damos en siguiente y luego damos publish
se generan las cofiguraciones del despliegue y configuramos las dependencias a otras necesidades como el de bases de datos y finalmente
publish y listo

ahora, como lo hariamos desde vs Code, lo primero es crear todos los recursosen azure, bases de datos, app service

una vez creado la ejecutamos las migraciones convirtiendolas primero es scriots con el siguiente comando

*** dotnet-ef migrations script -o script.sql -i

lo cual generarar el script sql de la base de datos para ser ejecutado en la bases ded datos de azure

para publicar la aplicacion ejecutamos el siguiente comando

*** dotnet publish --self-contained -c Release -r win-x64

despues de tener la publicacion local y la ruta donde esta la publicacion, una vez tengamos la publicacion lista vamos al 
paquete Azure app Services que se puede agregar a vs Code y nos logueamos, seleccionamos el app service que creamos, click derecho
deploy y buscamos la carpeta donde quedo la publicacion y damos enter y se completa el despliegue

para poder debuggar errores en aplicaciones en azure podemos hacer uso de [Insights] en azure

si tenemos nuestro codigo en github podemos implementar integracion continua creando un pipeline en donde podemos hacer que
una vez se detecte un cambio en una rama de github nos lo publique automaticamente, en la carpeta AzureFiles queda el yaml de ejemplo

como ya tenemos la integracion continua ahora hacemos el despliegue conttinuo agregando un nuevo release en dev ops