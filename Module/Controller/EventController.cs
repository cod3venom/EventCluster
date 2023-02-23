using EventCluster.EventBus.Type;
using EventCluster.Module.Controller.Attribute;
using EventCluster.Module.Controller.Interface;
using System.Linq;
using System.Reflection;

namespace EventCluster.Module.Controller
{
    public class EventController
    {
        private readonly IEventController _controller;
        private readonly EventConsumer _consumer;
        public EventController(IEventController controller, EventConsumer consumer) 
        {
            this._controller = controller;
            this._consumer = consumer;
            this.HookController();
        }

        private void HookController()
        {
            MethodInfo[] methods = this.GetMEthods();
            foreach(MethodInfo method in methods)
            {
                Endpoint attr = this.GetAttributes(method);

                if (attr.Name == "") {
                    continue;
                }
                this._consumer.On(attr.Name, (Event @event) => {
                    method.Invoke(this._controller,  new object[] { @event} );
                });
            }
        }

        private MethodInfo[] GetMEthods ()
        {
            MethodInfo[] methods = this._controller.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            return methods;
        }

        private Endpoint GetAttributes(MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(true).OfType<Endpoint>().FirstOrDefault();
            if (attributes == null) {
                return new Endpoint("");
            }
            return attributes;
        }
    }
}
