using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public string serverUrl = "http://localhost:3000/player/";
    private PlayerData player;

    void Start()
    {
        // Nếu đã có dữ liệu PlayerPrefs, khởi tạo tạm PlayerData
        if (PlayerPrefs.HasKey("username"))
        {
            player = new PlayerData();
        player.LoadFromPlayerPrefs();  // ⚠️ gọi ở đây, an toàn với Unity
        player.PrintInfo();

            // Tự start mạng theo role đã lưu
            StartNetworkByRole(player.role);
        }
        else
        {
            Debug.Log("❌ Chưa có dữ liệu PlayerPrefs, cần gọi GetPlayer(username)");
        }
        PlayerNameTag nameTag = FindObjectOfType<PlayerNameTag>();
        if(nameTag != null)
        {
            Debug.Log("[PlayerManager] Found PlayerNameTag, setting data...");
            nameTag.SetPlayerData(player);
        }
        else
        {
            Debug.LogError("[PlayerManager] PlayerNameTag not found!");
        }
    }

    public void GetPlayer(string username)
    {
        StartCoroutine(GetPlayerCoroutine(username));
    }

    private IEnumerator GetPlayerCoroutine(string username)
    {
        UnityWebRequest req = UnityWebRequest.Get(serverUrl + username);
        yield return req.SendWebRequest();

        if(req.result == UnityWebRequest.Result.Success)
        {
            string json = req.downloadHandler.text;
            player = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log($"Player Loaded: {player.username}, Points: {player.points}, Role: {player.role}");

            // Hiển thị tên lên NameTag
            PlayerNameTag nameTag = FindObjectOfType<PlayerNameTag>();
            if(nameTag != null)
                nameTag.SetPlayerData(player);

            // Lưu PlayerData vào PlayerPrefs
            PlayerPrefs.SetInt("id", player.id);
            PlayerPrefs.SetString("username", player.username);
            PlayerPrefs.SetInt("points", player.points);
            PlayerPrefs.SetString("role", player.role);
            PlayerPrefs.Save();

            // Tự start mạng theo role
            StartNetworkByRole(player.role);
        }
        else
        {
            Debug.LogError("Lỗi lấy dữ liệu player: " + req.error);
        }
    }

    private void StartNetworkByRole(string role)
    {
        switch(role)
        {
            case "host":
                Debug.Log("✅ Starting as Host");
                // NetworkManager.Singleton.StartHost();  // uncomment nếu dùng Netcode
                break;
            case "client":
                Debug.Log("✅ Starting as Client");
                // NetworkManager.Singleton.StartClient();
                break;
            case "server":
                Debug.Log("✅ Starting as Server");
                // NetworkManager.Singleton.StartServer();
                break;
            default:
                Debug.LogWarning("⚠️ Role không xác định: " + role);
                break;
        }
    }
}
