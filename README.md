# Geolocator
This is a briefly example of an Web API in **.NET 6** with **Docker** and **Sql Server** used.
<br>
<br>
App is also hosted in cloud. Both, Web App and Sql Db are hosted in **Azure**.
<br>Available under: https://geolocator-webapp-acr.azurewebsites.net/ (available endpoints are described below).

## Available Endpoints
Below endpoints are described with random and example data.
<br>
GET: https://geolocator-webapp-acr.azurewebsites.net/v1/Geolocation/31.62.171.98
<br>
GET: https://geolocator-webapp-acr.azurewebsites.net/v1/Geolocation/ext/21.71.171.98
<br>
GET: https://geolocator-webapp-acr.azurewebsites.net/v1/Geolocation
<br>
POST: https://geolocator-webapp-acr.azurewebsites.net/v1/Geolocation
with en example json body:
{"ip":"27.88.171.22","hostname":"unknown@hosting.com","city":"Rome","region":"Lazio","country":"IT","loc":"12.1232,-12,1232","org":"Unknown Origin","postal":"12-123"}
<br> DELETE: https://geolocator-webapp-acr.azurewebsites.net/v1/Geolocation/27.88.171.22
