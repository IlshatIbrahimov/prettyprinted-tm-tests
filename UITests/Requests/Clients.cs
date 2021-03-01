using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests.Requests
{
    public class Clients
    {
        public static string BackendUrl { get; } = "https://prettyprinted-tm-backend.herokuapp.com/";
        public static string UserToken { get; set; } = "";
        
        public static RestClient BackendClient { get; } = new RestClient(BackendUrl)
        {
            FollowRedirects = true,
            CookieContainer = new System.Net.CookieContainer(),
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36"
        };

        public static void Init()
        {
            BackendClient.AddDefaultHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            BackendClient.AddDefaultHeader("Accept-Encoding", "gzip, deflate");
            BackendClient.AddDefaultHeader("Accept", "*/*");
        }
        
        public static void AddAuthorizationToken(RestRequest request)
        {
            request.AddHeader("Authorization", UserToken);
        }
    }
}
