using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalPlayerRepository : IPlayerRepository
{
    readonly string rootPath;

    public LocalPlayerRepository() : this(Application.persistentDataPath)
    {
    }

    public LocalPlayerRepository(string rootPath)
    {
        this.rootPath = rootPath;
    }

    public async UniTask<PlayerSaveDto> LoadAsync(string playerId, CancellationToken cancellationToken = default)
    {
        string path = GetPath(playerId);
        if (!File.Exists(path))
            return new PlayerSaveDto();

        string json = await UniTask.RunOnThreadPool(() => File.ReadAllText(path), cancellationToken: cancellationToken);
        return SaveMigration.Migrate(JsonUtility.FromJson<PlayerSaveDto>(json));
    }

    public async UniTask SaveAsync(string playerId, PlayerSaveDto save, CancellationToken cancellationToken = default)
    {
        if (save == null)
            return;

        string json = JsonUtility.ToJson(save, true);
        string path = GetPath(playerId);
        string directory = rootPath;

        await UniTask.RunOnThreadPool(() =>
        {
            Directory.CreateDirectory(directory);
            File.WriteAllText(path, json);
        }, cancellationToken: cancellationToken);
    }

    string GetPath(string playerId)
    {
        string safeId = string.IsNullOrWhiteSpace(playerId) ? "default" : playerId;
        return Path.Combine(rootPath, $"player_{safeId}.json");
    }
}
