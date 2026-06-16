# Postman tests

Postman API tests are exported as collections, postman tests verify API compatibility with external client machine and correct responses with wrong and correct data. Purpose of this is to check correctness of API calls of backend section.

## Usage

Open postman and import collections. Run methods manually or run them tests of each collection.

## Sections

For each dotnet DTOs and endpoints there are postman collections with dummy data, also with wrong and correct json schemas of each endpoint API call. In each collections there are tests with GET, POST, PUT and DELETE.

# SQL files

Here you can find .sql files used for development and testing. 
* `clear.sql` for clearing all data and adding new mocks.
* `verify_new_db_data.sql` for checking data in databse tables and table archives.
