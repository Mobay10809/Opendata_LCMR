namespace OpendataApi_LCMR.Services
{
    public class DBService
    {
        public string ConnectionString { get; }

        public DBService(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
