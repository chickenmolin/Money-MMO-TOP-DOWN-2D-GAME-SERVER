using UnityEngine;
using TMPro;

public class PlayerNameTag : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;

    private PlayerData playerData;

    public void SetPlayerData(PlayerData data)
    {
        playerData = data;

        if(playerData != null)
        {
            Debug.Log($"[PlayerNameTag] SetPlayerData called -> Username: {playerData.username}, Points: {playerData.points}, Role: {playerData.role}");
        }

        if(nameText != null && playerData != null)
        {
            nameText.text = playerData.username;
        }
    }

    void LateUpdate()
    {
        if(playerData != null && nameText != null)
        {
            // Debug mỗi frame sẽ in ra username + points
            Debug.Log($"[PlayerNameTag] LateUpdate -> Username: {playerData.username}, Points: {playerData.points}");
            nameText.text = $"{playerData.username} ";
        }

        if(Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
}
