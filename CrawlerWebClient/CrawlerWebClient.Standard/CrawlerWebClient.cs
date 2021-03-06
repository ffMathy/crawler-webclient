﻿using System;
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

    private string lastUrl;

    public CrawlerWebClient()
    {
        container = new CookieContainer();

        lastUrl = null;
    }

    public async Task<string> GetAsync(string uri, string parameters = null, string referer = null, string userAgent = null)
    {
        if (referer == null && lastUrl != null)
        {
            referer = lastUrl;
        }

        if (userAgent == null)
        {
            userAgent = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";
        }

        var request = WebRequest.CreateHttp(uri + (parameters == null ? "" : "?" + parameters));
        request.CookieContainer = container;
        request.Method = "GET";

        request.Referer = referer;
        request.UserAgent = userAgent;

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

        lastUrl = uri;

        return result;
    }

    public Task<string> PostAsync(string uri, string body = null, string referer = null, string userAgent = null)
    {
        throw new NotImplementedException();
    }
}