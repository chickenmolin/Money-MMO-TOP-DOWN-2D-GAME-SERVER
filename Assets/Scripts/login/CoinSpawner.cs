using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float spawnInterval = 5f;

    private CoinSpawnPoint[] spawnPoints;

    void Start()
    {
        // Tìm tất cả các điểm spawn có gắn script CoinSpawnPoint
        spawnPoints = FindObjectsOfType<CoinSpawnPoint>();

        // Gọi mỗi vài giây
        InvokeRepeating(nameof(SpawnCoins), 0f, spawnInterval);
    }

    void SpawnCoins()
    {
        foreach (var point in spawnPoints)
        {
            // Kiểm tra xem đã có coin con bên dưới chưa
            if (point.transform.childCount == 0)
            {
                GameObject newCoin = Instantiate(coinPrefab, point.transform.position, Quaternion.identity);

                // Gắn coin làm con của point (để kiểm tra lần sau)
                newCoin.transform.parent = point.transform;
            }
        }
    }
}
