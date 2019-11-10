using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class AiUtil : Singleton<AiUtil>
{
    public bool IsPlayerInRadius(Vector3 position, float radius)
    {
        for (var playerIndex = 0; playerIndex < PlayersManager.Instance.PlayerContexts.Length; playerIndex++)
        {
            var playerContext = PlayersManager.Instance.PlayerContexts[playerIndex];
            var playerPosition = playerContext.MovementController.transform.position;
            if (Vector3.Distance(playerPosition, position) < radius)
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 GetNearestPlayerPosition(Vector3 position, out PlayerContext nearestPlayer, out float nearestPlayerDistance)
    {
        Vector3 nearestPlayerPosition = default(Vector3);
        nearestPlayer = null;
        nearestPlayerDistance = 0f;
        for (var playerIndex = 0; playerIndex < PlayersManager.Instance.PlayerContexts.Length; playerIndex++)
        {
            var playerContext = PlayersManager.Instance.PlayerContexts[playerIndex];
            var playerPosition = playerContext.MovementController.transform.position;
            var playerDistance = Vector3.Distance(playerPosition, position);
            if (playerIndex != 0 && !(playerDistance < nearestPlayerDistance))
            {
                continue;
            }
            nearestPlayerPosition = playerPosition;
            nearestPlayerDistance = playerDistance;
            nearestPlayer = playerContext;
        }
        
        return nearestPlayerPosition;
    }
}
