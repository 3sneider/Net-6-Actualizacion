Apuntes tutorial y novedades sobre .net 6

---------------------------------------------------------------------------------------------------------------------------------

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

---------------------------------------------------------------------------------------------------------------------------------

