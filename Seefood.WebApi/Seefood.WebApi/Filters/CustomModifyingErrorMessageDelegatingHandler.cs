using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Seefood.WebApi.Filters
{
    public class CustomModifyingErrorMessageDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>((responseToCompleteTask) =>
            {
                HttpResponseMessage response = responseToCompleteTask.Result;

                HttpError error = null;
                if (response.TryGetContentValue<HttpError>(out error))
                {
                    error.Message = "Có lỗi xảy ra";
                    error.MessageDetail = "Có lỗi xảy ra";
                }
                return response;
            });
        }
    }
}