using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using PetHouse.API.Contracts.Storage;
using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;

namespace PetHouse.Application.Services;

public class StorageService : IStorageService
{
   private readonly IAmazonS3 _s3Client;
   private readonly AwsOptions _options;

   public StorageService(IAmazonS3 s3Client, IOptions<AwsOptions> options)
   {
      _s3Client = s3Client;
      _options = options.Value;
   }

   // Uploads a file to the specified S3 bucket and returns the file's URL.
   public async Task<string> UploadFileAsync(string id, string key, Stream fileStream, string contentType)
   {
      // Configure the upload request with bucket details, file key, and metadata.
      var request = new PutObjectRequest
      {
         BucketName = _options.BucketName,
         Key = "recognizable_data/" + $"{id}/" + DateTime.Now.ToString("HH:mm:ss") + key,
         InputStream = fileStream,
         ContentType = contentType,
         CannedACL = S3CannedACL.BucketOwnerFullControl
      };

      try
      {
         // Attempt to upload the file to S3.
         var response = await _s3Client.PutObjectAsync(request);

         // If the upload is successful, return the file's public URL.
         if (response.HttpStatusCode == HttpStatusCode.OK)
         {
            return $"https://pethousebucket.s3.eu-north-1.amazonaws.com/{request.Key}";
         }

         // If upload fails, throw an exception.
         throw new Exception("File upload failed");
      }
      catch (AmazonS3Exception e)
      {
         // Handle S3-specific errors and wrap them in a custom API exception.
         throw new ApiException(e.Message, 500);
      }
      catch (Exception e)
      {
         // Handle generic exceptions and wrap them in a custom API exception.
         throw new ApiException(e.Message, 500);
      }
   }
}