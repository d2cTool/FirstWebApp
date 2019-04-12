namespace FirstWebApp.Repositories
{
    public class BaseRepository
    {
        protected readonly string _connectionString;
        public BaseRepository()
        {
            _connectionString = "Data Source=app.db";
        }
    }
}
