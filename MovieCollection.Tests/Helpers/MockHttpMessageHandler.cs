using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCollection.Tests.Helpers
{
    internal class MockHttpMessageHandler : HttpMessageHandler
    {
        public HttpResponseMessage DefaultResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        public List<HttpRequestMessage> RequestMessages = new List<HttpRequestMessage>();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestMessages.Add(request);
            return Task.FromResult(DefaultResponse);
        }
    }
}
