using System;
using System.Net;
using RestSharp;

namespace Jueci.WeChatApp.RestfulRequestTool.Common
{
    /// <summary> Provides data from an API request. </summary>
    /// <typeparam name="T"> The type of the data for this result. </typeparam>
    public class RequestResult<T>
    {
        /// <summary> The <see cref="HttpStatusCode" /> returned by the server for this request. </summary>
        public HttpStatusCode StatusCode { get; }
        /// <summary> The results from the query. </summary>
        public T Data { get; }
        /// <summary> Creates a new <see cref="RequestResult{T}" /> instance. </summary>
        /// <param name="response"> The response to populate this instance with. </param>
        public RequestResult(IRestResponse<T> response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));
            StatusCode = response.StatusCode;
            Data = response.Data;
        }
        /// <summary> Creates a new <see cref="RequestResult{T}" /> instance. </summary>
        /// <param name="response"> The response to populate this instance with. </param>
        /// <param name="data"> The data for this instance. </param>
        public RequestResult(IRestResponse response, T data)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));
            StatusCode = response.StatusCode;
            Data = data;
        }
    }
}