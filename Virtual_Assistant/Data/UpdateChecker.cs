using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Virtual_Assistant.Models;

namespace Virtual_Assistant.Data;

public class UpdateChecker
{
    private readonly ILogger<UpdateChecker> _logger;
    private readonly HttpClient _httpClient;

    public UpdateChecker(ILogger<UpdateChecker> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public event EventHandler<Update> UpdateFound;

    public async Task CheckUpdateAndNotifyFrontendAsync()
    {
        if (!File.Exists(Constants.UpdateJson))
        {
            _logger.LogError($"Update metadata file not found at {Constants.UpdateJson}");
            return;
        }

        var currentUpdateMetadataFileContent = await File.ReadAllTextAsync(Constants.UpdateJson);
        var currentUpdateMetadata = JsonConvert.DeserializeObject<Update>(currentUpdateMetadataFileContent);

        if (currentUpdateMetadata is null)
        {
            _logger.LogError($"Update metadata at {Constants.UpdateJson} is malformed!");
            return;
        }

        // automatically get update metadata from repo...most unsafe code i wrote here...man
        try
        {
            var updateMetadataUrl =
                $"{currentUpdateMetadata.Repository?.Replace("github.com", "raw.githubusercontent.com")}/master/Virtual_Assistant/update.json";

            var remoteUpdateDataContent = await _httpClient.GetStringAsync(updateMetadataUrl);
            var remoteUpdateMetadata = JsonConvert.DeserializeObject<Update>(remoteUpdateDataContent);

            if (remoteUpdateMetadata?.Version > currentUpdateMetadata.Version)
            {
                // new update!
                UpdateFound?.Invoke(this, remoteUpdateMetadata);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching update metadata: {ex.Message}");
        }
    }
}