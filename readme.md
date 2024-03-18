# Me Faz um Pix Aí

"Faz um pix aí" is a simple API that allows PSPs (PaymentProviders) connect to the BC (Banco Central) to create and retrieve Pix keys to their users. It was developed using ASP and .NET 8 and Entity Framework Core.
The API was meant to fast and scalable, so it was developed with performance in mind.

## How to Run the App

### Prerequisites

You need installed on your machine:

- .NET 8
- Docker
- Grafana k6 (For load testing).

### Notes

- Note that `network_mode` is set for all containers as `host`. This configuration helped me to run the containers but can cause issues in other environments. If you have any issues, this may be a cause.
- Verify if the database environment variables are correctly set. If not, modify them as needed.
- Copy the `.env.example` to `.env` and change the database URL if necessary

### Running the Project

In the project root, run the following commands:

```sh
dotnet ef database update # For applying the migrations
docker compose up -d # For running the database, rabbitmq and api
```
And you will need to run the PixWorker and PspMock.
PixWorker: https://github.com/PPAKruNN/fazumpix-worker
PspMock: https://github.com/DiegoPinho/psp-mockl

The api will be running on `localhost:5000`
and RabbitMQ Managment will be running on `localhost:15672`

#### How to Run Monitoring

- Navigate to the `/Monitoring` folder and run:

```sh
docker compose up -d
```

Then, open Grafana on `localhost:3000`.

#### How to Run Load Tests
Navigate to the `k6` folder.

If needed, change configurations variables on `seed.js` (Database entity count, urls, webhook urls)

To seed the database, run:

```sh
npm run seed
```

To run the load tests against the `POST /keys` endpoint, run:

```sh
npm run test:post
```

To run the load tests against the `GET /keys` endpoint, run:

```sh
npm run test:get
```

To run the load tests against the `POST /payments` endpoint, run:

```sh
npm run test:payments
```
