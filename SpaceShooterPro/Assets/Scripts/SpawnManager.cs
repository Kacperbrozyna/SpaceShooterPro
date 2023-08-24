using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;
    private bool _spawning = true;


    // Start is called before the first frame update
    public void StartSpawing()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(spawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_spawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator spawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_spawning)
        { 
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            Instantiate(_powerUps[Random.Range(0, 3)], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }

    }

    public void OnPlayerDeath()
    {
        _spawning = false;
    }
  
}
