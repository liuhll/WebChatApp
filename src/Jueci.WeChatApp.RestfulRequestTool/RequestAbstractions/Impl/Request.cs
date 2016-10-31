using System.Collections.Generic;
using System.Threading.Tasks;
using Jueci.WeChatApp.RestfulRequestTool.Common;
using RestSharp;
using JueciMethod = Jueci.WeChatApp.RestfulRequestTool.Common.Enums;

namespace Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions.Impl
{
    internal class Request : IRequest
    {
        private readonly IRestClient _client;
        private readonly IRestRequest _request;

        public Request(string resource, JueciMethod.Method method, IRestClient client)
        {
            _client = client;
            switch (method)
            {
                case JueciMethod.Method.Delete:
                    _request = new RestSharp.RestRequest(resource, RestSharp.Method.DELETE);
                    break;
                case JueciMethod.Method.Get:
                    _request = new RestSharp.RestRequest(resource, RestSharp.Method.GET);
                    break;
                case JueciMethod.Method.Head:
                    _request = new RestSharp.RestRequest(resource, RestSharp.Method.HEAD);
                    break;
                case JueciMethod.Method.Merge:
                    _request = new RestSharp.RestRequest(resource, RestSharp.Method.MERGE);
                    break;
                case JueciMethod.Method.Options:
                    _request = new RestSharp.RestRequest(resource, RestSharp.Method.OPTIONS);
                    break;
                case JueciMethod.Method.Patch:
                    _request = new RestSharp.RestRequest(resource, RestSharp.Method.PATCH);
                    break;
                case JueciMethod.Method.Post:
                    _request = new RestSharp.RestRequest(resource, RestSharp.Method.POST);
                    break;
                case JueciMethod.Method.Put:
                    _request = new RestSharp.RestRequest(resource, RestSharp.Method.PUT);
                    break;
            }
        }
        public void AddParameter(string name, object value)
        {
            _request.AddParameter(name, value);
        }
        public void AddParameterIfNotNull(string name, object value)
        {
            if (value != null)
                _request.AddParameter(name, value);
        }
        public void AddUrlSegment(string name, object value)
        {
            _request.AddParameter(name, value, ParameterType.UrlSegment);
        }
        public void AddUrlSegmentIfNotNull(string name, object value)
        {
            if (value != null)
                _request.AddParameter(name, value, ParameterType.UrlSegment);
        }
        public async Task<RequestResult<T>> Execute<T>() where T : new()
        {
            var result = await _client.ExecuteTaskAsync<T>(_request);
            return new RequestResult<T>(result);
        }
        public async Task<RequestResult<byte[]>> ExecuteBytes()
        {
            var result = await _client.ExecuteTaskAsync(_request);
            return new RequestResult<byte[]>(result, result.RawBytes);
        }
        public async Task<RequestResult<string>> ExecuteContent()
        {
            var result = await _client.ExecuteTaskAsync(_request);
            return new RequestResult<string>(result, result.Content);
        }
        public async Task<PaginatedResult<T>> ExecutePaginated<T>() where T : new()
        {
            var result = await _client.ExecuteTaskAsync<List<T>>(_request);
            return new PaginatedResult<T>(result);
        }
    }
}
