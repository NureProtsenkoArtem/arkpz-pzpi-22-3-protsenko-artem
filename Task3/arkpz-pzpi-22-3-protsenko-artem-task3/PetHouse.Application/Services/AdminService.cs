using System.Diagnostics;
using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;

namespace PetHouse.Application.Services;

public class AdminService : IAdminService
{
   private readonly string _connectionString;

   public AdminService(string? connectionString)
   {
      _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
   }

   // Creates a backup of the PostgreSQL database.
   // - Ensures the output directory exists.
   // - Uses `pg_dump` to generate a compressed database backup file.
   // - Throws an exception if the backup process fails.
   public async Task<string> BackupData(string? outputDirectory)
   {
      try
      {
         outputDirectory ??= Path.GetTempPath();
         if (!Directory.Exists(outputDirectory))
         {
            Directory.CreateDirectory(outputDirectory);
         }

         string backupFileName = $"backup_{DateTime.Now:yyyyMMddHHmmss}.sql";
         string backupFilePath = Path.Combine(outputDirectory, backupFileName);


         string pgDumpPath = "\"C:\\Program Files\\PostgreSQL\\16\\bin\\pg_dump.exe\"";

         var startInfo = new ProcessStartInfo
         {
            FileName = pgDumpPath,
            Arguments = GetPgDumpArguments(backupFilePath),
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
         };

         using (var process = new Process { StartInfo = startInfo })
         {
            process.Start();
            string errorOutput = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
               throw new ApiException($"Backup creation error: {errorOutput}", 500);
            }
         }

         return backupFilePath;
      }
      catch (Exception ex)
      {
         throw new ApiException($"An error occurred while creating the backup: {ex.Message}", 500);
      }
   }

   // Restores a PostgreSQL database from a given backup file.
   // - Validates the backup file path.
   // - Extracts connection details (host, port, database, user, password) from the connection string.
   // - Uses `pg_restore` to restore the database.
   // - Throws an exception if the restoration process fails.
   public async Task RestoreDataAsync(string backupFilePath)
   {
      try
      {
         if (!File.Exists(backupFilePath))
         {
            throw new FileNotFoundException("Backup file not found.");
         }

         var parameters =
            _connectionString
               .Split(';')
               .Where(param => !string.IsNullOrEmpty(param))
               .Select(param => param.Split('='))
               .ToDictionary(param => param[0].Trim(), param => param[1].Trim());

         string host = parameters["Host"];
         string port = parameters["Port"];
         string database = parameters["Database"];
         string user = parameters["Username"];
         string password = parameters["Password"];

         Environment.SetEnvironmentVariable("PGPASSWORD", password);

         string pgRestorePath = "\"C:\\Program Files\\PostgreSQL\\16\\bin\\pg_restore.exe\"";

         var restoreStartInfo = new ProcessStartInfo
         {
            FileName = pgRestorePath,
            Arguments = $"-h {host} -p {port} -U {user} -d {database} --clean -v \"{backupFilePath}\"",
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
         };

         using (var process = new Process { StartInfo = restoreStartInfo })
         {
            process.Start();
            string errorOutput = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
               throw new ApiException($"Restore failed: {errorOutput}", 500);
            }
         }
      }
      catch (Exception ex)
      {
         throw new ApiException($"An error occurred during database restore: {ex.Message}", 500);
      }
   }

   // Constructs the argument string required for the `pg_dump` command.
   // - Extracts PostgreSQL connection parameters from the connection string.
   // - Formats the arguments to include host, port, user, and database details.
   private string GetPgDumpArguments(string outputPath)
   {
      var builder = new Npgsql.NpgsqlConnectionStringBuilder(_connectionString);

      string? host = builder.Host;
      string? database = builder.Database;
      string? user = builder.Username;
      string? password = builder.Password;
      string? port = builder.Port.ToString();

      Environment.SetEnvironmentVariable("PGPASSWORD", password);

      return $"-h {host} -p {port} -U {user} -F c -b -v -f \"{outputPath}\" {database}";
   }
}