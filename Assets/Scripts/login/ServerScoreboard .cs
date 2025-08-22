using Unity.Netcode;
using UnityEngine;

public class ServerScoreboard : MonoBehaviour
{
    void OnGUI()
    {
        if (!NetworkManager.Singleton.IsServer) return;

        GUILayout.BeginArea(new Rect(10, 200, 300, 400));
        GUILayout.Label("=== Danh sách Player Points ===");

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var playerObj = client.PlayerObject;
            if (playerObj != null && playerObj.TryGetComponent<PlayerStats>(out var stats))
            {
                GUILayout.Label($"Player {client.ClientId} : {stats.Point.Value} điểm");
            }
        }

        GUILayout.EndArea();
    }
}
