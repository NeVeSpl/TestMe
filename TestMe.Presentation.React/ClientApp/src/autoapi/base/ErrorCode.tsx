enum ErrorCode
{
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    Conflict = 409,
    DomainException = 422,   
    InternalServerError = 500,
    NetworkError = 600,
}

export default ErrorCode;