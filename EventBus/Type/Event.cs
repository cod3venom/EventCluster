using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EventCluster.EventBus.Type
{
    [Serializable]
    public class Event
    {
        [JsonProperty]
        public string Id => Guid.NewGuid().ToString();
        
        [JsonProperty]
        public string Topic { get; private set; }
       
        [JsonProperty]
        public dynamic Data { get; private set; }

        [JsonProperty]
        public dynamic PlainData { get; set; }

        [JsonProperty]
        public Dictionary<string, object> Headers { get; private set; }

        public Event()
        {
        }

        [JsonConstructor]
        public Event(string topic, dynamic data)
        {
            this.Topic = topic;
            this.Data = data;
            this.Headers = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            string str = JsonConvert.SerializeObject(this, Formatting.None);
            return str;
        }

        public byte[] ToByte()
        {
            return Encoding.UTF8.GetBytes(this.ToString());
        }
        public static Event FromJSON(string json)
        {
            try
            {
                Event @event = JsonConvert.DeserializeObject<Event>(json);
                return @event;
            }
            catch(JsonReaderException)
            {
                Event @event = new Event();
                @event.PlainData = json;
                return @event;
            }
            catch (JsonSerializationException)
            {
                Event @event = new Event();
                @event.PlainData = json;
                return @event;
            }
            catch(Exception)
            {
                Event @event = new Event();
                @event.PlainData = json;
                return @event;
            }
        }
    }
}
