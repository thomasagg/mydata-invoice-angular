# myDATA Invoice Manager (.NET / Angular)

The same app as [mydata-invoice-manager](https://github.com/thomasagg/mydata-invoice-manager), rebuilt using C#/.NET and Angular. Same functionality, different stack. Users submit invoices to the Greek tax authority (AADE) via the myDATA API and see whether they were accepted or rejected.

## Tech Stack

**Backend:** C#, .NET 10, ASP.NET Core, Entity Framework Core, PostgreSQL  
**Frontend:** Angular 21, Tailwind CSS, @ngx-translate  
**Infrastructure:** Docker, Docker Compose

## What it does

- Register/login with JWT auth
- Manage clients
- Create invoices and submit them to AADE automatically
- See whether each invoice was accepted or rejected, and why
- Cancel or resubmit invoices
- Switch between Greek and English
- Dark mode

## Running the App

Requires Docker and Docker Compose.

```bash
docker-compose up --build
```

- Frontend: http://localhost:80  
- Backend API: http://localhost:8080

## API Endpoints

```
POST   /api/auth/register
POST   /api/auth/login

GET    /api/clients
POST   /api/clients
PUT    /api/clients/{id}
DELETE /api/clients/{id}

GET    /api/invoices
POST   /api/invoices
GET    /api/invoices/{id}
POST   /api/invoices/{id}/cancel
POST   /api/invoices/{id}/resubmit
```

## Related Project

Same application built with Java 21 / Spring Boot and React: [mydata-invoice-manager](https://github.com/thomasagg/mydata-invoice-manager)
