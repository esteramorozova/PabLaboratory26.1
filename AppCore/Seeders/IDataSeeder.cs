namespace AppCore.Seeders;

public interface IDataSeeder
{
    public int Order { get; }
    Task SeedAsync();
}