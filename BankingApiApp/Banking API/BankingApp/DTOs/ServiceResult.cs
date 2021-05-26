

using System.Collections.Generic;

namespace BankingApp.DTOs
{
    public class ServiceResult<T>
    {
        public ServiceResult(T result)
        {
            Result = result;
        }

        public ServiceResult(List<string> errors)
        {
            Errors = errors;
        }

        public T Result { get; set; }
        public List<string> Errors { get; set; }
    }
}
