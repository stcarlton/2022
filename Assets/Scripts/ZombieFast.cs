using UnityEngine;
using UnityEngine.AI;

public class ZombieFast : Zombie
{
    void Start()
    {
        _armor = (int)(40f * _levelMultiplierMedium);
        _navMeshAgent.speed = 5 * (1f - _player.TimeDilation) * _levelMultiplierSlow;
    }
}
