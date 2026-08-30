using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [Header("Danh sách Prefab Thực phẩm")]
    public GameObject[] easyFoods;
    public GameObject[] mediumFoods;
    public GameObject[] hardFoods;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GeneratedLevelData GenerateLevel(int levelNumber)
    {
        GeneratedLevelData data = new GeneratedLevelData();
        data.levelNumber = levelNumber;

        // Giới hạn chiều cao FinishLine tối đa là 3.2f để Spawner đặt ở Y = 4.2f không bị tràn màn hình
        data.finishLineHeight = Mathf.Min(1.5f + (levelNumber - 1) * 0.15f, 3.2f);

        data.spawnerSpeed = Mathf.Min(3.0f + (levelNumber - 1) * 0.15f, 6.5f);
        data.timeLimit = Mathf.Max(20.0f, 20.0f + (data.finishLineHeight * 2.0f) - (levelNumber * 0.3f));
        data.allowedFoods = SelectFoodsForLevel(levelNumber);

        return data;
    }

    private GameObject[] SelectFoodsForLevel(int level)
    {
        if (level <= 3) return easyFoods;
        if (level <= 7) return CombineArrays(easyFoods, mediumFoods);
        return CombineArrays(mediumFoods, hardFoods);
    }

    private GameObject[] CombineArrays(GameObject[] first, GameObject[] second)
    {
        GameObject[] result = new GameObject[first.Length + second.Length];
        first.CopyTo(result, 0);
        second.CopyTo(result, first.Length);
        return result;
    }
}

public class GeneratedLevelData
{
    public int levelNumber;
    public float finishLineHeight;
    public float spawnerSpeed;
    public float timeLimit;
    public GameObject[] allowedFoods;
}