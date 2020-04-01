using System.Runtime.Serialization;

namespace Matrix.TaskManager.Common.Configuration
{
    [DataContract(Name = "network_proxy_configuration")]
    public class NetworkProxyConfiguration
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "proxy_address")]
        public string ProxyAddress { get; set; }

        [DataMember(Name = "port")]
        public int Port { get; set; }

        [DataMember(Name = "requires_authentication")]
        public bool RequiresAuthentication { get; set; }

        [DataMember(Name = "proxy_username")]
        public string ProxyUsername { get; set; }

        [DataMember(Name = "proxy_user_password")]
        public string ProxyUserPassword { get; set; }

    }
}
