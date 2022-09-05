using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using XPaidMerchantIntegrationExample.Models;

namespace XPaidMerchantIntegrationExample
{
    public class XPaidClient
    {
        private const string HostUrl = "https://api.exchangepaid.com";

        private const string ApiKey = "";
        private const string Secret = "";

        public InvoiceDTO GetInvoice(string externalId)
        {
            return SendGet<InvoiceDTO>($"invoice/getByExternalId?externalId={externalId}");
        }

        public InvoiceDTO CreateInvoice(decimal amount, string currency, string externalId, string callbackUrl)
        {
            return SendPost<InvoiceDTO>("invoice/create", new
            {
                externalId = externalId,
                amount = amount,
                currency = currency,
                callbackUrl = callbackUrl
            });
        }

        private T SendPost<T>(string path, object body)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{HostUrl}/{path}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var bodyJson = JsonConvert.SerializeObject(body);

            SignRequest(httpWebRequest, path, bodyJson);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(bodyJson);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var response = JsonConvert.DeserializeObject<T>(result);
            return response;
        }

        private T SendGet<T>(string path)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{HostUrl}/{path}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            SignRequest(httpWebRequest, path);

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var response = JsonConvert.DeserializeObject<T>(result);
            return response;
        }

        private void SignRequest(HttpWebRequest httpWebRequest, string path, string bodyJson = null)
        {
            var payloadJson = JsonConvert.SerializeObject(new
            {
                request = path,
                data = bodyJson != null ? Convert.ToBase64String(Encoding.UTF8.GetBytes(bodyJson)) : null
            });
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(Secret));

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payloadBase64));

            var signature = Convert.ToHexString(computedHash);

            httpWebRequest.Headers.Add("X-TXC-APIKEY", ApiKey);
            httpWebRequest.Headers.Add("X-TXC-SIGNATURE", signature);
            httpWebRequest.Headers.Add("X-TXC-PAYLOAD", payloadBase64);
        }
    }
}
