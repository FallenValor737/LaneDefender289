using UnityEditor.Tilemaps;
using UnityEngine;

using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameManager Manager;
    public GameObject[] Spawns;
    public GameObject[] EnemyTypes;
    public int ChosenLine;
    public int EnemyType;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    public void SpawnEnemy()
    {
        if (Manager.GameInPlay)
        {
            Manager.ManuallySpawnOneEnemy = false;
            ChosenLine = Random.Range(0, 5);
            EnemyType = Random.Range(0, 3);
            Manager.Enemies.Add(Instantiate(EnemyTypes[EnemyType], Spawns[ChosenLine].transform.position, Quaternion.identity));
        }
    }

    public IEnumerator SpawnEnemies()
    {
        SpawnEnemy();
        yield return new WaitForSeconds(1);
        StartCoroutine(SpawnEnemies());
    }
}
