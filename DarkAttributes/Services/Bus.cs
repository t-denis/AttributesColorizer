using Caliburn.Micro;
using Microsoft.VisualStudio.Text.Classification;

namespace DarkAttributes.Services
{
    /// <summary>
    /// Event aggregator. It delegates calls to Caliburn.Micro, that implements the pattern 
    /// using weak references (no need to unsubscribe). Basically the bus is used because
    /// <see cref="IClassifier"/> doesn't implement IDisposable, so we can't unsubscribe.
    /// </summary>
    public static class Bus
    {
        private static readonly IEventAggregator EventAggregator = new EventAggregator();

        public static void Publish(object message)
        {
            EventAggregator.Publish(message);
        }

        public static void Subscribe(IHandle subscriber)
        {
            EventAggregator.Subscribe(subscriber);
        }
    }
}
