# Faktor 1(Codebasis)
1. Git + Github als CodeRepository/Versionskontrollsystem
2. Ein Coderepository pro Anwendung
3. Deploy + Build ist �ber Github-Actions(Yaml) gel�st.
4. Codebasis ist f�r jedes Deploy Dev, Staging, Prod die selbe.

# Faktor 2(Abh�ngigkeiten)
Dotnet Core auf welchen die Anwendung basiert, verwendet ein Packagingsystem(NugetPackageMgr),
Somit ist es einfach eine Codebasis zu verwenden und durch durch Aufrufe wie dotnet restore welche Aufgrund der packages.config die richtigen Abh�ngigkeiten(NuGet-Packete) automatisch in das Projekt reinziehen.
und auf dem richtigen Stand zu halten. Die Packetkonfiguration entspricht dem geforderten Abh�ngigkeiten-Deklarations-Manifest
Durch Kennzeichnung von einzelnen Packeten  als DevelopementDependencys in der Paketkonfiguration erreicht man unterschiedliche Abh�ngigkeitenspezifikationen f�r Entwicklung und Produktion mit nur einer
Abh�ngigkeitsspezifaktiosndatei.
Alle Deployments tragen ihre Abh�ngigkeiten, in sich selbst und sind nicht vom Host auf welchem die App abl�uft abh�ngig, dass dieser die Abh�ngigkeiten bereitstellt.
Docker ist hier das Mittel der Wahl.

# Faktor 3(Konfiguration wird erst in der Ablaufumgebung definiert)
Es besteht eine strikte Trennung von Konfiguration und Code(der ja f�r jedes Deployment gleich sein soll)
Geht man von Dockercontainern, die ja alle gleich sind, egal ob sie in Dev-, Staging- oder Produktivumgebung laufen aus, so sind unterschiedliche Umgebungsvariablen die je verwendeter
Umgebung unterschiedlich parameterisiert sein k�nnen, das Ziel. 
Diese Umgebungsvariablen werden von der App eingelesen. So kann man z.B auf unterschiedliche Datastores zugreifen.
In meiner App lese ich den Connectionstring, den DB- und Collectionnamen �ber Umgebungsvariablen aus.
Je Umgebung werden die Variablen entweder h�ndisch im Hostbetriebssystem gesetzt(m�hsam), durch -e Parameter beim Aufruf von docker run gesetzt, oder wie ich es mache �ber Parameter im
docker-compose oder dessen override.yaml(bevorzugt) gesetzt.

# Faktor 4(Behandle Backingservices als angeh�ngte Ressourcen)
Backingservices sind angeh�ngte Ressourcen weil sie �ber kanonische Namen, 
oftmals URLS(mit Domainnamen) angesprochen oder verlinkt werden.
Dadurch wird es in der Produktionsumgebung einfach, solche eine verlinktes Backingservice
auszutauschen ohne den Code, oder die Konfiguration des Hauptservices �ndern zu m�ssen.
In meiner App im docker-composer.yaml �ber den Eintrag links: "databases" erreicht dass der Connectionstring
- ConnectionString=mongodb://databases:27017 databases refernzieren kann. databases ist hier ein kanonischer 
Name und hat den Vorteil, dass nicht einmal das Hauptservice gestoppt werden muss um seine Konfiguration zu �ndern.
Es ist ausreichend das neue Backingservice zu starten, denn es kann aufgrund des Dockernetworkings
unter dem selben kanonischen Hostnamen wie zuvor erreicht werden.
Dieser Punkt wird durch den Faktor 3 erleichtert. 

# Faktor 5(Trennen von build und run stages)
Build das Erzeugen von Binaries, Releases die aus den Binaries und der Konfiguration entstehen und die Runtimestage m�ssen getrennt werden.
Die Binaries m�ssen immuteable sein, jede Code�nderung bedarf eines neuen Releases mit einer eindeutigen Version(Tag).
In meiner App habe ich das mit Docker umgesetzt. Die Build-Stage ist das builden des DockerImages welches ich mit einem eindeutigem Tag versehen habe.
Das Image habe ich in das Docker.Hubrepository gespielt.
F�r die Running-Stage wird dieses Image in meiner App �ber Docker-Compose aus dem Hubrepository
gezogen und mit den Docker-Compose-Parametern parametrisiert als Container-App gestartet.

# Faktor 6 Prozesse(App als stateless Proze�)
Die App h�lt/speichert keinen eigenen State. Meine App ist ein zustandsloser WebApi-Provider. 
Der State wird im Backenddienst(MongoDB) gehalten. Und die aus Punkt 3 bedingte Konfiguration aus Hostvariablen, welche meine App im Code aus den Umgebungsvariablen einliest erm�glichen die absolute Zustandsfreiheit der App.
Der Vorteil ist eine hohe Skalierbarkeit, weil die App

# Faktor 7(Port-Binding)
Jeder Dienst soll �ber Urls angesprochen werden k�nnen, auch meine App.
Meine App bietet �ber das Dockerfile die M�glichkeit Ports zu exposen. 
Im meiner App werden die Ports 80 und 5000(nicht ben�tigt, als Reserve gehalten) exposed.
Wenn meine App nun in einem Dockercontainer l�uft ist es m�glich �ber Portmapping welches in Docker �ber -p <hostport>:<containerport> ereicht werden kann, oder wie im Falle meiner App �ber docker-compose.override.yml
�ber ports:- "88:80" eingestellt wird, das Service der Au�enwelt, oder anderen Diensten welche meine App wiederum als Backupdienst benutzen, anzubieten.
Meine App ist auch komplett self-contained, ben�tigt also keinen Webserver oder �hnliches um lauff�hig zu sein. Das ist in meiner App in Program.cs umgesetzt, indem Kestrel verwendet wird und auf IIS oder IIS-Express oder andere Webservice-Container verzichtet wird.

# Faktor 8(Concurrency)
Meine App kann horizonatl skaliert werden, indem einfach mehrere Container mit dem AppDienst gestartet werden.
Das funktioniert wie unter Faktor 5 beschrieben aufgrund der zustandslosen Architektur der App, welche es einfach
macht die App horizonatl zu skalieren.
Da meine App einerseits zustandslos und abererseits als Dockerimage deployed wird ist dieser Punkt erf�llt, da mit Leichtigkeit weitere Container instanziert werden k�nnen.

# Faktor 9(Disposibility)
Meine App ist schnell startbar aber verf�gt �ber kein geordnetes Shutdownverhalten. 
In Startup.Configure wird ein Shutdownhandler registriert, der bei Empfang des  Signals Sigterm ausgef�hrt wird.
Hier passiert aber derzeit nur ein Logging
Was aber fehlt ist das Abwarten der Beendigung aktiver Requests

# Faktor 11(Logs)
Meine App schreibt die Logs nicht in eine Datei, oder ein Logservice
sondern streamt direkt in stdout. 


