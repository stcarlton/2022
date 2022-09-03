using UnityEngine;
using UnityEngine.AI;

public class ZombieDance : Zombie
{
    private void Awake()
    {
        _currentHealth = _health;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
        _dot = new int[12];

        _navMeshAgent.speed = 0;
        _health = 30;
        _armor = 20;
        _power = 50;
        _attackRange = 1f;
    }
    void Update()
    {
        return;
    }
}
