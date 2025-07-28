namespace App.Domain.Options
{
    public class ConnectionStringOptions
    {
        public const string Key = "ConnectionStrings";
        public string SqlServer { get; set; } = default!;
    }
}
