using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class UINetworkManager : MonoBehaviour
{
    private string role = "";
    private bool modeSelected = false; // Đã chọn chế độ chưa

    void Start()
    {
        // Lấy role từ PlayerPrefs (vd: "host", "client", "server")
        role = PlayerPrefs.GetString("role", "");

        if (!string.IsNullOrEmpty(role))
        {
            AutoStartByRole();
        }
    }

    void AutoStartByRole()
    {
        switch (role.ToLower())
        {
            case "host":
                Debug.Log("Auto start as Host");
                NetworkManager.Singleton.StartHost();
                break;
            case "server":
                Debug.Log("Auto start as Server");
                NetworkManager.Singleton.StartServer();
                break;
            case "client":
                Debug.Log("Auto start as Client");
                NetworkManager.Singleton.StartClient();
                break;
            default:
                Debug.LogWarning("Role không hợp lệ, hiển thị nút chọn thủ công");
                break;
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (string.IsNullOrEmpty(role) && !modeSelected)
            {
                // Chưa có role => hiện đủ nút
                StartButtons();
            }
            else if (!modeSelected)
            {
                // Có role => chỉ hiện nút theo role
                RoleBasedButtons();
            }
            else
            {
                GUILayout.Label("Đang chờ kết nối...");
            }
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    void RoleBasedButtons()
    {
        if (role == "host" && GUILayout.Button("Host"))
        {
            modeSelected = true;
            NetworkManager.Singleton.StartHost();
        }
        else if (role == "client" && GUILayout.Button("Client"))
        {
            modeSelected = true;
            NetworkManager.Singleton.StartClient();
        }
        else if (role == "server" && GUILayout.Button("Server"))
        {
            modeSelected = true;
            NetworkManager.Singleton.StartServer();
        }
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost
            ? "Host"
            : NetworkManager.Singleton.IsServer
                ? "Server"
                : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }
}
