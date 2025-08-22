using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger enter from {gameObject.name} → {collision.gameObject.name}");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("💰 Player collected a coin!");

            // Đọc thông tin từ PlayerPrefs
            string username = PlayerPrefs.GetString("username", "Player");
            int currentPoints = PlayerPrefs.GetInt("points", 0);
            int newPoints = currentPoints + 1;

            // ✅ Cập nhật local
            PlayerPrefs.SetInt("points", newPoints);
            PlayerPrefs.Save();

            // ✅ Cập nhật UI
            PointsDisplay display = FindObjectOfType<PointsDisplay>();
            if (display != null)
            {
                display.UpdatePoints(newPoints);
            }

            // ✅ Gửi lên server
            StartCoroutine(UpdatePointsToServer(username, newPoints));

            // ✅ Xoá coin
            Destroy(gameObject);
        }
    }

    IEnumerator UpdatePointsToServer(string username, int newPoints)
    {
        string url = $"http://localhost:3000/player/{username}";

        // Gói JSON gửi đi
        string json = JsonUtility.ToJson(new PointsPayload { points = newPoints });

        UnityWebRequest req = UnityWebRequest.Put(url, json);
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"✅ Điểm đã được cập nhật lên server: {newPoints}");
        }
        else
        {
            Debug.LogError($"❌ Lỗi cập nhật điểm lên server: {req.error}");
        }
    }

    // Class JSON để gửi PUT
    [System.Serializable]
    public class PointsPayload
    {
        public int points;
    }
}
