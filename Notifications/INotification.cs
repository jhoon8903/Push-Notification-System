namespace Projects.Scripts.Plugin.Notifications
{
    public interface INotification
    {
        void RequestAuthorization();
        void RegisterNotification();
        void SendNotification(string title, string body, int fireTime);
        void ScheduleNotification();
    }
}