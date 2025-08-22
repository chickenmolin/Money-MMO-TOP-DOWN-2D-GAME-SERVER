using UnityEngine;
using TMPro;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;

    private PlayerData playerData;

    void Start()
    {
        playerData = new PlayerData();
        playerData.LoadFromPlayerPrefs();

        if (pointsText != null)
        {
            pointsText.text = $" {playerData.points}";
        }
    }

    // Gọi hàm này nếu muốn cập nhật điểm từ nơi khác
    public void UpdatePoints(int newPoints)
    {
        playerData.points = newPoints;

        if (pointsText != null)
        {
            pointsText.text = $" {newPoints}";
        }

        PlayerPrefs.SetInt("points", newPoints);
        PlayerPrefs.Save();
    }
}
