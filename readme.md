# BookReviewer

Book Reviewer is an API that where users have the ability to leave their own reviews for books, and subscribe to specific books in order to recieve notification of new reviews appeared for those books.

### Technologies Used
- ASP.Net Core;
- MongoDB databases for storing books, reviews, subscriptions and user data;
- RabbitMQ for communication between microservices;
- Ocelot as API Gateway;
- JWT for authentication and authorization;
- MailKit library to send e-mails via SMTP;

### Application Structure

Application is built using microservices architecture approach. The diagram below shows how different modules of the application are connected with each other:

![bookreviewerDiagram](https://github.com/alshuriga/book-reviewer/assets/8162224/64650aca-c554-47d3-a80e-aeed0f98f59e)


### How to launch

-  Clone repository
-  **Create .env file** in *src* directory and add following **enviromental variables**:

```
#SMTP configuration:
EmailConfigurationHost=
EmailConfigurationPort=
EmailConfigurationEmail=
EmailConfigurationPassword=

#JWT configuration
JwtSettingsJwtSecret=
JwtSettingsLifetimeSeconds=
JwtSettingsJwtIssuer=
```

-  Launch docker compose in *src* directory:

```
docker compose up
```

-  Access API at http://localhost:3000/. Swagger Documentation is also available at http://localhost:3000/swagger
