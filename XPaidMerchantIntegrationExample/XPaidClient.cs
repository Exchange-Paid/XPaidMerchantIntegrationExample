using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using XPaidMerchantIntegrationExample.Models;

namespace XPaidMerchantIntegrationExample
{
    public class XPaidClient
    {
        private const string HostUrl = "https://api.xpaids.com/External";

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

            SignRequest(httpWebRequest, path);

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

        private void SignRequest(HttpWebRequest httpWebRequest, string path)
        {
            var payloadJson = JsonConvert.SerializeObject(new
            {
                request = path
            });

            var payloadBytes = Encoding.UTF8.GetBytes(payloadJson);
            var payloadBase64 = Convert.ToBase64String(payloadBytes);

            using (var rsa = RSA.Create(1024))
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(Secret), out var length);

                var signBytes = rsa.SignData(payloadBytes, 0, payloadBytes.Length,
                    HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

                var signature = Convert.ToBase64String(signBytes);

                httpWebRequest.Headers.Add("X-TXC-CLIENT", ApiKey);
                httpWebRequest.Headers.Add("X-TXC-SIGNATURE", signature);
                httpWebRequest.Headers.Add("X-TXC-PAYLOAD", payloadBase64);
            }
        }
    }
}
