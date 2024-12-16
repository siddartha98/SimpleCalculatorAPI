# SimpleCalculatorAPI
A simple RESTful web service: a calculator for four basic operations (+, -, *, /). User calls the service by passing two numerical arguments and receives the result of the operation.
# To Test
- Clone and build repository.
- Ensure secure key is configured in the .env file.
- Run project -> Swagger UI will launch.
- First simulate login via the login endpoint to receive an auth token.
- Click Authorize button and follow instructions to use the above auth token to get authorized.
- Once authorized, proceed with testing the available arithmetic operations.
# NOTE
- In real-world projects ensure to add .env to .gitignore file to prevent exposing secret keys/tokens. It is included in this example project just for testing purposes.
