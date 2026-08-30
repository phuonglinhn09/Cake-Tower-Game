using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Cấu hình di chuyển")]
    public float speed = 3.5f;
    public float leftBound = -2.5f;
    public float rightBound = 2.5f;

    [Header("Danh sách Bánh")]
    public GameObject[] foodPrefabs;

    private GameObject currentFood;
    private bool movingRight = true;
    private bool isHoldingFood = false;

    void Start()
    {
        SpawnFood();
    }

    void Update()
    {
        MoveSpawner();

        if (isHoldingFood && currentFood != null)
        {
            currentFood.transform.position = transform.position;

            if (Input.GetMouseButtonDown(0))
            {
                DropFood();
            }
        }
    }

    void MoveSpawner()
    {
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound) movingRight = false;
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound) movingRight = true;
        }
    }

    public void SpawnFood()
    {
        if (foodPrefabs == null || foodPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, foodPrefabs.Length);
        currentFood = Instantiate(foodPrefabs[randomIndex], transform.position, Quaternion.identity);

        Rigidbody2D rb = currentFood.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }

        isHoldingFood = true;
    }

    void DropFood()
    {
        if (currentFood == null) return;

        Rigidbody2D rb = currentFood.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            // TĂNG LỰC CẢN ĐỂ BÓNG ĐẦM VÀ BỚT TRƠN TRƯỢT:
            rb.linearDamping = 1.0f;         // Lực cản di chuyển (giúp bóng rơi đầm và bớt nảy)
            rb.angularDamping = 2.0f;  // Lực cản xoay (giúp bóng đứng yên trên tháp, khó bị lăn/trượt)
            rb.gravityScale = 1.5f; // Tăng trọng lực nhẹ để bóng đáp xuống dứt khoát hơn
        }

        isHoldingFood = false;
        currentFood = null;
    }

    public void OnFoodLanded()
    {
        if (!isHoldingFood && currentFood == null)
        {
            Invoke(nameof(SpawnFood), 0.5f);
        }
    }
}