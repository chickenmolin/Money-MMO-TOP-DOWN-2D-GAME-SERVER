using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int id;
    public string username;
    public int points;
    public string role;

    // Constructor trống
    public PlayerData() { }

    // Khởi tạo từ PlayerPrefs
    public void LoadFromPlayerPrefs()
    {
        id = PlayerPrefs.GetInt("id", 0);
        username = PlayerPrefs.GetString("username", "Player");
        points = PlayerPrefs.GetInt("points", 0);
        role = PlayerPrefs.GetString("role", "client");

        Debug.Log($"[PlayerData] Loaded from PlayerPrefs -> ID: {id}, Username: {username}, Points: {points}, Role: {role}");
    }

    public void PrintInfo()
    {
        Debug.Log($"[PlayerData] ID: {id}, Username: {username}, Points: {points}, Role: {role}");
    }
}