using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using Xamarin.Forms;
using Log = RepeatingWords.LoggerService.Log;

namespace RepeatingWords.Services
{
    public interface IGoogleDriveService
    {
        Task<bool> GetBackupAsync(GoogleOAuthToken oAuthToken, string folderName, string fileStartName, IImport import);
    }

    public class GoogleDriveService : IGoogleDriveService
    {

        private const string ENTRY_POINT_GDRIVE = "https://www.googleapis.com/drive/";
        private const string VERSION_API_V3 = "v3/";
        private const string VERSION_API_V2 = "v2/";
        private const string TIME_FORMAT = "ddMMyyyy_hhmm";

        public async Task<bool> GetBackupAsync(GoogleOAuthToken oAuthToken, string fileStartName, string folderName,
            IImport import)
        {
            try
            {
                string jsonStr;
                var httpClient = new HttpClient();
                var existFolders = await GetBackupFolders(oAuthToken, folderName, httpClient);
                if (existFolders.Any())
                {
                    jsonStr = await httpClient.GetStringAsync("https://www.googleapis.com/drive/v3/files?orderBy=createdTime&q=mimeType%3D%27application%2Fjson%27%20and%20name%20contains%20%27backupcardsofwords%27");
                    var json = (JObject)JsonConvert.DeserializeObject(jsonStr);
                    var lastMetaFileId = (((JArray)json["files"]).Last() as JObject)?["id"].ToString();
                    jsonStr = await httpClient.GetStringAsync(ENTRY_POINT_GDRIVE + VERSION_API_V2 + "files/" + lastMetaFileId + "?alt=media&source=downloadUrl");
                    byte[] bytes = Convert.FromBase64String(jsonStr);
                    var jsonStrContent = JObject.Parse(Encoding.UTF8.GetString(bytes));
                    return await import.import(jsonStrContent);
                }

                return false;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                return false;
            }
        }

        private static async Task<IEnumerable<string>> GetBackupFolders(GoogleOAuthToken oAuthToken, string folderName, HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(oAuthToken.TokenType, oAuthToken.AccessToken);
            var jsonStr = await httpClient.GetStringAsync(
                "https://www.googleapis.com/drive/v3/files?q=mimeType%3D%27application%2Fvnd.google-apps.folder%27%20and%20name%3D%27CardsOfWordsBackup%27");
            var json = (JObject) JsonConvert.DeserializeObject(jsonStr);
            var metaFilesList = (JArray) json["files"];
            var existFolder = from meta in metaFilesList
                where meta["name"].ToString() == folderName
                select meta["id"].ToString();
            return existFolder;
        }

        
        public async Task<bool> SetBackupAsync(GoogleOAuthToken oAuthToken, string fileName, string folderName, IExport export)
        {
            try
            {
                var jsonContent = await export.Export();
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var existFolders = await GetBackupFolders(oAuthToken, folderName, client);
                string folderDriveId;
                if (existFolders.Any())
                    folderDriveId = await CreateGDriveItem(folderName, "application/vnd.google-apps.folder", client);
                else
                    folderDriveId = existFolders.LastOrDefault();
                //create file in folder
                string fileId = await CreateGDriveItem(fileName, "application/json", client, folderDriveId);
                //update content file(upload data)
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri("https://content.googleapis.com/drive/v3/files/"+ fileId);
                request.Method = HttpMethod.Put;
                byte[] bytes = ASCIIEncoding.UTF8.GetBytes(jsonContent.ToString(Newtonsoft.Json.Formatting.None));
                JObject body = new JObject();
                JArray parents = new JArray();
                parents.Add(folderDriveId);
                body.Add("name", fileName);
                body.Add("parents", parents);
                request.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");


                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    var answer = await responseContent.ReadAsStringAsync();
                    var jsonAnswer = (JObject)JsonConvert.DeserializeObject(answer);
                }
                return false;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                return false;
            }

        }

       

        private async Task<string> CreateGDriveItem(string name,string mimeType, HttpClient client, string parent = null)
        {
            string body;
            if (String.IsNullOrEmpty(parent))
                body = "{\"name\": \""+ name + "\", \"mimeType\": \""+ mimeType + "\"}";
            else
              body = "{\"name\": \"" + name + "\", \"mimeType\": \"" + mimeType + "\", \"parents\": [\""+parent+"\"] }";
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://content.googleapis.com/drive/v3/files"),
                Method = HttpMethod.Post,
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                var answer = await responseContent.ReadAsStringAsync();
                var jsonAnswer = (JObject)JsonConvert.DeserializeObject(answer);
                return jsonAnswer["id"].ToString();
            }
            throw new Exception("Error create folder or file in google drive? with name: "+name);
        }
    }

    internal class MetaFileGoogle
    {
        internal DateTime CreateDateTime { get; set; }
        internal string Id { get; set; }
    }
}
