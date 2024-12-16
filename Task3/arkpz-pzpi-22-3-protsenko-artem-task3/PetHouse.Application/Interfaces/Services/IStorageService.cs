namespace PetHouse.Application.Interfaces.Services;

public interface IStorageService
{
   Task<string> UploadFileAsync(string id, string key, Stream fileStream, string contentType);
}