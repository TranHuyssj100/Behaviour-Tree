using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ServerPlayerRepository : IPlayerRepository
{
    readonly string baseUrl;
    readonly string authToken;

    public ServerPlayerRepository(string baseUrl, string authToken = null)
    {
        this.baseUrl = baseUrl != null ? baseUrl.TrimEnd('/') : string.Empty;
        this.authToken = authToken;
    }

    public async UniTask<PlayerSaveDto> LoadAsync(string playerId, CancellationToken cancellationToken = default)
    {
        using UnityWebRequest request = UnityWebRequest.Get(GetUrl(playerId));
        ApplyHeaders(request);

        await request.SendWebRequest().WithCancellation(cancellationToken);
        return SaveMigration.Migrate(JsonUtility.FromJson<PlayerSaveDto>(request.downloadHandler.text));
    }

    public async UniTask SaveAsync(string playerId, PlayerSaveDto save, CancellationToken cancellationToken = default)
    {
        if (save == null)
            return;

        byte[] body = Encoding.UTF8.GetBytes(JsonUtility.ToJson(save));

        using UnityWebRequest request = new UnityWebRequest(GetUrl(playerId), UnityWebRequest.kHttpVerbPUT)
        {
            uploadHandler = new UploadHandlerRaw(body),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        ApplyHeaders(request);

        await request.SendWebRequest().WithCancellation(cancellationToken);
    }

    void ApplyHeaders(UnityWebRequest request)
    {
        if (!string.IsNullOrEmpty(authToken))
            request.SetRequestHeader("Authorization", $"Bearer {authToken}");
    }

    string GetUrl(string playerId)
    {
        return $"{baseUrl}/players/{playerId}";
    }
}
