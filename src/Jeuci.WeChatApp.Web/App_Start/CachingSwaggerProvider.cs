using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swashbuckle.Swagger;

namespace Jeuci.WeChatApp.Web
{
    internal class CachingSwaggerProvider : ISwaggerProvider
    {
        private ISwaggerProvider defaultProvider;

        public CachingSwaggerProvider(ISwaggerProvider defaultProvider)
        {
            this.defaultProvider = defaultProvider;
        }

        public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
        {
            var sd = defaultProvider.GetSwagger(rootUrl, apiVersion);

            var rmPaths = new List<string>();

            foreach (var path in sd.paths)
            {
                if (path.Key.Contains("Abp") || path.Key.Contains("Proxies") || path.Key.Contains("services"))
                {
                    rmPaths.Add(path.Key);
                }
            }
            foreach (var rmpath in rmPaths)
            {
                if (sd.paths.ContainsKey(rmpath))
                {
                    sd.paths.Remove(rmpath);
                }

            }

            return sd;
        }
    }
}