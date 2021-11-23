using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazingStore.Application.Interfaces;
using Newtonsoft.Json;

namespace AmazingStore.Application.Models.Common
{
    public class AppServiceResponse<T> : IAppServiceResponse where T : class
    {
        #region Constructors

        public AppServiceResponse(T data, string message, bool success)
        {
            Data = data;
            Message = message;
            Success = success;
        }

        protected AppServiceResponse()
        {
        }

        #endregion Constructors

        #region Properties

        public T Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

        #endregion Properties
    }
}