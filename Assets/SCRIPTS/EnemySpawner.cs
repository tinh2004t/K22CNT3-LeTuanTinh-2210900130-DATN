using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnTimeOption { Anytime, DayOnly, NightOnly }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float respawnDelay = 10f;
    [SerializeField] private SpawnTimeOption spawnTimeRequirement = SpawnTimeOption.Anytime;

    [Header("Spawn Settings")]
    [Tooltip("Nếu bật, khi vào game (dù đúng thời gian) vẫn sẽ đợi hết respawnDelay rồi mới đẻ quái lần đầu.")]
    [SerializeField] private bool waitBeforeFirstSpawn = false;

    private GameObject currentEnemy;

    private void Start()
    {
        StartCoroutine(SpawnProcess());
    }

    private IEnumerator SpawnProcess()
    {
        // Tùy chọn: Chờ một khoảng thời gian trước khi bắt đầu sinh quái lần đầu tiên
        if (waitBeforeFirstSpawn)
        {
            yield return new WaitForSeconds(respawnDelay);
        }

        while (true)
        {
            // Kiểm tra: Chưa có quái VÀ ĐÚNG thời gian thì mới spawn
            if (currentEnemy == null && CanSpawnNow())
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private bool CanSpawnNow()
    {
        // ĐÃ SỬA: Nếu DayNightSystem chưa sẵn sàng, trả về false để bắt spawner phải CHỜ.
        if (DayNightSystem.Instance == null) return false;

        bool isNight = DayNightSystem.Instance.IsNight();

        switch (spawnTimeRequirement)
        {
            case SpawnTimeOption.DayOnly:
                return !isNight;
            case SpawnTimeOption.NightOnly:
                return isNight;
            case SpawnTimeOption.Anytime:
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