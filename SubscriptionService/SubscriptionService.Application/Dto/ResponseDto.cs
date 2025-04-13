using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionService.Application.Dto
{
    public class ResponseDto<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public static ResponseDto<T> Ok(T data)
        {
            return new ResponseDto<T>
            {
                Success = true,
                Data = data
            };
        }

        public static ResponseDto<T> Fail(string errorMessage)
        {
            return new ResponseDto<T>
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
