using UnityEngine;
using UnityEngine.AI;

public class ZombieCrawl : Zombie
{
    void Start()
    {
        _armor = (int)(20f * _levelMultiplierMedium);
        _navMeshAgent.speed = 7 * (1f - _player.TimeDilation) * _levelMultiplierSlow;
    }
}
