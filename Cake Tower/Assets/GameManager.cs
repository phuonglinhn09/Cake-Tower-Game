using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Cấu hình UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;

    [Header("Tham chiếu Scene")]
    public Transform finishLineTransform;

    private float currentTime;
    private bool isGameEnded = true; // Mặc định chưa chơi
    private int currentLevel = 1;
    private float finishLineY;

    // Hàm gọi từ UIManager khi bắt đầu vào chơi 1 level
    public void StartNewGameSession(int levelNum)
    {
        currentLevel = levelNum;
        isGameEnded = false;

        // Dọn dẹp tất cả bánh cũ trên bàn từ lượt chơi trước
        GameObject[] oldFoods = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject food in oldFoods)
        {
            Destroy(food);
        }

        // Nạp dữ liệu Level từ LevelGenerator
        if (LevelGenerator.Instance != null)
        {
            GeneratedLevelData data = LevelGenerator.Instance.GenerateLevel(levelNum);

            currentTime = data.timeLimit;
            if (levelText != null) levelText.text = "LEVEL " + data.levelNumber;

            // Đặt vị trí FinishLine
            if (finishLineTransform != null)
            {
                Vector3 pos = finishLineTransform.position;
                pos.y = data.finishLineHeight;
                finishLineTransform.position = pos;
                finishLineY = pos.y;
            }

            // Đặt vị trí Spawner
            Spawner spawner = FindFirstObjectByType<Spawner>();
            if (spawner != null)
            {
                spawner.speed = data.spawnerSpeed;
                spawner.foodPrefabs = data.allowedFoods;

                Vector3 spawnerPos = spawner.transform.position;
                spawnerPos.y = finishLineY + 1.0f;
                spawner.transform.position = spawnerPos;

                spawner.SpawnFood(); // Sinh quả đầu tiên
            }
        }
    }

    void Update()
    {
        if (isGameEnded) return;

        // Đếm ngược thời gian
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (timerText != null) timerText.text = "Thời gian: " + Mathf.CeilToInt(currentTime) + "s";
        }
        else
        {
            TriggerGameOver("Hết thời gian!");
            return;
        }

        CheckTowerHeight();
    }

    void CheckTowerHeight()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");

        foreach (GameObject food in foods)
        {
            Rigidbody2D rb = food.GetComponent<Rigidbody2D>();
            FoodPhysics fp = food.GetComponent<FoodPhysics>();

            if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
            {
                if (fp != null && fp.hasLanded)
                {
                    if (food.transform.position.y >= finishLineY)
                    {
                        if (rb.linearVelocity.y <= 0.1f)
                        {
                            TriggerWin();
                            break;
                        }
                    }
                }
            }
        }
    }

    public void TriggerWin()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
        }

        // Hiện WinPanel thông qua UIManager
        if (UIManager.Instance != null && UIManager.Instance.winPanel != null)
        {
            UIManager.Instance.winPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void TriggerGameOver(string reason)
    {
        if (isGameEnded) return;
        isGameEnded = true;

        // Hiện GameOverPanel thông qua UIManager
        if (UIManager.Instance != null && UIManager.Instance.gameOverPanel != null)
        {
            UIManager.Instance.gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    // Bấm chơi tiếp màn sau
    public void LoadNextLevel()
    {
        StartNewGameSession(currentLevel + 1);
        if (UIManager.Instance != null)
        {
            UIManager.Instance.winPanel.SetActive(false);
            UIManager.Instance.gameplayUI.SetActive(true);
        }
        Time.timeScale = 1f;
    }

    // Bấm chơi lại màn hiện tại
    public void RestartGame()
    {
        StartNewGameSession(currentLevel);
        if (UIManager.Instance != null)
        {
            UIManager.Instance.winPanel.SetActive(false);
            UIManager.Instance.gameOverPanel.SetActive(false);
            UIManager.Instance.gameplayUI.SetActive(true);
        }
        Time.timeScale = 1f;
    }
}