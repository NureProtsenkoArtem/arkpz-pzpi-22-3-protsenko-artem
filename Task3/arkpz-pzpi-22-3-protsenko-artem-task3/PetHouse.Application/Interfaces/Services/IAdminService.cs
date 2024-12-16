namespace PetHouse.Application.Interfaces.Services;

public interface IAdminService
{
   Task<string> BackupData(string? outputDirectory);
   Task RestoreDataAsync(string backupFilePath);
}