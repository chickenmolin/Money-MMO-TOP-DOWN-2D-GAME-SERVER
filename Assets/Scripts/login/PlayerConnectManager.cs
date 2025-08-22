using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using SocketIOClient;

public class PlayerConnectManager : MonoBehaviour
{
    [Header("UI Login")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public GameObject loginPanel;  // Panel login
    public GameObject gameUI;      // UI khi đã vào game

    [Header("Server Settings")]
    public string apiURL = "http://localhost:3000/auth/login"; // API login NodeJS
    public string socketURL = "http://localhost:3000";         // Socket.IO server

    private SocketIO client;
    private string username;
    private int points;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    void OnLoginClicked()
    {
        username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("⚠️ Username hoặc Password trống!");
            return;
        }

        StartCoroutine(LoginRoutine(username, password));
    }

    IEnumerator LoginRoutine(string user, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", user);
        form.AddField("password", pass);

        using (UnityWebRequest www = UnityWebRequest.Post(apiURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ Lỗi kết nối API: " + www.error);
            }
            else
            {
                Debug.Log("📩 Server response: " + www.downloadHandler.text);

                // Giả sử API trả về JSON {"success":true,"points":100}
                LoginResponse res = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);

                if (res.success)
                {
                    points = res.points;
                    loginPanel.SetActive(false);
                    gameUI.SetActive(true);
                    ConnectSocket();
                }
                else
                {
                    Debug.LogWarning("⚠️ Đăng nhập thất bại!");
                }
            }
        }
    }

    async void ConnectSocket()
    {
        client = new SocketIO(socketURL);

        client.OnConnected += (sender, e) =>
        {
            Debug.Log("🔌 Socket connected!");
            client.EmitAsync("join", new
            {
                username = username,
                points = points
            });
        };

        client.On("updatePlayers", response =>
        {
            string json = response.GetValue<string>();
            Debug.Log("📊 Player list: " + json);
        });

        await client.ConnectAsync();
    }

    private void OnDestroy()
    {
        if (client != null)
        {
            client.DisconnectAsync();
        }
    }

    [System.Serializable]
    public class LoginResponse
    {
        public bool success;
        public int points;
    }
}
