using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ResearchLinks.Tests.Mocks
{
    class MockInnerHandler : DelegatingHandler
    {
        public HttpResponseMessage Message { get; set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (Message == null) {
                return base.SendAsync(request, cancellationToken);
            }
            return Task.Factory.StartNew(() => Message);
        }
    }
}
