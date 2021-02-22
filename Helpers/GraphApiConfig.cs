using System;
using System.Globalization;

namespace GraphiAPI.Helpers
{
    public class GraphApiConfig
    {
        public string Instance { get; set; } = "https://login.microsoftonline.com/{0}";
        public string ApiUrl { get; set; } = "https://graph.microsoft.com/";
        public string Tenant { get; set; }
        public string ClientId { get; set; }
        public string Authority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Instance, Tenant);
            }
        }
        public string ClientSecret { get; set; }
        public string CertificateName { get; set; }
    }
}
