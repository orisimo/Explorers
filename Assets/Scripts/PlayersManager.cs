using UnityEngine;

public class PlayersManager : Singleton<PlayersManager>
{
    [SerializeField] private PlayerContext[] _playerContexts;
    [SerializeField] private float _chaseFleeRadius = 2f;
    public PlayerContext[] PlayerContexts { get { return _playerContexts; }}
    public float ChaseFleeRadius => _chaseFleeRadius;
}
