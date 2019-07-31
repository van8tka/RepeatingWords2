using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RepeatingWords.Model;
using System;
using RepeatingWords.DataService.Model;
using RepeatingWords.LoggerService;
using System.Net;
using System.Text;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.Services
{
    internal class CustomWebClient
    {
        public HttpWebRequest CreateRequest(Uri uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.Timeout = 30000;
            return request;
        }

        public HttpWebResponse CreateResponse(HttpWebRequest request)
        {
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                if (response == null)
                    throw;
            }
            finally
            {
                if (response != null)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new WebException("Response answer is not OK");
                    if ((int)response.StatusCode >= 500)
                    {
                        response.Close();
                        throw new WebException(string.Format("Client received error response from server. Status code: {0}.", response.StatusCode), WebExceptionStatus.ReceiveFailure);
                    }
                }
            }
            return response;
        }
        public HttpWebResponse CreateResponse(HttpWebRequest request, out string responseData)
        {
            HttpWebResponse response = CreateResponse(request);
            StringBuilder sb = new StringBuilder();
            using (var stream = response.GetResponseStream())
            {
                int readCount;
                int count = 1024;
                byte[] buffer = new byte[count];
                while ((readCount = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, readCount));
                }
            }
            responseData = sb.ToString();
            return response;
        }
    }





    internal class OnlineDictionaryService : IWebApiService
    {
        private const string Url = "http://devprogram.ru/api/data/";
        private const string UrlLang = "http://devprogram.ru/api/langdata/";
 
        //получаем список словарей
        public async Task<IEnumerable<Dictionary>> Get()
        {           
            try
            {
                return await GetData<Dictionary>(new Uri(Url));
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }

      
        //получаем список слов выбранного словаря
        public async Task<IEnumerable<Words>> Get(int idDict)
        {
         
            try
            {
                return await GetData<Words>(new Uri(Url + idDict));
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }
        //---------------------------обновление с выбором языковой среды-------------------------
        //получаем список языков
        public async Task<IEnumerable<Language>> GetLanguage()
        {
            try
            {
                return await GetData<Language>(new Uri(UrlLang));                
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }
        // получаем список словарей выбранного языка
        public async Task<IEnumerable<Dictionary>> GetLanguage(int idLang)
        {          
            try
            {
                return await GetData<Dictionary>(new Uri(UrlLang + idLang));
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }
        /// <summary>
        /// обобщенный метод получения данных по сети
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        private async Task<IEnumerable<T>> GetData<T>(Uri uri) where T : class
        {
            try
            {
                var client = new CustomWebClient();
                var request = client.CreateRequest(uri);
                string data;
                client.CreateResponse(request, out data);
                if (!string.IsNullOrEmpty(data))
                {
                    IEnumerable<T> items = JsonConvert.DeserializeObject<IEnumerable<T>>(data);
                    return items;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }
    }
}
