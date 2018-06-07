using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace Core
{
    public class RestClientService
    {
        RestSharp.RestClient _client;
        public RestClientService()
        {
            _client = new RestSharp.RestClient("http://34.241.188.161:5000/");
        }
        public string SendLockRequest()
        {
            var request = new RestRequest("lock", Method.GET);
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            IRestResponse response =_client.Execute(request);
            return response.Content;
        }

        public string SendUnlockRequest()
        {
            var request = new RestRequest("unlock", Method.GET);
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            IRestResponse response = _client.Execute(request);
            return response.Content;
        }
    }
}