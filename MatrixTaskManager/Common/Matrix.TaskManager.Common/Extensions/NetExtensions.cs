using Matrix.TaskManager.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Matrix.TaskManager.Common.Extensions
{
    public static class NetExtensions
    {

        public static WebClient WebClient(NetworkProxyConfiguration networkProxy=null)
        {
            var webClient = new WebClient();

            if (networkProxy != null)
            {
                if (networkProxy.RequiresAuthentication &&
                    !string.IsNullOrWhiteSpace(networkProxy.ProxyAddress) &&
                    !string.IsNullOrWhiteSpace(networkProxy.ProxyUsername) &&
                    !string.IsNullOrWhiteSpace(networkProxy.ProxyUserPassword) &&
                    networkProxy.Port > 0 && 
                    networkProxy.Port < 65536)
                {
                    WebProxy proxy = new WebProxy(networkProxy.ProxyAddress, networkProxy.Port);                    
                    proxy.Credentials = new NetworkCredential(networkProxy.ProxyUsername, 
                        networkProxy.ProxyUserPassword);
                    proxy.UseDefaultCredentials = false;
                    proxy.BypassProxyOnLocal = false;  //still use the proxy for local addresses
                    webClient.Proxy = proxy;
                }
            }

            return webClient;

        }
    }
}
