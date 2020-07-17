using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using Log = RepeatingWords.LoggerService.Log;

namespace RepeatingWords.Services
{
    public interface IGoogleDriveService
    {
        Task<bool> GetBackupAsync(GoogleOAuthToken oAuthToken, string folderName, string fileStartName, IImport import);
    }
   public class GoogleDriveService:IGoogleDriveService
   {
       
       private const string ENTRY_POINT_GDRIVE = "https://www.googleapis.com/drive/";
       private const string VERSION_API_V2 = "v2/";
       private const string VERSION_API_V3 = "v3/";

        public async Task<bool> GetBackupAsync(GoogleOAuthToken oAuthToken, string folderName, string fileStartName, IImport import)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(oAuthToken.TokenType, oAuthToken.AccessToken);
                var jsonStr = await httpClient.GetStringAsync(ENTRY_POINT_GDRIVE+ VERSION_API_V3+"files");
                var json = (JObject)JsonConvert.DeserializeObject(jsonStr);
                var metaFilesList = (JArray)json["files"];
                var isExistFolder = from meta in metaFilesList where meta["name"].ToString() == folderName select meta["id"].ToString();
                if (isExistFolder.Any())
                {
                    var lastFileMeta = (metaFilesList.Where(meta =>
                            meta["mimeType"].ToString().Equals("application/json") &&
                            meta["name"].ToString().StartsWith(fileStartName))
                        .Select(meta => new MetaFileGoogle { Id = meta["id"].ToString(), CreateDateTime = ParseDateTimeFromFileName( meta["name"].ToString(), fileStartName ) })).OrderBy(x => x.CreateDateTime).LastOrDefault();
                    var contentFile = await httpClient.GetStringAsync(ENTRY_POINT_GDRIVE + VERSION_API_V2 + "files/" + lastFileMeta.Id + "?alt=media&source=downloadUrl");
                    byte[] bytes = Convert.FromBase64String(contentFile.ToString());
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

            private DateTime ParseDateTimeFromFileName(string input, string fileStartName)
            {
                DateTime newDate;
                if (DateTime.TryParseExact(input.Replace(fileStartName, "").Replace(".json", ""), "ddMMyyyy_hhmm", null, DateTimeStyles.None, out newDate))
                    return newDate;
                Log.Logger.Error("Error parse datetime from file name: "+input);
                return DateTime.MinValue;
            }
      
    }

   internal class MetaFileGoogle
   {
       internal DateTime CreateDateTime { get; set; }
       internal string Id { get; set; }
    }
}
