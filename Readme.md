

# Todo-Service

## Web-Api-Beschreibung
GET /api/TodoItems	Alle To-do-Elemente abrufen	Keine	Array von To-do-Elementen
GET /api/TodoItems/{id}	Ein Element nach ID abrufen	Keine	To-do-Element
POST /api/TodoItems	Neues Element hinzufügen	To-do-Element	To-do-Element
PUT /api/TodoItems/{id}	Vorhandenes Element aktualisieren  	To-do-Element	Keine
DELETE /api/TodoItems/{id}    	Löschen eines Elements    	Keine	Keine




mongo:
https://docs.microsoft.com/de-at/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio

The installation process installs both the MongoDB binaries as well as the default configuration file <install directory>\bin\mongod.cfg.

 MongoDBCompass --> GUI für mongo

https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/
 To begin using MongoDB, connect a mongo.exe shell to the running MongoDB instance
 C:\Program Files\MongoDB\Server\4.4\bin\mongo.exe
 The MongoDB instance is configured using the configuration file <install directory>\bin\mongod.cfg.

To start MongoDB, run mongod.exe.
C:\Program Files\MongoDB\Server\4.4\bin\mongod.exe" --dbpath="c:\data\db
The --dbpath option points to your database directory.

https://docs.mongodb.com/manual/mongo/#working-with-the-mongo-shell


-> "C:\Program Files\MongoDB\Server\4.4\bin\mongod.exe" --dbpath="C:\Users\Maxi\.FH\SWE\Microservice\MongoData"

<mongodb anlegen standaardport=27017 >
 1. "C:\Program Files\MongoDB\Server\4.4\bin\mongo.exe"
 2. use MenuPlan  DBAnlegen  Wenn nicht bereits vorhanden, wird eine Datenbank namens BookstoreDb erstellt. 
                  Wenn die Datenbank vorhanden ist, wird die Verbindung für Transaktionen geöffnet.
 3. db.createCollection('Menu') ->   Erstellen Sie eine Menu-Sammlung/Collection               
 4. db.Menu.insertMany([{'Name':'Eierspeise','Preis':11.11}, {'Name':'Schnitzel','Preis':22.22}]) -> Schema definieren
 5. db.Menu.find({}).pretty() -> Dokumente der DB anzeigen
</mongodb anlegen>

https://docs.microsoft.com/de-at/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio
<projekt>
 1.Install-Package MongoDB.Driver
 2. Hinzufügen eines Konfigurationsmodells -> appsettings.json

{
  "MenuplanDatabaseSettings": {
    "BooksCollectionName": "Menu",
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "MenuplanDb"
  },
  "Logging": {
    "IncludeScopes": false,
    "Debug": {
      "LogLevel": {
        "Default": "Warning"
      }
    },
    "Console": {
      "LogLevel": {
        "Default": "Warning"
      }
    }
  }
}



</projekt>

<docker>
https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/?view=aspnetcore-3.1

docker Compose: A command-line tool and YAML file format with metadata for defining and running multi-container applications.

compose: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/visual-studio-tools-for-docker?view=aspnetcore-3.1

bevor ein docker image gemacht wird
dotnet publish -c Release
This command compiles your app to the publish folder. The path to the publish folder from the working folder should be .\App\bin\Release\netcoreapp3.1\publish\


Log into Docker Hub and create your repository. This is the repository where your image will be stored. Go to https://hub.docker.com/ and log in with your credentials.
https://www.tutorialspoint.com/docker/docker_public_repositories.htm

docker pull demousr/demorep // image aus repo holen
docker inspect Container/Image  // what ports are exposed by the image - ExposedPorts die müssen  nun auf 
        //host   gemapped werden
sudo docker run -p 8080:8080 -p 50000:50000 jenkins // portmapping links=hostport  rechts=containerport
docker push Repositoryname // push image auf repo docker push demousr/demorep:1.0 


Dockerfile:
COPY <folder on local computer> <folder in Contaier>
WORKDIR /App //changes the current directory inside of the container to App.
ENTRYPOINT ["dotnet", "NetCore.Docker.dll"] //config cont to run as  executable When cont starts, the ENTRYPOINT command runs. When this command ends, the container will automatically stop.

FROM ubuntu 
RUN apt-get update 
RUN apt-get install –y apache2 
RUN apt-get install –y apache2-utils 
RUN apt-get clean 
EXPOSE 80 
CMD [“apache2ctl”, “-D”, “FOREGROUND”]

RUN command to install the necessary utility apache2 packages on our image.
EXPOSE command is used to expose port 80 of Apache in the container to the Docker host.
CMD command is used to run apache2 in the background.

Dockerfile Commands:
CMD command param1 ->  execute a command at runtime when the container is executed.
CMD [“echo” , “hello world”] 

ENTRYPOINT command param1 -> flexibler als cmd also be used to execute commands at runtime for the container.
ENV key value -> set environment variables in the container 
ENV var1=Tutorial var2=point 

WORKDIR dirname ->  set the working directory of the container.



docker build  -t ImageName:TagName <dir-dockerfile .> // build image from dockerfile


docker info // info über docker zb verwendete fs-treiber

sudo docker commit [CONTAINER_ID] [new_image_name] // mache neues image aus bestehendem dockercontainer
docker pull <imagename>
docker build -t <imagename> .
docker build -t counter-image -f Dockerfile .
docker create --name core-counter counter-image //create container
docker run -it --rm <containername>  [parameter  an cont übegeben] //create and run the container as a single command
docker run -it --rm --entrypoint "cmd.exe" counter-image // cont führt anderes programm aus
docker attach ContainerID // attached to container -> ich lande in seiner shell
docker pause ContainerID // pause the processes in a running container
docker unpause ContainerID
docker kill ContainerID // kill the processes in a running container

docker start <containername> [parameter  an cont übegeben]
docker stop  <containername> 
docker run -it --rm -p 5000l:80c --name <containername> <imagename>
docker ps -a //list of all containers
docker ps //list of running containers
docker kill 61a68ffc3851 
docker attach --sig-proxy=false <containername> //attach to running container to stdout
docker rm <containername>  // lösche container

docker images // list of images installed:
 docker ps //running containers
 docker exec aspnetcore_sample ipconfig // display the IP address of the container.
 docker rmi counter-image:latest // lösche image
docker rmi mcr.microsoft.com/dotnet/core/aspnet:3.1 // lösche image

docker log <container_name> // check logs
docker top ContainerID // see the top processes within a container.
docker stats ContainerID //  statistics of a running container.

docker network ls  // all the networks on the Docker Host.
docker network inspect networkname //  more details on the network associated with Docker

dockerd –l debug & // docker logging im debuglevel
docker logs containerID // see logs



docker volume create my-vol // crate volume
docker volume ls // list volumes 
docker volume inspect my-vol // inspect a volume
docker volume rm my-vol // remove volume

Start a container with a volume:
If you start a container with a volume that does not yet exist, Docker creates the volume for you. The following example mounts the volume myvol2 into /app/ in the container.

sudo docker inspect Jenkins // da sehe ich auch die zu mappenden volumes zb /var/jenkins_home
docker run -d   --name devtest   -v myvol2:/var/jenkins_home   nginx:latest

The –v option is used to map the volume in the container which is /var/jenkins_home to a location on our Docker Host which is myvol2.

docker inspect devtest //  details of an image or container
docker inspect devtest // verify that the volume was created and mounted correctly. Look for the Mounts section
docker volume rm myvol2 // remove volume
docker volume prune // remove all volumes 

The following example starts a nginx service with four replicas, each of which uses a local volume called myvol2.
docker service create -d \
  --replicas=4 \
  --name devtest-service \
  --mount source=myvol2,target=/app \
  nginx:latest

this example starts an nginx container and populates the new volume nginx-vol with the contents of the container’s /usr/share/nginx/html directory
docker run -d \
  --name=nginxtest \
  -v nginx-vol:/usr/share/nginx/html \
  nginx:latest






</docker>

<docker-compose>
https://docs.docker.com/compose/gettingstarted/
Use a volume with docker-compose:

docker-compose.yml
version: "3.7"
services:
  databases: //service1
    image mysql
    ports:
    - "80:80"
    environment:
    - MYSQL_ROOT_PASSWORD=pwd
  frontend: //service2
    image: node:lts
    volumes: 
      - myapp:/home/node/app
volumes:
  myapp:


docker-compose up //command will take the docker-compose.yml file in your local directory and start building the  
                  // containers. Once executed, all the images will start downloading and the containers will //
                  // start automatically.

docker ps // running containers
</docker-compose>

<config>
1.appsettings.json
2.runtimeconfig.template.json
{
  
    "configProperties": {
      "System.GC.Concurrent": false,
      "System.Threading.ThreadPool.MinThreads": 4,
      "System.Threading.ThreadPool.MaxThreads": 25
    }
  }
}
3.Environment variables
# Windows
set COMPlus_GCRetainVM=1

# Powershell
$env:COMPlus_GCRetainVM="1"

# Unix
export COMPlus_GCRetainVM=1

dotnet pack // create nuget
dotnet pack --configuration release
dotnet publish // deploying applications with all of their dependencies in the same bundle nix nuget

</config>

<12factor>
https://coderbook.com/@marcus/how-to-use-the-12-factor-app-methodology-in-practice/

https://medium.com/@FlywireEng/gem-puma-3-10-b622e350a8c6

https://medium.com/hashmapinc/how-i-use-the-twelve-factor-app-methodology-for-building-saas-applications-with-java-scala-4cdb668cc908


https://blog.newrelic.com/product-news/twelve-factor-app/

https://www.webagesolutions.com/blog/twelve-factor-applications-12-best-practices-for-microservices

 https://www.bmc.com/blogs/twelve-factor-app/
<1codebase>
  code -> git(version control) -> deployed version (staging prod)
</1codebase>

<2dependencies-declaration>
 (dependencies, code, binaries) -> App-Bundle
 dependency declarion = package.json dotnet restore
</2dependencies-declaration>

<2dependencies-isolation>
 app-deployments sollen alle ihre dependencies carry with them -> never depend on the host to have your dependencies
 (dependencies, code, binaries) -> docker

 !!!! dep decl + isolation : Docker
 from xx
 apk add
 workdir /svc
 add . .
 run npm install

 expose 3000
 cmd ["node","index.js"]

docker run dieses images auf dev, stging, prod
</2dependencies-isolation>

<3config>
config is part of environment on host (weil container ja gleich sind)
zur laufzeit bekommt container die config von der umgebung

AppCode holt config von environ -> db = process.env.Database
env is custom when docker runs container 
docker run -e "Database=mongodb://localhost:27017" -e "Secret=hhh" myapp
</3config> 
<4backingservices>
treat local services like remote 3rd party services
benutze cnames(canonical namen also domainnamen statt ip)
</4backingservices>

<5buildreleaserun>
build: (dependencies, code, binaries) -> docker
release:docker buildartifact + config = docker release image
amazon-elastic-container-service <- docker + config
run: relese -> run
</5buildreleaserun>
<6statelessprocesses>
falsch: stateful container speichert state in local disk,mem -> Workload ist an bestimmten host gebunden, der den state hat
richtig: benutze db(als service)
richtig: Cache (als service) schneller temp speicher redis, memcached
</6statelessprocesses>

<7portbinding>
?docker container mit fixem port versehen
Your	app	should	bind	to	a	port,	and	receive	requests	via	that	port
The	App	should	be	self-hosted
It	should	spin	up,	and	start	listening	on	a	port
Avoid	hosting	in	an	application	server,	like	IIS


</7portbinding>

<8concurrency>
? large host more conc proc, small host less conc proz
</8concurrency>

<9disposibility>
Maximize robustness with fast startup and graceful shutdown, responsive
fast launch:
  scape up faster in response to spikes
  fähigkeit proc auf andere hosts zu moven wenn nötig
  replace crashed processes faster
responsive graceful shutdown = should respond to sigterm by shutting down gracefully
  process.on(Sigterm) -> console log -> server close  

</9disposibility>

<10devprodparity>
Keep development, staging, and production as similar as possible
git -> dev,prod,parity via dockerimage
<10devprodparity>

<11logs>
 treat logs als event streams, keep  logic for routing + processing logs seperate from app itself



 log auf stdout
docker connect containers stdout to log driver
docker run --log-driver fluentid myapp

docker log <container_name> // check logs
</11logs>

<12adminprocesses>
- migrate db
-repair broken data
-einmal pro woche lösche datensätze älter als 1 woche
- einmal pro tag email report 
- run admin processes like other processes 
</12adminprocesses>

</12factor>

<cd>
docker compose

Twelve-Factor App
orchestrate this image inside Docker Swarm or Kubernetes or push it to
Cloud Foundry.

orchestration platforms like
Kubernetes, Docker Swarm, CoreOS Fleet, Mesosphere Marathon, Cloud
Foundry,

Wercker Team City and Jenkins


travis ci mit github

bamboo

Deploying to Docker Hub
</cd>



QueryTeamListReturnsCorrectTeams.


---------------------------------------------

By default all files created inside a container are stored on a writable container layer. This means that:

The data doesn’t persist when that container no longer exists, and

Docker has two options for containers to store files in the host machine, so that the files are persisted even after the container stops: volumes, and bind mounts.