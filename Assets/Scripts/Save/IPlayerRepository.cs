using System.Threading;
using Cysharp.Threading.Tasks;

public interface IPlayerRepository
{
    UniTask<PlayerSaveDto> LoadAsync(string playerId, CancellationToken cancellationToken = default);
    UniTask SaveAsync(string playerId, PlayerSaveDto save, CancellationToken cancellationToken = default);
}
