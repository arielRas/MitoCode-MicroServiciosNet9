namespace FastBuy.Shared.Library.Configurations
{
    public class MongoDbSettings
    {
        public string? User {  get; set; }
        public string? Password { get; set; }
        public required string Host { get; set; }
        public required int Port { get; set; }
        public string ConnectionString => GetConnectionString();

        private string GetConnectionString()
        {
            if (string.IsNullOrWhiteSpace(User) && !string.IsNullOrWhiteSpace(Password))
                throw new ArgumentException("There is a problem in the connection string configuration.");

            if (string.IsNullOrWhiteSpace(User) && string.IsNullOrWhiteSpace(Password))
                return $"mongodb://{Host}:{Port}";

            if (!string.IsNullOrWhiteSpace(User) && string.IsNullOrWhiteSpace(Password))
                return $"mongodb://{User}@{Host}:{Port}";

            return $"mongodb://{User}:{Password}@{Host}:{Port}";
        }
    }
}