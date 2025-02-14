# fetch-receipt-rewards

## Details

This is an implementation of the Receipt Rewards project. This is done in C# on .NET 8.0. This is setup in a docker container for easy testing. 

## Docker Setup Instructions

Run `docker build -t fetch-receipts:latest .`

Run `docker run -d -p 8080:8080 fetch-receipts:latest`

Optionally, navigate to `http://localhost:8080/swagger` to get a UI view of the Receipts API. In fact, it should redirect there even if you only navigate to root. API endpoints should match the spec provided in the `api.yml` file.

## Code Navigation

`/Attributes`: Just one attribute that made binding the timestamp easier - there were some quirks around the HH:mm format that needed handling.

`/Controllers`: This has the API and points calculation code.

`/Models`: Data bindings for the json spec

There is also a test project in here with `ReceiptsControllerTests.cs` in it. It's a quick and dirty test suite for the API - not too extensive due to self-imposed time constraints.

## Why .Net?

It's just what I'm using in my current job, and I've got a lot of familiarity with it, and I wanted to limit my time spent on this. Using another framework would have meant more time spent. 