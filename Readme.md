# Menu-Service

## Web-Api-Beschreibung
Aufruf | Beschreibung
------------ | -------------
GET /api/menu | Alle Menu-Elemente abrufen	
GET /api/menu/{id} | Ein Menu-Element mit seiner ID abrufen
POST /api/menu | Neues Menu-Element hinzufügen	
PUT /api/menu/{id} | Vorhandenes Menu-Element aktualisieren
DELETE /api/menu/{id} | Löschen eines  Menu-Elementes
> Beiliegend **Menu-Microservice.postman_collection.json** eine exportierte Postman-Collection um die Web-Apiaufrufe ohne Verwendung des Dienstes in Docker durchzuführen.
> Beiliegend **Menu-MicroserviceDocker.postman_collection.json** eine exportierte Postman-Collection um die Web-Apiaufrufe unter Verwendung des Dienstes in Docker durchzuführen.

## Artefaktespeicher
Art | Ort | Beschreibung
------------ | ------------- | ------------
Zip-Archiv | Hochgeladen in Moodle | Beinhaltet sämtl. Quellcode, Docker + Docker-Compose-Configurationen
**Code**GitHubRepository | [Github](https://github.com/leageorg/SWE) | Code-Repository inkl Docker + Docker-Compose-Configurationen, CICD-Configuration in Yaml
**Produktion**GitHubRepository | [Github-Produktion](https://github.com/leageorg/SWE-Produkt) | Beinhaltet die **Docker-Compose-Configuration** zum Testen und die Postman-Collection. Achtung die Ids der Web-Apis mit {id} beginnen nicht bei 1 das ist mongo geschuldet !
DockerHubRepository | [DockerHub](https://hub.docker.com/repository/docker/leageorg/swe2020wm) | Docker-Image zum download

## Verwendete Technologien
- Entwicklung
    - Visualstudio
    - DotNetCore / C#
- Repository CI
    - Git
    - Github
- CD
    - Github Actions als Yaml, selbst in Github abgelegt
    - DockerHub als Docker-Image-Repo
- Datenbank des Backing-Service(Dataservice)
    - MongoDB als lokal installierte Mongo-Instanz oder
    - MongoDB als Docker-Container
- Ablaufumgebung
    - Ohne Docker-Container direkt im OS
    - In Docker-Container, gemeinsam mit dem DB-Service(Mongo) mittels Docker-Compose startbar.

# **Starten-**Menu-Service (Wirtshaus-Menü-Service)
1. **Hole.Docker-Compose-Configuration** [Github-Produktion](https://github.com/leageorg/SWE-Produkt)
2. Kommandozeile  `docker-compose up`
3. **Testen **[ Get-All](http://localhost:88/api/menu/)
4. Kommandozeile  `docker-compose down`
