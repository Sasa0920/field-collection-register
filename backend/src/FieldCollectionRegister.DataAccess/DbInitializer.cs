namespace FieldCollectionRegister.DataAccess;

// Creates the SQLite schema and inserts a handful of fictional Regions,
// Estates and Fields if the database is empty. Everything here is
// invented sample data - no real company data of any kind.
public static class DbInitializer
{
    public static void Initialize(SqliteConnectionFactory factory)
    {
        using var connection = factory.CreateConnection();
        connection.Open();

        using var createCmd = connection.CreateCommand();
        createCmd.CommandText = """
            CREATE TABLE IF NOT EXISTS Regions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Estates (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                RegionId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                FOREIGN KEY (RegionId) REFERENCES Regions(Id)
            );

            CREATE TABLE IF NOT EXISTS Fields (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EstateId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                FOREIGN KEY (EstateId) REFERENCES Estates(Id)
            );

            CREATE TABLE IF NOT EXISTS Entries (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FieldId INTEGER NOT NULL,
                CollectionDate TEXT NOT NULL,
                Quantity REAL NOT NULL,
                Note TEXT NULL,
                FOREIGN KEY (FieldId) REFERENCES Fields(Id)
            );
            """;
        createCmd.ExecuteNonQuery();

        using var countCmd = connection.CreateCommand();
        countCmd.CommandText = "SELECT COUNT(*) FROM Regions;";
        var regionCount = (long)(countCmd.ExecuteScalar() ?? 0L);

        if (regionCount > 0)
        {
            return; // already seeded
        }

        using var seedCmd = connection.CreateCommand();
        seedCmd.CommandText = """
            INSERT INTO Regions (Name) VALUES ('Uva'), ('Sabaragamuwa');

            INSERT INTO Estates (RegionId, Name) VALUES
                (1, 'Wellawaya Estate'),
                (1, 'Badulla Estate'),
                (2, 'Ratnapura Estate');

            INSERT INTO Fields (EstateId, Name) VALUES
                (1, 'Field A1'),
                (1, 'Field A2'),
                (2, 'Field B1'),
                (3, 'Field C1'),
                (3, 'Field C2');
            """;
        seedCmd.ExecuteNonQuery();
    }
}
