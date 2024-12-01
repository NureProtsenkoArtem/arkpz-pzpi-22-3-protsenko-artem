// Код до рефакторингу

public class BaseStorageService
{
    protected readonly IAmazonS3 _s3Client;
    protected readonly AwsOptions _options;
    protected readonly ILogger _logger;

    public BaseStorageService(IAmazonS3 s3Client, IOptions<AwsOptions> options, ILogger logger)
    {
        _s3Client = s3Client;
        _options = options.Value;
        _logger = logger;
    }

    protected string BuildFileKey(string id, string key)
    {
        return $"user_icons/{id}/{DateTime.Now:HH:mm:ss}{key}";
    }

    protected string GetFileUrl(string key)
    {
        return $"https://mydevhubimagebucket.s3.eu-west-3.amazonaws.com/{key}";
    }
}

public class StorageService : BaseStorageService, IStorageService
{
    public StorageService(IAmazonS3 s3Client, IOptions<AwsOptions> options, ILogger<StorageService> logger)
        : base(s3Client, options, logger)
    { }

    public async Task<string> UploadFileAsync(string id, string key, Stream fileStream, string contentType)
    {
        var request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = BuildFileKey(id, key),
            InputStream = fileStream,
            ContentType = contentType
        };

        try
        {
            var response = await _s3Client.PutObjectAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return GetFileUrl(request.Key);
            }

            throw new Exception("File upload failed");
        }
        catch (AmazonS3Exception e)
        {
            throw new AmazonS3Exception($"500: {e.Message}");
        }
        catch (Exception e)
        {
            throw new Exception($"500:{e.Message}");
        }
    }

    public async Task DeleteFileAsync(string avatarPath)
    {
        var uri = new Uri(avatarPath);
        var key = uri.AbsolutePath.TrimStart('/');

        var request = new DeleteObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key,
        };

        try
        {
            var response = await _s3Client.DeleteObjectAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.NoContent)
            {
                _logger.LogInformation($"User icon with {avatarPath} path successfully deleted");
                return;
            }
            else
            {
                throw new Exception($"{response.HttpStatusCode}: Failed to delete file {avatarPath}.");
            }
        }
        catch (AmazonS3Exception e)
        {
            throw new AmazonS3Exception($"500: {e.Message}");
        }
        catch (Exception e)
        {
            throw new Exception($"500:{e.Message}");
        }
    }
}

// Код після рефакторингу

public class StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly AwsOptions _options;
    private readonly ILogger<StorageService> _logger;

    public StorageService(IAmazonS3 s3Client, IOptions<AwsOptions> options, ILogger<StorageService> logger)
    {
        _s3Client = s3Client;
        _logger = logger;
        _options = options.Value;
    }

    private string BuildFileKey(string id, string key)
    {
        return $"user_icons/{id}/{DateTime.Now:HH:mm:ss}{key}";
    }

    private string GetFileUrl(string key)
    {
        return $"https://mydevhubimagebucket.s3.eu-west-3.amazonaws.com/{key}";
    }

    public async Task<string> UploadFileAsync(string id, string key, Stream fileStream, string contentType)
    {
        var request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = BuildFileKey(id, key),
            InputStream = fileStream,
            ContentType = contentType
        };

        try
        {
            var response = await _s3Client.PutObjectAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return GetFileUrl(request.Key);
            }

            throw new Exception("File upload failed");
        }
        catch (AmazonS3Exception e)
        {
            throw new AmazonS3Exception($"500: {e.Message}");
        }
        catch (Exception e)
        {
            throw new Exception($"500:{e.Message}");
        }
    }

    public async Task DeleteFileAsync(string avatarPath)
    {
        var uri = new Uri(avatarPath);
        var key = uri.AbsolutePath.TrimStart('/');

        var request = new DeleteObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key,
        };

        try
        {
            var response = await _s3Client.DeleteObjectAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.NoContent)
            {
                _logger.LogInformation($"User icon with {avatarPath} path successfully deleted");
                return;
            }

            throw new Exception($"{response.HttpStatusCode}: Failed to delete file {avatarPath}.");
        }
        catch (AmazonS3Exception e)
        {
            throw new AmazonS3Exception($"500: {e.Message}");
        }
        catch (Exception e)
        {
            throw new Exception($"500:{e.Message}");
        }
    }
}
