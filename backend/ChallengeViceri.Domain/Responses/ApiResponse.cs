using System.Net;

namespace ChallengeViceri.Domain.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse(HttpStatusCode code, T? result, Dictionary<string, string>? errors = null)
        {
            Code = code;
            Result = result;
            Errors = errors ?? null;
            Message = GenerateMessage(code, result);
        }

        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public T? Result { get; set; }
        public Dictionary<string, string>? Errors { get; set; }

        private string GenerateMessage(HttpStatusCode statusCode, T? result)
        {
            return statusCode switch
            {
                HttpStatusCode.OK => "Request successfully completed.",
                HttpStatusCode.Created => "Resource successfully created.",
                HttpStatusCode.BadRequest =>
                    AddErrorAndReturnMessage("Invalid request.", result),
                HttpStatusCode.Unauthorized => "Unauthorized access.",
                HttpStatusCode.Forbidden => "Access forbidden.",
                HttpStatusCode.NotFound =>
                    AddErrorAndReturnMessage("Resource not found.", result),
                HttpStatusCode.InternalServerError =>
                    AddErrorAndReturnMessage("Internal server error.", result),
                _ =>
                    AddErrorAndReturnMessage("Unknown error occurred.", result)
            };
        }

        private string AddErrorAndReturnMessage(string defaultMessage, T? result)
        {
            if (result != null)
            {
                Errors = new Dictionary<string, string>();
                Errors["General"] = result.ToString()!;
            }
            return defaultMessage;
        }
    }
}
