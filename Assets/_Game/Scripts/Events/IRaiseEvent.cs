#nullable enable

namespace _Game.Scripts.Events
{
    /// <summary>
    ///     This interface is a blueprint for scriptable objects that behave like events.
    /// </summary>
    public interface IRaiseEvent
    {
        public void OnEnable();

        public void RaiseEvent();
    }
    
    /// <summary>
    ///     This interface is a blueprint for scriptable objects that behave like events,
    ///     and can send data through those events.
    /// </summary>
    /// <typeparam name="T">Data passed with the event</typeparam>
    // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/creating-variant-generic-interfaces
    public interface IRaiseEvent<in T>
    {
        public void OnEnable();
        public void RaiseEvent(T data);
    }
}