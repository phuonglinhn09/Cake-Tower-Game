using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private bool hasTriggeredWin = false;

    void Start()
    {
        hasTriggeredWin = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckWin(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckWin(collision);
    }

    void CheckWin(Collider2D collision)
    {
        if (hasTriggeredWin) return;

        if (collision.CompareTag("Food"))
        {
            FoodPhysics food = collision.GetComponent<FoodPhysics>();
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            // CHỈ THẮNG KHI VỎN VẸN 3 ĐIỀU KIỆN:
            // 1. Bánh ở trạng thái rơi vật lý Dynamic
            // 2. Bánh ĐÃ CHẠM BÀN HOẶC BÁNH KHÁC TRƯỚC ĐÓ (food.hasLanded == true)
            // 3. Bánh phải nằm trong vùng vạch đích sau khi rơi xuống (vận tốc Y gần như đã ổn định, không phải đang bay ngang trên tay Spawner)
            if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic && food != null && food.hasLanded)
            {
                hasTriggeredWin = true;

                GameManager gm = Object.FindFirstObjectByType<GameManager>();
                if (gm != null)
                {
                    gm.TriggerWin();
                }
            }
        }
    }
}