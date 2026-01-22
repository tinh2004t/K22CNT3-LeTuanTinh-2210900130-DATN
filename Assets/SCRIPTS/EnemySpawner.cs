using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float respawnDelay = 10f;

    private GameObject currentEnemy;

    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy Prefab is not assigned in the EnemySpawner");
            return;
        }

        currentEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        currentEnemy.GetComponent<Animal>().OnDestroyed += HandleEnemyDeath;
    }

    private void OnDestroy()
    {
        currentEnemy.GetComponent<Animal>().OnDestroyed -= HandleEnemyDeath;

    }

    private void HandleEnemyDeath()
    {
        currentEnemy = null;
        Invoke(nameof(SpawnEnemy), respawnDelay);
    }
}
