using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_Text messageText;

    string serverUrl = "http://localhost:3000/auth/login"; // URL server

    public void OnLoginButtonClick()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            messageText.text = "Vui lòng nhập đầy đủ thông tin!";
            return;
        }

        StartCoroutine(LoginRequest(username, password));
    }

    IEnumerator LoginRequest(string username, string password)
    {
        // JSON gửi lên server
        string jsonData = JsonUtility.ToJson(new LoginData(username, password));

        UnityWebRequest req = new UnityWebRequest(serverUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            if (req.responseCode == 200)
            {
                // Parse dữ liệu trả về
                LoginResponse res = JsonUtility.FromJson<LoginResponse>(req.downloadHandler.text);

                messageText.text = res.message;

                // Lưu thông tin player để scene sau dùng
                PlayerPrefs.SetString("username", res.player.username);
                PlayerPrefs.SetInt("points", res.player.points);
                PlayerPrefs.SetString("role", res.player.role);

                PlayerPrefs.Save();

                yield return new WaitForSeconds(0.5f);
                SceneManager.LoadScene(1); // Load scene tiếp theo
            }
            else
            {
                messageText.text = "Sai tài khoản hoặc mật khẩu!";
            }
        }
        else
        {
            messageText.text = "Lỗi kết nối server!";
            Debug.LogError(req.error);
        }
    }

    [System.Serializable]
    public class LoginData
    {
        public string username;
        public string password;
        public LoginData(string u, string p)
        {
            username = u;
            password = p;
        }
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public string username;
        public int points;
        public string role;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string message;
        public PlayerInfo player;
    }
}
