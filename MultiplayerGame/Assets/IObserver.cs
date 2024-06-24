public interface IObserver
{
    void UpdateObserver(ISubject subject);
    void HandleObjectState(bool pEnabled);
}