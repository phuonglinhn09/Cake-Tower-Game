using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Các Panel Màn Hình")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    public GameObject gameplayUI;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // Khi mới bật game lên -> Mở Màn hình chờ Home
        ShowMainMenu();
    }

    // Tắt tất cả các Panel để tránh bị đè giao diện lên nhau
    private void HideAllPanels()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    // 1. Mở Màn hình chính (Home)
    public void ShowMainMenu()
    {
        HideAllPanels();
        mainMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Tạm dừng vật lý khi ở Màn hình chính
    }

    // 2. Mở Màn hình chọn Level
    public void ShowLevelSelect()
    {
        HideAllPanels();
        levelSelectPanel.SetActive(true);
    }

    // 3. Mở Màn hình Cài đặt (Mở dạng Pop-up đè lên)
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // 4. Bắt đầu vào chơi Game (Gọi khi bấm Play hoặc chọn Level)
    public void StartGame(int levelNum)
    {
        HideAllPanels();
        gameplayUI.SetActive(true);
        Time.timeScale = 1f; // Cho phép vật lý chạy lại

        // Lưu Level đang chơi và bảo GameManager nạp Level
        PlayerPrefs.SetInt("CurrentPlayingLevel", levelNum);

        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.StartNewGameSession(levelNum);
        }
    }
}