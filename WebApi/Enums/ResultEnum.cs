using WebApi.Extensions;

namespace WebApi.Enums
{
    public enum ResultEnum
    {
        [ReturnValues(true,1,"Operation Success")]
        OperationSucces,
        [ReturnValues(false, -1, "Technical Error")]
        Exception,
        [ReturnValues(true, 90, "Authentication Success")]
        AuthUser,
        [ReturnValues(false, 91, "Authentication Failed")]
        UnAuthUser,
        [ReturnValues(false,92,"Invalid token")]
        InvalidToken,
        [ReturnValues(false,93, "Missing Authorization Bearer")]
        MissingBearerToken,
        [ReturnValues(false,94,"Missing Jwt token")]
        MissingJwtToken,
        [ReturnValues(false,100,"Too many request")]
        TooManyRequests

    }
}