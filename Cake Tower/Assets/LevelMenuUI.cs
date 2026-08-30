using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelMenuUI : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform gridContent; // Ô chứa các nút level (Grid Layout Group)
    public int totalLevelsToShow = 50;

    void Start()
    {
        GenerateLevelButtons();
    }

    public void GenerateLevelButtons()
    {
        // Xóa các nút cũ nếu có
        foreach (Transform child in gridContent)
        {
            Destroy(child.gameObject);
        }

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 1; i <= totalLevelsToShow; i++)
        {
            int levelNum = i;
            GameObject btnObj = Instantiate(levelButtonPrefab, gridContent);

            TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null) btnText.text = levelNum.ToString();

            Button btn = btnObj.GetComponent<Button>();

            if (levelNum <= unlockedLevel)
            {
                btn.interactable = true;
                // Bấm vào nút level -> Gọi UIManager bắt đầu chơi level đó
                btn.onClick.AddListener(() => OnClickLevel(levelNum));
            }
            else
            {
                btn.interactable = false; // Khóa các level chưa mở
            }
        }
    }

    void OnClickLevel(int levelNum)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.StartGame(levelNum);
        }
    }
}