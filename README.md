# Job_Service
Job_Service asp.net core web application,Job_ServiceTests unit testing containing 
ViewController.cs,ErrorController.cs (Controllers) : User.cs,Error.cs(Model): CVRepository,ICVRepository (Services)

Base route - api/v1/ 
GET - jobs/{id?}
1.https://localhost:44365/api/v1/jobs 
Returns all the jobs in Postman from database table

2.https://localhost:44365/api/v1/jobs/1
Returns the job id where value is 1 listed in database table
Accept: application/json 

POST - jobs
https://localhost:44365/api/v1/jobs
Creates a new record in the database table with the json data provided in raw body of Postman
Content-Type:application/json;odata=verbose
Creates a new record in the database table with the json data provided in raw body of Postman

PUT - jobs/{id}")
https://localhost:44365/api/v1/jobs/1
Updates the record in the database table with the json data provided in raw body of Postman of the id value given
Accept: application/json 

GET - departments/{id?}
1.https://localhost:44365/api/v1/departments
Returns all the jobs listed in database table having non nullable departments

2.https://localhost:44365/api/v1/departments/2001
Returns all the jobs listed in database table with 2002 as department value

GET - locations/{id?}
1.https://localhost:44365/api/v1/locations
Returns all the jobs listed in database table having non nullable locations

2.https://localhost:44365/api/v1/locations/3001
Returns all the jobs listed in database table with 3001 as location value



