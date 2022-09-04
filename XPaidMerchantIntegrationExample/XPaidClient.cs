using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using XPaidMerchantIntegrationExample.Models;

namespace XPaidMerchantIntegrationExample
{
    public class XPaidClient
    {
        private const string HostUrl = "https://localhost:44354";

        private const string ApiKey = "22237f2a-655c-4e70-b346-a24024533b56";
        private const string Secret = "6dedd1caecfd11e8447827af1e638e61d0c29a1290eee1d8d32b000e0200ec9b";

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
