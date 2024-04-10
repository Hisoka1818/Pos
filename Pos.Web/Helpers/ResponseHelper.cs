using Pos.Web.Core;

namespace Pos.Web.Helpers
{
    public class ResponseHelper<T>
    {
        public static Response<T> MakeResponseSuccess(T model)
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = "Tarea realizada con éxito",
                Result = model
            };
        }

        public static Response<T> MakeResponseSuccess(T model, string message)
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
                Result = model
            };
        }

        public static Response<T> MakeResponseSuccess(string message)
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static Response<object> MakeResponseFail(List<string> errors)
        {
            return new Response<object>
            {
                IsSuccess = false,
                Message = "Error al generar la solicitud",
                Errors = errors
            };
        }

        public static Response<T> MakeResponseFail(List<string> errors, string message)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors
            };
        }

        public static Response<T> MakeResponseFail(Exception ex)
        {
            List<string> errors = new List<string>
            {
                ex.Message
            };

            return new Response<T>
            {
                IsSuccess = false,
                Message = "Error al generar la solicitud",
                Errors = errors
            };
        }

        public static Response<T> MakeResponseFail(string error)
        {
            List<string> errors = new List<string>
            {
                error
            };

            return new Response<T>
            {
                IsSuccess = false,
                Message = error,
                Errors = errors
            };
        }
    }
}
