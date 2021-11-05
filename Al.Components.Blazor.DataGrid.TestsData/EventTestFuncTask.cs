using System.Reflection;

namespace Al.Components.Blazor.DataGrid.TestsData
{
    public class EventTestFuncTask<T> : IDisposable
    {
        readonly T _instance;
        readonly EventInfo? _eventInfo;
        public bool CallEvent { get; private set; }



        public EventTestFuncTask(T instance, string eventName)
        {
            _instance = instance;
            var type = typeof(T);
            _eventInfo = type.GetEvent(eventName);
            _eventInfo.AddEventHandler(instance, Handler);
        }

        Task Handler()
        {
            CallEvent = true;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _eventInfo.RemoveEventHandler(_instance, Handler);
        }
    }
}
