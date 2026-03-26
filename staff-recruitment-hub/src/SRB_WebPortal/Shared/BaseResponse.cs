using System.Net;
using System.Text.Json.Serialization;

namespace SRB_WebPortal.Shared;

public static class BackendSignals
{
   public const string SESSION_EXPIRED = "SIG_SESSION_TIMED_OUT";
   // Hết hạn bình thường
   public const string SESSION_NOT_FOUND = "SIG_INVALID_ORIGIN";
   // Nghi vấn hack - SessionId bất thường
   public const string TAMPERED_TOKEN = "SIG_TAMPERED_TRACE";
   // Dấu hiệu can thiệp token
   public const string INVALID_TOKEN = "SIG_INVALID_TOKEN";
   // Token không chính xác hoặc sai
}

public class BaseResponse<T>
{
   public bool IsSuccess { get; set; }
   public int StatusCode { get; set; }
   public string? ReasonCode { get; set; }
   public string? Message { get; set; }

   [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
   public T? Data { get; set; }

   [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
   public List<string>? Errors { get; set; }

   public BaseResponse() { }

   public static BaseResponse<T> Success(T data, string message = "", HttpStatusCode statusCode = HttpStatusCode.OK)
   {
      return new BaseResponse<T>
      {
         IsSuccess = true,
         StatusCode = (int)statusCode,
         Message = message,
         Data = data
      };
   }

   public static BaseResponse<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? reasonCode = null, List<string>? errors = null)
   {
      return new BaseResponse<T>
      {
         IsSuccess = false,
         StatusCode = (int)statusCode,
         ReasonCode = reasonCode,
         Message = message,
         Errors = errors
      };
   }
}

public class BaseResponse : BaseResponse<object>
{
   public static BaseResponse Success(string message = "", HttpStatusCode statusCode = HttpStatusCode.OK)
   {
      return new BaseResponse
      {
         IsSuccess = true,
         StatusCode = (int)statusCode,
         Message = message
      };
   }

   public new static BaseResponse Failure(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? reasonCode = null, List<string>? errors = null)
   {
      return new BaseResponse
      {
         IsSuccess = false,
         StatusCode = (int)statusCode,
         ReasonCode = reasonCode,
         Message = message,
         Errors = errors
      };
   }

   // 400 Bad Request
   public static BaseResponse BadRequest(string message = "Bad request", string? reasonCode = null, List<string>? errors = null)
      => Failure(message, HttpStatusCode.BadRequest, reasonCode, errors);

   // 401 Unauthorized
   public static BaseResponse Unauthorized(string message = "Unauthorized access", string? reasonCode = null)
      => Failure(message, HttpStatusCode.Unauthorized, reasonCode);

   // 403 Forbidden
   public static BaseResponse Forbidden(string message = "You do not have permission to access this resource")
      => Failure(message, HttpStatusCode.Forbidden);

   // 404 Not Found
   public static BaseResponse NotFound(string message = "Resource not found")
      => Failure(message, HttpStatusCode.NotFound);

   // 409 Conflict
   public static BaseResponse Conflict(string message = "Resource already exists")
      => Failure(message, HttpStatusCode.Conflict);

   // 422 Unprocessable Entity
   public static BaseResponse UnprocessableEntity(
      string message = "Validation failed", string? reasonCode = null, List<string>? errors = null
   ) => Failure(message, HttpStatusCode.UnprocessableEntity, reasonCode, errors);

   // 500 Internal Server Error
   public static BaseResponse InternalServerError(string message = "An unexpected error occurred")
      => Failure(message, HttpStatusCode.InternalServerError);
}
