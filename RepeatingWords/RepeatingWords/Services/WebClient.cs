﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RepeatingWords.Model;
using System;
using System.Diagnostics;
using RepeatingWords.DataService.Model;
using RepeatingWords.LoggerService;
using System.Net;
using System.Net.Http;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.Services
{
    internal class StandartWebClient
    {
        private static HttpClient _client;
        private static StandartWebClient _instance;
        private static object _lock = new object();

        private StandartWebClient()
        {
            _client = new HttpClient();
        }


        public static StandartWebClient GetInstance()
        {
            if (_instance != null)
                return _instance;
            else
            {
                lock (_lock)
                {
                    if (_instance != null)
                        return _instance;
                    else
                    {
                        _instance = new StandartWebClient();
                        return _instance;
                    }
                }
            }
        }


        public HttpRequestMessage CreateRequest(Uri uri)
        {
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = uri;
            requestMessage.Method = HttpMethod.Get;
            requestMessage.Headers.Add("Accept","application/json");
            return requestMessage;
        }
 

        public  HttpResponseMessage CreateResponse(HttpRequestMessage request)
        {
            try
            {
                return _client.SendAsync(request).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public HttpResponseMessage CreateResponse(HttpRequestMessage request, out string responseData)
        {
            try
            {
                var response = _client.SendAsync(request).GetAwaiter().GetResult();
                responseData = string.Empty;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    responseData = responseContent.ReadAsStringAsync().GetAwaiter().GetResult();
                }
                return response;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }



    public class WebClient : IWebClient
    {
        private const string Url = "http://devprogram.ru/api/data/";
        private const string UrlLang = "http://devprogram.ru/api/langdata/";
        private const string UrlWords = "http://devprogram.ru/api/words/";
        private const string UrlVersion = "http://devprogram.ru/api/version/android_cardsofwords";

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
        public async Task<string> GetLanguageWords(int idLang)
        {          
            try
            {
                return await Task.Run(() =>
                {
                   var client = StandartWebClient.GetInstance();
                    var request = client.CreateRequest(new Uri(UrlWords+idLang));
                    string data;
                    client.CreateResponse(request, out data);
                    return data;
                });
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
               return await Task.Run(() =>
                {
                    //var client = new CustomWebClient();
                    var client = StandartWebClient.GetInstance();
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
                });             
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }


        public async Task<float> GetVersionApp()
        {           
            try
            {
                return await Task.Run(() =>
                {
                    var uri = new Uri(UrlVersion);
                    var client = StandartWebClient.GetInstance();
                    var request = client.CreateRequest(uri);
                    string data;
                   var response = client.CreateResponse(request, out data);
                    if (!string.IsNullOrEmpty(data) && response.StatusCode == HttpStatusCode.OK)
                    {
                        float items = JsonConvert.DeserializeObject<float>(data);
                        return items;
                    }
                    else
                        return -1;
                });
            }
            catch (WebException e)
            {
                Log.Logger.Error(e);
                return -1;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }
    }
}
