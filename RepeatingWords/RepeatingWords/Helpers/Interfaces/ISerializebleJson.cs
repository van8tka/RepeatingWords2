using Newtonsoft.Json.Linq;

namespace RepeatingWords.Helpers.Interfaces
{
   public interface ISerializebleJson
   {
       JObject ToJson();

       T FromJson<T>(JObject jItem) where T : class;

   }
}
