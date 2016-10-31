using System.Threading.Tasks;
using Abp.Dependency;
using Jueci.WeChatApp.RestfulRequestTool.Common;

namespace Jueci.WeChatApp.RestfulRequestTool.RequestAbstractions
{
    /// <summary> Provides a simple wrapper around RESTful requests. </summary>
	public interface IRequest : ITransientDependency
    {
        /// <summary> Adds a parameter to the request. </summary>
        /// <param name="name"> The name of the parameter. </param>
        /// <param name="value"> The value for the parameter. </param>
        void AddParameter(string name, object value);

        /// <summary> Adds a parameter to the request if value is not null. </summary>
        /// <param name="name"> The name of the parameter. </param>
        /// <param name="value"> The value for the parameter. </param>
        void AddParameterIfNotNull(string name, object value);

        /// <summary> Adds a url segment to the request. </summary>
        /// <param name="name"> The name of the url segment. </param>
        /// <param name="value"> The value for the url segment. </param>
        void AddUrlSegment(string name, object value);

        /// <summary> Adds a url segment to the request if value is not null. </summary>
        /// <param name="name"> The name of the url segment. </param>
        /// <param name="value"> The value for the url segment. </param>
        void AddUrlSegmentIfNotNull(string name, object value);

        /// <summary> Executes the request and returns the response deserialized into the specified type. </summary>
        /// <typeparam name="T"> The type to deserialize the response into. </typeparam>
        /// <returns> The response. </returns>
        Task<RequestResult<T>> Execute<T>() where T : new();

        /// <summary> Executes the request and returns the response as a byte array. </summary>
        /// <returns> The response containing a byte array. </returns>
        Task<RequestResult<byte[]>> ExecuteBytes();

        /// <summary> Executes the request and returns the contents of the response. </summary>
        /// <returns> The response containing the contents of the response as a string. </returns>
        Task<RequestResult<string>> ExecuteContent();

        /// <summary>
        ///     Executes the request and returns the response deserialized into the specified type along with pagination
        ///     data.
        /// </summary>
        /// <typeparam name="T"> The type to deserialize the response into. </typeparam>
        /// <returns> The response. </returns>
        Task<PaginatedResult<T>> ExecutePaginated<T>() where T : new();
    }
}
