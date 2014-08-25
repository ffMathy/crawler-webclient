using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class CrawlerWebClient
{
    private CookieContainer container;

    public CrawlerWebClient()
    {
        container = new CookieContainer();
    }

    public async Task<string> GetAsync(string uri, string parameters = null, string referrer = null, string userAgent = null)
    {
        var request = WebRequest.CreateHttp(uri + (parameters == null ? "" : "?" + parameters));
        request.CookieContainer = container;
        request.Method = "GET";

        var headers = request.Headers;
        headers[HttpRequestHeader.Referer] = referrer;
        headers[HttpRequestHeader.UserAgent] = userAgent;

        var downloading = true;
        string result = null;

        request.BeginGetResponse(
            new AsyncCallback(async delegate (IAsyncResult ar)
            {

                using (var response = request.EndGetResponse(ar))
                using (var stream = response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    result = await streamReader.ReadToEndAsync();
                }

                downloading = false;

            })
            , null);

        while (downloading)
        {
            await Task.Delay(100);
        }

        return result;
    }

    public Task<string> PostAsync(string uri, string body = null, string referer = null, string userAgent = null)
    {
        throw new NotImplementedException();
    }
}