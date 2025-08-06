using System.Text.Json;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using static AP.AWS.Libs.Constants;

namespace AP.AWS.Secrets;

public class APSecretsManager
{
    private AmazonSecretsManagerClient _client;

    public APSecretsManager()
    {
        _client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(Region));
    }

    public APSecretsManager(string access, string secret)
    {
        _client = new AmazonSecretsManagerClient(access, secret, RegionEndpoint.USEast2);
    }

    private async Task<string> GetSecret(string secretName)
    {
        var request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = VersionStage,
        };

        GetSecretValueResponse response;

        try
        {
            response = await _client.GetSecretValueAsync(request);
        }
        catch (Exception e)
        {
            throw e;
        }

        var secret = response.SecretString;

        return secret;
    }

    public async Task<Dictionary<string, string>> GetSecrets(IList<string> secretNames)
    {
        var requests = secretNames.Select(secretKey => new GetSecretValueRequest
        {
            SecretId = secretKey,
            VersionStage = VersionStage,
        }).ToList();

        var tasks = secretNames.Select(GetSecret);
        var responses = await Task.WhenAll(tasks);
        var results = new Dictionary<string, string>();

        for (var i = 0; i < requests.Count; i++)
        {
            var value = JsonSerializer.Deserialize<Dictionary<string, string>>(responses[i]);
            
            if (value == null) continue;
            
            foreach (var key in value.Keys)
            {
                results[key] = value[key];
            }
        }

        return results;
    }
}