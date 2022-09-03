using UnityEngine;
using UnityEngine.AI;

public class ZombieSlow : Zombie
{
    void Start()
    {
        _armor = (int)(60f * _levelMultiplierMedium);
        _navMeshAgent.speed = 2f * (1f - _player.TimeDilation) * _levelMultiplierSlow;
    }
}
