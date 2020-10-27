# Faktor 1(Codebasis)
1. Git + Github als CodeRepository/Versionskontrollsystem
2. Ein Coderepository pro Anwendung
3. Deploy + Build ist über Github-Actions(Yaml) gelöst.
4. Codebasis ist für jedes Deploy Dev, Staging, Prod die selbe.

# Faktor 2(Abhängigkeiten)
Dotnet Core auf welchen die Anwendung basiert, verwendet ein Packagingsystem(NugetPackageMgr),
Somit ist es einfach eine Codebasis zu verwenden und durch durch Aufrufe wie dotnet restore welche Aufgrund der packages.config die richtigen Abhängigkeiten(NuGet-Packete) automatisch in das Projekt reinziehen.
und auf dem richtigen Stand zu halten. Die Packetkonfiguration entspricht dem geforderten Abhängigkeiten-Deklarations-Manifest
Durch Kennzeichnung von einzelnen Packeten  als DevelopementDependencys in der Paketkonfiguration erreicht man unterschiedliche Abhängigkeitenspezifikationen für Entwicklung und Produktion mit nur einer
Abhängigkeitsspezifaktiosndatei.
Alle Deployments tragen ihre Abhängigkeiten, in sich selbst und sind nicht vom Host auf welchem die App abläuft abhängig, dass dieser die Abhängigkeiten bereitstellt.
Docker ist hier das Mittel der Wahl.

# Faktor 3(Konfiguration wird erst in der Ablaufumgebung definiert)
Es besteht eine strikte Trennung von Konfiguration und Code(der ja für jedes Deployment gleich sein soll)
Geht man von Dockercontainern, die ja alle gleich sind, egal ob sie in Dev-, Staging- oder Produktivumgebung laufen aus, so sind unterschiedliche Umgebungsvariablen die je verwendeter
Umgebung unterschiedlich parameterisiert sein können, das Ziel. 
Diese Umgebungsvariablen werden von der App eingelesen. So kann man z.B auf unterschiedliche Datastores zugreifen.
In meiner App lese ich den Connectionstring, den DB- und Collectionnamen über Umgebungsvariablen aus.
Je Umgebung werden die Variablen entweder händisch im Hostbetriebssystem gesetzt(mühsam), durch -e Parameter beim Aufruf von docker run gesetzt, oder wie ich es mache über Parameter im
docker-compose oder dessen override.yaml(bevorzugt) gesetzt.

# Faktor 4(Behandle Backingservices als angehängte Ressourcen)
Backingservices sind angehängte Ressourcen weil sie über kanonische Namen, 
oftmals URLS(mit Domainnamen) angesprochen oder verlinkt werden.
Dadurch wird es in der Produktionsumgebung einfach, solche eine verlinktes Backingservice
auszutauschen ohne den Code, oder die Konfiguration des Hauptservices ändern zu müssen.
In meiner App im docker-composer.yaml über den Eintrag links: "databases" erreicht dass der Connectionstring
- ConnectionString=mongodb://databases:27017 databases refernzieren kann. databases ist hier ein kanonischer 
Name und hat den Vorteil, dass nicht einmal das Hauptservice gestoppt werden muss um seine Konfiguration zu ändern.
Es ist ausreichend das neue Backingservice zu starten, denn es kann aufgrund des Dockernetworkings
unter dem selben kanonischen Hostnamen wie zuvor erreicht werden.
Dieser Punkt wird durch den Faktor 3 erleichtert. 

# Faktor 5(Trennen von build und run stages)
Build das Erzeugen von Binaries, Releases die aus den Binaries und der Konfiguration entstehen und die Runtimestage müssen getrennt werden.
Die Binaries müssen immuteable sein, jede Codeänderung bedarf eines neuen Releases mit einer eindeutigen Version(Tag).
In meiner App habe ich das mit Docker umgesetzt. Die Build-Stage ist das builden des DockerImages welches ich mit einem eindeutigem Tag versehen habe.
Das Image habe ich in das Docker.Hubrepository gespielt.
Für die Running-Stage wird dieses Image in meiner App über Docker-Compose aus dem Hubrepository
gezogen und mit den Docker-Compose-Parametern parametrisiert als Container-App gestartet.

# Faktor 6 Prozesse(App als stateless Prozeß)
Die App hält/speichert keinen eigenen State. Meine App ist ein zustandsloser WebApi-Provider. 
Der State wird im Backenddienst(MongoDB) gehalten. Und die aus Punkt 3 bedingte Konfiguration aus Hostvariablen, welche meine App im Code aus den Umgebungsvariablen einliest ermöglichen die absolute Zustandsfreiheit der App.
Der Vorteil ist eine hohe Skalierbarkeit, weil die App

# Faktor 7(Port-Binding)
Jeder Dienst soll über Urls angesprochen werden können, auch meine App.
Meine App bietet über das Dockerfile die Möglichkeit Ports zu exposen. 
Im meiner App werden die Ports 80 und 5000(nicht benötigt, als Reserve gehalten) exposed.
Wenn meine App nun in einem Dockercontainer läuft ist es möglich über Portmapping welches in Docker über -p <hostport>:<containerport> ereicht werden kann, oder wie im Falle meiner App über docker-compose.override.yml
über ports:- "88:80" eingestellt wird, das Service der Außenwelt, oder anderen Diensten welche meine App wiederum als Backupdienst benutzen, anzubieten.
Meine App ist auch komplett self-contained, benötigt also keinen Webserver oder ähnliches um lauffähig zu sein. Das ist in meiner App in Program.cs umgesetzt, indem Kestrel verwendet wird und auf IIS oder IIS-Express oder andere Webservice-Container verzichtet wird.

# Faktor 8(Concurrency)
Meine App kann horizonatl skaliert werden, indem einfach mehrere Container mit dem AppDienst gestartet werden.
Das funktioniert wie unter Faktor 5 beschrieben aufgrund der zustandslosen Architektur der App, welche es einfach
macht die App horizonatl zu skalieren.
Da meine App einerseits zustandslos und abererseits als Dockerimage deployed wird ist dieser Punkt erfüllt, da mit Leichtigkeit weitere Container instanziert werden können.

# Faktor 9(Disposibility)
Meine App ist schnell startbar aber verfügt über kein geordnetes Shutdownverhalten. 
In Startup.Configure wird ein Shutdownhandler registriert, der bei Empfang des  Signals Sigterm ausgeführt wird.
Hier passiert aber derzeit nur ein Logging
Was aber fehlt ist das Abwarten der Beendigung aktiver Requests

# Faktor 11(Logs)
Meine App schreibt die Logs nicht in eine Datei, oder ein Logservice
sondern streamt direkt in stdout. 


