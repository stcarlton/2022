using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] BlasterShot _blasterShotPrefab;
    [SerializeField] LayerMask _aimLayerMask;
    [SerializeField] Transform _firePoint;

    Animator _animator;
    SkillTree _skillTree;
    HealthBar _healthBar;
    GameOverUI _gameOverUI;

    float _fireRate = 10f;
    float _dodgeChance = 0f;
    int _shotPower = 20;
    int _baseShotPower = 20;
    float _critRate = 0.2f;
    public int _maxHealth = 200;
    int _spread = 0;

    float _velocity = 10f;
    float _movementSpeed = 2f;
    float _critMult = 2f;
    int _armor = 50;
    float _armorShred = 0f;

    public float TimeDilation = 0f;
    float _damageCurve = 0f;
    float _vampirism = 0f;
    float _incendiary = 0f;

    bool _ricochet = false;
    bool _stagger = false;
    bool _amplify = false;

    bool _regen = false;
    bool _lifeLine = false;

    bool _goldenGod = false;

    public static event Action<Player> Hit;
    public int LastHitTaken;
    int _health;
    float _nextFireTime;
    float _nextDotTime;
    float _healthRegenTime;
    float _lifelineTime;
    float _lifelineCooldown;
    bool _fireTrigger;
    int[] _dot;
    int _dotPointer;
    bool _dead;
    float _regenPerc
    {
        get
        {
            return (float)(_regen ? 0.2 : 0.1);
        }
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _skillTree = FindObjectOfType<SkillTree>();
        _healthBar = FindObjectOfType<HealthBar>();
        _gameOverUI = FindObjectOfType<GameOverUI>();
        _dot = new int[5];
        _fireTrigger = false;
        _health = _maxHealth;
        _dead = false;
    }

    void Update()
    {
        if (!_dead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0f, vertical);

            if (movement.magnitude > 0)
            {
                movement.Normalize();
                movement *= (_movementSpeed + (_fireTrigger ? 0f : 2f))* Time.deltaTime;
                transform.Translate(movement, Space.World);
            }

            float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
            float velocityX = Vector3.Dot(movement.normalized, transform.right);


            _animator.SetFloat("Vertical", velocityZ, 0.1f, Time.deltaTime);
            _animator.SetFloat("Horizontal", velocityX, 0.1f, Time.deltaTime);

            AimTowardMouse();
            if (Input.GetMouseButtonDown(0))
            {
                _fireTrigger = !_fireTrigger;
            }
            if (ReadyToFire() && _fireTrigger)
            {
                Fire();
            }
            if (ReadyForDot())
            {
                AdvanceDot();
            }
        }
    }

    void AimTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _aimLayerMask))
        {
            var destination = hitInfo.point;
            destination.y = transform.position.y;

            Vector3 direction = destination - transform.position;
            direction.Normalize();
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        }
    }

    bool ReadyToFire()
    {
        return Time.time >= _nextFireTime && _fireTrigger;
    }
    bool ReadyForDot() => Time.time >= _nextDotTime;

    void Fire()
    {
        float delay = 1f/_fireRate;
        _nextFireTime = Time.time + delay;
        BlasterShot shot = Instantiate(_blasterShotPrefab, _firePoint.position, transform.rotation);
        System.Random r = new System.Random();
        bool crit = r.NextDouble() <= _critRate;
        int shotPower = (int)(crit ? _shotPower * _critMult : _shotPower);
        shot.Launch(transform.forward, _velocity, shotPower, crit, _stagger, _ricochet, _amplify, _incendiary, _armorShred);

        for(int i = 2; i < _spread+2; i++)
        {
            int k = i % 2 == 0 ? (i / 2) * 8 : -(i / 2) * 8 ;
            shot = Instantiate(
                _blasterShotPrefab,
                _firePoint.position,
                Quaternion.Euler(Quaternion.Euler(0, k, 0) * transform.forward));
            shot.Launch(Quaternion.Euler(0, k, 0)*transform.forward, _velocity, shotPower/2, crit, _stagger, _ricochet, _amplify, _incendiary, _armorShred);
            if(i == 6)
            {
                shot = Instantiate(
                    _blasterShotPrefab,
                    _firePoint.position,
                    Quaternion.Euler(Quaternion.Euler(0, -k, 0) * transform.forward));
                shot.Launch(Quaternion.Euler(0, -k, 0) * transform.forward, _velocity, shotPower/2, crit, _stagger, _ricochet, _amplify, _incendiary, _armorShred);
            }
        }
    }

    public void TakeHit(int power)
    {
        if (!_dead)
        {
            System.Random r = new System.Random();
            if (r.NextDouble() > _dodgeChance)
            {
                int modifiedPower = -(int)(power * (1 / Math.Pow(2, _armor / 100f)));
                int dotDamage = (int)(modifiedPower * _damageCurve);
                modifiedPower -= dotDamage;
                incDotDamage(dotDamage);
                AdjustHealth(modifiedPower);
                _healthRegenTime = Time.time + (_regen ? 0f : 5f);
            }
            else
            {
                LastHitTaken = 0;
                Hit?.Invoke(this);
            }
        }
    }
    void AdvanceDot()
    {
        _nextDotTime = Time.time + 1f;
        int regen = (int)(Time.time < _healthRegenTime ? 0 : _maxHealth * _regenPerc);
        int dot = _dot[_dotPointer] + regen;
        dot = dot > (_maxHealth - _health) ? (_maxHealth - _health) : dot;
        if (dot != 0)
        {
            AdjustHealth(dot);
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
    void CheckDeath()
    {
        if (_health < 0)
        {
            if(_lifeLine && Time.time >= _lifelineCooldown)
            {
                _lifelineCooldown = Time.time + 120f;
                _lifelineTime = Time.time + 5f;
                _health = 1;
            }
            else if(!_dead)
            {
                _animator.SetTrigger("Die");
                _dead = true;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<CapsuleCollider>().enabled = false;
                _gameOverUI.Show();
            }
        }
    }
    void CapHealth()
    {
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }
    void incDotDamage(int num)
    {
        for(int i = 0; i < 5; i++)
        {
            _dot[i] += num/5;
        }
    }
    public void AddItem()
    {
        _baseShotPower += 5;
    }
    public void Vamp(int power)
    {
        if(_health < _maxHealth && _vampirism > 0f)
        {
            power = (int)(power * _vampirism) + 1;
            power = power > (_maxHealth - _health) ? (_maxHealth - _health) : power;
            AdjustHealth(power);
        }
    }
    public void AdjustHealth(int power)
    {
        if(power < 0 && !_lifeLine || Time.time > _lifelineTime)
        {
            LastHitTaken = power;
            Hit?.Invoke(this);
            _health += power;
            CheckDeath();
        }
        else if(power > 0)
        {
            LastHitTaken = power;
            Hit?.Invoke(this);
            _health += power;
            CapHealth();
        }
        _healthBar.SetHealthBar((float)_health/(float)_maxHealth);
    }
    public void LevelUp()
    {
        _goldenGod = _skillTree.Skills[20].CurrentRank == 1;

        float goldenGodMultiplier = _goldenGod ? (float)Math.Pow(1.01f,_skillTree.Level - 60) : 1f;
        
        _fireRate = 10f + _skillTree.Skills[0].CurrentRank * 2f * goldenGodMultiplier;
        _dodgeChance = 0f + _skillTree.Skills[1].CurrentRank * 0.1f;
        _critRate = 0f + _skillTree.Skills[2].CurrentRank * 0.1f;
        _shotPower = (int)(_baseShotPower + _skillTree.Skills[3].CurrentRank * 4f * goldenGodMultiplier);
        _maxHealth = (int)(200 + _skillTree.Skills[4].CurrentRank * 40f * goldenGodMultiplier);
        _spread = 0 + _skillTree.Skills[5].CurrentRank;

        _velocity = 10f + _skillTree.Skills[6].CurrentRank * 2f * goldenGodMultiplier;
        _movementSpeed = 2f + _skillTree.Skills[7].CurrentRank * 0.4f;
        _critMult = 2f + _skillTree.Skills[8].CurrentRank * 0.4f * goldenGodMultiplier;
        _armor = (int)(50 + _skillTree.Skills[9].CurrentRank * 10f * goldenGodMultiplier);
        _armorShred = 0f + _skillTree.Skills[10].CurrentRank * 0.1f;

        TimeDilation = 0f + _skillTree.Skills[11].CurrentRank * 0.1f;
        _damageCurve = 0f + _skillTree.Skills[12].CurrentRank * 0.1f;
        _vampirism = 0f + _skillTree.Skills[13].CurrentRank * 0.05f;
        _incendiary = 0f + _skillTree.Skills[14].CurrentRank * 0.2f;

        _amplify = _skillTree.Skills[15].CurrentRank == 1;
        _stagger = _skillTree.Skills[16].CurrentRank == 1;
        _ricochet = _skillTree.Skills[17].CurrentRank == 1;

        _regen = _skillTree.Skills[18].CurrentRank == 1;
        _lifeLine = _skillTree.Skills[19].CurrentRank == 1;
    }
}
