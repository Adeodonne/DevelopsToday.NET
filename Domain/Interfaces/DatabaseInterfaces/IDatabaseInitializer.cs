namespace Domain.Interfaces;

public interface IDatabaseInitializer
{
    void EnsureDatabaseExists();
}