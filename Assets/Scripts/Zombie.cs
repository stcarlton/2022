using System;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public static event Action<Zombie> Hit;
    public static event Action<Zombie> Death;

    public int LastHitTaken;
    public bool LastCrit;
    public bool Burning;

    protected const float LEVEL_MULTIPLIER = 1.04f;
    protected const float LEVEL_MULTIPLIER_MEDIUM = 1.03f;
    protected const float LEVEL_MULTIPLIER_SLOW = 1.01f;
    protected const float INITIAL_ATTACK_RANGE = 1f;
    protected const float INITIAL_HEALTH = 100;
    protected const float INITIAL_POWER = 25;

    protected float _levelMultiplier;
    protected float _levelMultiplierMedium;
    protected float _levelMultiplierSlow;
    protected float _attackRange;
    protected int _health;
    protected int _power;
    protected int _armor;
    protected int _currentHealth;
    protected int[] _dot;
 
    protected bool Alive => _currentHealth > 0;
    
    int _amplify;
    float _nextDotTime;
    float _nextStaggerTime;
    int _dotPointer;

    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected Player _player;
    protected SkillTree _skillTree;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
        _skillTree = FindObjectOfType<SkillTree>();
        _dot = new int[12];
        _nextStaggerTime = Time.time;

        _levelMultiplier = Mathf.Pow(LEVEL_MULTIPLIER, _skillTree.Level);
        _levelMultiplierMedium = Mathf.Pow(LEVEL_MULTIPLIER_MEDIUM, _skillTree.Level);
        _levelMultiplierSlow = Mathf.Pow(LEVEL_MULTIPLIER_SLOW, _skillTree.Level);
        _health = (int)(INITIAL_HEALTH * _levelMultiplier);
        _power = (int)(INITIAL_POWER * _levelMultiplier);
        _attackRange = (INITIAL_ATTACK_RANGE * _levelMultiplierSlow);
        _currentHealth = _health;
    }

    void Update()
    {
        if (!Alive)
        {
            return;
        }
        if (ReadyForDot())
        {
            AdvanceDot();
        }
        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.SetDestination(_player.transform.position);
            if (Vector3.Distance(transform.position, _player.transform.position) < _attackRange)
            {
                Attack();
            }
        }
    }
    bool ReadyForDot() => Time.time >= _nextDotTime;
    void OnCollisionEnter(Collision collision)
    {
        var blasterShot = collision.collider.GetComponent<BlasterShot>();
        if(blasterShot != null)
        {
            if (blasterShot.Incendiary > 0)
            {
                incDotDamage((int)(blasterShot.ShotPower * blasterShot.Incendiary));
            }
            int armor = (int)((1 - blasterShot.ArmorShred) * _armor);
            float modification = (float)(1.0f / Math.Pow(2, (float)armor / 100f));
            float modifiedPower = blasterShot.ShotPower * (float)Math.Pow(1.01f,_amplify) * modification;
            LastCrit = blasterShot.Crit;
            AdjustHealth((int)modifiedPower, blasterShot.Stagger);
            if (blasterShot.Amplify)
            {
                _amplify++;
            }
        }
    }

    void TakeHit(bool stagger)
    {
        if (_navMeshAgent.enabled && stagger && Time.time > _nextStaggerTime)
        {
            _animator.SetTrigger("Hit");
            _navMeshAgent.enabled = false;
            _nextStaggerTime = Time.time + 5f;
        }
    }

    void Die()
    {
        GetComponent<Collider>().enabled = false;
        _animator.SetTrigger("Died");
        _navMeshAgent.enabled = false;
        Death?.Invoke(this);
        Destroy(gameObject, 5f);
    }

    void Attack()
    {
        _animator.SetTrigger("Attack");
        _navMeshAgent.enabled = false;

    }
    void AdvanceDot()
    {
        _nextDotTime = Time.time + 0.25f;
        int dot = _dot[_dotPointer] / _dot.Length;
        if (dot != 0)
        {
            Burning = true;
            AdjustHealth(dot, false);
            Burning = false;
            _dot[_dotPointer] = 0;
            if (_dotPointer >= 4)
            {
                _dotPointer = 0;
            }
            else
            {
                _dotPointer++;
            }
        }
    }
    void AdjustHealth(int power, bool stagger)
    {
        _currentHealth -= power;
        LastHitTaken = power;
        _player.Vamp(LastHitTaken);
        Hit?.Invoke(this);
        if (!Alive)
        {
            Die();
        }
        else
        {
            TakeHit(stagger);
        }
    }
    void incDotDamage(int num)
    {
        for (int i = 0; i < _dot.Length; i++)
        {
            _dot[i] += num;
        }
    }

    //Animation Callback
    #region Animation_Callbacks
    void AttackComplete()
    {
        if(Alive)
        {
            _navMeshAgent.enabled = true;
        }
    }
    void AttackHit()
    {
        var player = FindObjectOfType<Player>();
        if (Vector3.Distance(transform.position, player.transform.position) < _attackRange*2)
        {
            player.TakeHit(_power);
        }
    }
    void HitComplete()
    {
        if (Alive)
        {
            _navMeshAgent.enabled = true;
        }
    }
    #endregion
}
