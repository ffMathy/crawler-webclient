using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public static class Extensions
{
    static string[] RestrictedHeaders = new string[] {
            "Accept",
            "Connection",
            "Content-Length",
            "Content-Type",
            "Date",
            "Expect",
            "Host",
            "If-Modified-Since",
            "Range",
            "Referer",
            "Transfer-Encoding",
            "User-Agent",
            "Proxy-Connection"
    };

    static Dictionary<string, PropertyInfo> HeaderProperties = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

    static Extensions()
    {
        var type = typeof(HttpWebRequest);
        foreach (string header in RestrictedHeaders)
        {
            string propertyName = header.Replace("-", "");
            var headerProperty = type.GetProperty(propertyName);
            HeaderProperties[header] = headerProperty;
        }
    }

    public static void SetRawHeader(this HttpWebRequest request, string name, string value)
    {
        if (HeaderProperties.ContainsKey(name))
        {
            HeaderProperties[name].SetValue(request, value, null);
        }
        else
        {
            request.Headers[name] = value;
        }
    }
}