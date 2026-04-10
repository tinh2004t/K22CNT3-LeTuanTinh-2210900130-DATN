using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnTimeOption { Anytime, DayOnly, NightOnly }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float respawnDelay = 10f;
    [SerializeField] private SpawnTimeOption spawnTimeRequirement = SpawnTimeOption.Anytime;

    private GameObject currentEnemy;

    private void Start()
    {
        StartCoroutine(SpawnProcess());
    }

    private IEnumerator SpawnProcess()
    {
        while (true)
        {
            if (currentEnemy == null && CanSpawnNow())
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private bool CanSpawnNow()
    {
        if (DayNightSystem.Instance == null) return true;

        bool isNight = DayNightSystem.Instance.IsNight();

        switch (spawnTimeRequirement)
        {
            case SpawnTimeOption.DayOnly:
                return !isNight;
            case SpawnTimeOption.NightOnly:
                return isNight;
            default:
                return true;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        currentEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        Animal animalScript = currentEnemy.GetComponent<Animal>();
        if (animalScript != null)
        {
            animalScript.OnDestroyed += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath()
    {
        if (currentEnemy != null)
        {
            Animal animalScript = currentEnemy.GetComponent<Animal>();
            if (animalScript != null) animalScript.OnDestroyed -= HandleEnemyDeath;
        }

        currentEnemy = null;

        StopAllCoroutines();
        StartCoroutine(DelayedRestart());
    }

    private IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(respawnDelay);
        StartCoroutine(SpawnProcess());
    }
}