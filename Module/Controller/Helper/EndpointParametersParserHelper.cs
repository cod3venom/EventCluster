using EventCluster.EventBus.Type;
using EventCluster.Module.Controller.Attribute;
using EventCluster.Module.Controller.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventCluster.Module.Controller.Helper
{
    public  class EndpointParametersParserHelper
    {

        private string endpoint = "";
        private StringBuilder endpointPattern ;
       
        public string BuildPattern(Endpoint endpoint)
        {
            if (endpoint == null)
            {
                return "";
            }

            this.endpointPattern = new StringBuilder();
            this.endpoint = endpoint.Name;

            string[] routeParts = endpoint.Name.Split(EndpointSpecialChars.RouterSeparator);
            routeParts.ToList().ForEach(parameter =>
            {
                if (!parameter.Contains('@')) {
                    this.endpointPattern.Append(string.Format("{0}/", parameter));
                } else {
                    this.endpointPattern.Append("\\/\\/{?([^\\/}]+)}?");
                }
            });

            Console.WriteLine(this.endpointPattern.ToString());
           
            return this.endpointPattern.ToString();
        }

        public string Match(Event @event)
        {
            Regex regex = new Regex(this.endpointPattern.ToString());
            Match match = regex.Match(@event.Topic);

            Console.WriteLine(this.endpointPattern.ToString());
            Console.WriteLine(match.Name.ToString());
            Console.WriteLine(match.Value.ToString());

            if (match.Success)
            {
                return @event.Topic;
            }

            return match.Value.ToString();
        }
    }
}
