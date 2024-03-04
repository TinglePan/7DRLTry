namespace Proj7DRL.scripts.data_binding;

public interface IObservableProperty<out T>: INotifyValueChanged
{
    public string Name { get; }
    public T Value { get; }
}

public struct ValueChangedEventArgs
{
    public string PropertyName;
}

public struct ValueChangedEventDetailedArgs<T>
{
    public string PropertyName;
    public T OldValue;
    public T NewValue;
}