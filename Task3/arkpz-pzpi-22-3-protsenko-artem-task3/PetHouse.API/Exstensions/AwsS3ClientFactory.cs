using Amazon;
using Amazon.Internal;
using Amazon.Runtime;
using Amazon.S3;
using PetHouse.API.Contracts.Storage;

namespace PetHouse.API.Exstensions;

public static class AwsS3ClientFactory
{
   public static IAmazonS3 CreateS3Client(IConfiguration configuration)
   {
      var awsOptions = configuration.GetSection("AWS").Get<AwsOptions>();
      var credentials = new BasicAWSCredentials(awsOptions?.AccessKey, awsOptions?.SecretKey);
      var config = new AmazonS3Config
      {
         RegionEndpoint = RegionEndpoint.EUNorth1
      };

      return new AmazonS3Client(credentials, config);
      
   }
}