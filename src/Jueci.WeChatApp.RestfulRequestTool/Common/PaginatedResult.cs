using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace Jueci.WeChatApp.RestfulRequestTool.Common
{
    public class PaginatedResult<T> : RequestResult<List<T>>
    {
        /// <summary> The current page. </summary>
        public uint? CurrentPage { get; }

        /// <summary> The number of the previous page. </summary>
        public uint? PreviousPage { get; }

        /// <summary> The number of the next page. </summary>
        public uint? NextPage { get; }

        /// <summary> The number of results returned per page. </summary>
        public uint? ResultsPerPage { get; }

        /// <summary> The total number of pages for this result. </summary>
        public uint? TotalPages { get; }

        /// <summary> The total number of results available. </summary>
        public uint? TotalResults { get; }

        public PaginatedResult(IRestResponse<List<T>> response) : base(response)
        {
            CurrentPage = GetHeaderAsUInt(response.Headers, "X-Page");
            PreviousPage = GetHeaderAsUInt(response.Headers, "X-Prev-Page");
            NextPage = GetHeaderAsUInt(response.Headers, "X-Next-Page");
            ResultsPerPage = GetHeaderAsUInt(response.Headers, "X-Per-Page");
            TotalPages = GetHeaderAsUInt(response.Headers, "X-Total-Pages");
            TotalResults = GetHeaderAsUInt(response.Headers, "X-Total");
        }

        private static uint? GetHeaderAsUInt(IEnumerable<Parameter> headers, string name)
        {
            uint result;
            var header = (string)headers.FirstOrDefault(h => h.Name == name)?.Value;
            var didParse = uint.TryParse(header, out result);
            return didParse ? result : (uint?)null;
        }

    }
}