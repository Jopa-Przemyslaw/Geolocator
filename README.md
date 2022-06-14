# Geolocator
### Summary
This is a simple API backed by a SQL Database. The application is able to store geolocation data in the database based on the IP address. It uses https://ipinfo.io/ to get the geolocation data. The API is able to add, delete, read records and provide geolocation data on the base of IP address.

### Application specification
- it is a RESTful API
- it is build in .NET 6
- it is contenerized in Docker
- it is deployed in Azure
- it has a client pointing to https://ipinfo.io/ address
- it is operating using JSON (for both input and output)
- it is covered by Unit Tests
- it is the first version of this app

### Exposed Endpoints
The below list contains a base specification about the exposed endpoints. Wherever you see _{ip_address}_ replace it with any address you want.<br>
- To get geolocation record from the database for a specified IP address use:<br>
GET: /v1/Geolocation/_{ip_address}_
- To get geolocation from the external service (https://ipinfo.io/) use:<br>
GET: /v1/Geolocation/ext/_{ip_address}_
- To get all geolocations records from the database use:<br>
GET: /v1/Geolocation
- To save geolocation to the database use:<br>
POST: /v1/Geolocation<br>
with a request body in JSON.<br>
Note: IP address needs to be in a 0.0.0.0 - 999.999.999.999 range. The rest of fields are also required.<br>
Example:<br>
{<br>
  "ip":"27.88.171.22",<br>
  "hostname":"unknown@hosting.com",<br>
  "city":"Rome",<br>
  "region":"Lazio",<br>
  "country":"IT",<br>
  "loc":"12.1232,-12,1232",<br>
  "org":"Unknown Origin",<br>
  "postal":"12-123"<br>
}<br>
- To delete geolocation record from the database use:<br>
DELETE: /v1/Geolocation/_{ip_address}_<br>

### Deployed in Azure
App is hosted in cloud. Both, Web App and Sql Db are hosted in Azure.<br>
Available under: https://geolocator-webapp-acr.azurewebsites.net/ (available endpoints are described below).

#### Available Endpoints in Azure
Below endpoints are described with random and example data.
<br>
GET: https://geolocator-webapp-acr.azurewebsites.net/v1/Geolocation/{ip_address}
<br>
GET: https://geolocator-webapp-acr.azurewebsites.net/v1/Geolocation/ext/{ip_address}
<br>
GET: https://geolocator-webapp-acr.azurewebsites.net/v1/Geolocation
<br>
POST: https://geolocator-webapp-acr.azurewebsites.net/v1/Geolocation
with en example json body described above
<br> DELETE: https://geolocator-webapp-acr.azurewebsites.net/v1/Geolocation/27.88.171.22
