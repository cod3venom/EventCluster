using EventCluster.EventBus.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCluster.Module.Controller.Attribute
{
    public class Endpoint : System.Attribute
    {
        public string Name;
        public string Version;

        public Endpoint(string name, string version = "1.0")
        {
            this.Name = name;
            this.Version = version;
        }
    }
}
