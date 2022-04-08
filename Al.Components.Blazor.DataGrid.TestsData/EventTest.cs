using System.Reflection;

namespace Al.Components.Blazor.DataGrid.TestsData
{
    public class EventTest<T> : IDisposable
    {
        readonly T _instance;
        readonly Delegate _delegat;
        readonly EventInfo? _eventInfo;

        public EventTest(T instance, string eventName, Delegate delegat)
        {
            _instance = instance;
            var type = typeof(T);
            _delegat = delegat;
            _eventInfo = type.GetEvent(eventName);
            _eventInfo.AddEventHandler(instance, delegat);
        }

        public void Dispose()
        {
            _eventInfo.RemoveEventHandler(_instance, _delegat);
        }
    }
}
