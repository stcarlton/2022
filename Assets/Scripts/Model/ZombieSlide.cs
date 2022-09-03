using UnityEngine;
using UnityEngine.AI;

public class ZombieSlide : Zombie
{
    void Start()
    {
        _armor = (int)(80f * _levelMultiplierMedium);
        _navMeshAgent.speed = 1 * (1f - _player.TimeDilation) * _levelMultiplierSlow;
    }
}
