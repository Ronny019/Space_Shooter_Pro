using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (!_stopSpawn)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 spawnVector = new Vector3(randomX, 7.0f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnVector, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(!_stopSpawn)
        {
            float randomTime = Random.Range(3.0f, 7.0f);
            float randomX = Random.Range(-8f, 8f);
            Vector3 spawnVector = new Vector3(randomX, 7.0f, 0);
            int randomPowerUp = Random.Range(0, 3);
            GameObject newTripleShot = Instantiate(_powerups[randomPowerUp], spawnVector, Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawn = true;
    }
}
