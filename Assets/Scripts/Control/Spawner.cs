using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Zombie[] _zombiePrefabs;
    [SerializeField] Item _itemPrefab;
    float _spawnDelay = 12f;
    float _nextSpawnTime;
    float _initialSpawnTime;

    SkillTree _skillTree;

    private void Start()
    {
        _skillTree = FindObjectOfType<SkillTree>();
        _initialSpawnTime = Time.time + 60;
    }

    void Update()
    {
        if (ReadyToSpawn())
        {
            Spawn();
        }
    }

    bool ReadyToSpawn()
    {
        return Time.time >= _nextSpawnTime && Time.time >= _initialSpawnTime;
    }

    void Spawn()
    {
        _nextSpawnTime = Time.time + _spawnDelay / (float)System.Math.Pow(1.005f,_skillTree.Level);

        int randomIndex = Random.Range(0, _zombiePrefabs.Length);
        var zombiePrefab = _zombiePrefabs[randomIndex];
        
        Instantiate(zombiePrefab, transform.position, transform.rotation);
    }
    public void SpawnItem()
    {
        Instantiate(_itemPrefab, transform.position, transform.rotation);
    }
}