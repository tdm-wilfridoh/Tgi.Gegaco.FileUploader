using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgi.Gegaco.FileUploader.Application.Common.Models
{
    public class Result<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        private Result(T data) {
            this.Data = data;
            this.IsSuccess = true;
        }

        private Result(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            this.IsSuccess = false;
        }

        public static Result<T> Success(T data) => new Result<T>(data);

        public static Result<T> Error (string errorMesage) => new Result<T>(errorMesage);

        public static implicit operator bool(Result<T> result)
            => result.IsSuccess;
    }
}
