# BookReviewer

BookReviewer is an API built with microservices that provides users the ability to leave their own book reviews and subscribe to specific books to receive email notifications of new reviews.

### Technologies Used
- ASP.Net Core;
- MongoDB databases for storing books, reviews, subscriptions and user data;
- RabbitMQ for communication between microservices;
- Ocelot as API Gateway;
- JWT for authentication and authorization;
- MailKit library to send e-mails via SMTP;

### Application Structure

Application is built using microservices architecture approach. The diagram below shows how different modules of the application are connected with each other:

![bookreviewerDiagram](https://github.com/alshuriga/book-reviewer/assets/8162224/af0e6e8f-9d48-4355-ab19-538021f489fa)


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

#InitialAdminAccountCredentials (password min. length is 8, must have uppercase, lowercase, numeric and special character)
InitialAdminUserCredentialsEmail=
InitialAdminUserCredentialsPassword=
```

-  Launch docker compose in *src* directory:

```
docker compose up
```

-  Access API at http://localhost:3000/. Swagger Documentation is also available at http://localhost:3000/swagger
