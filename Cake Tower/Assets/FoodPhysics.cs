using UnityEngine;

public class FoodPhysics : MonoBehaviour
{
    public bool hasLanded = false;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Xử lý va chạm lần đầu để kích hoạt hạ cánh và gọi Spawner bóng tiếp theo
        if (!hasLanded && (collision.gameObject.CompareTag("Food") || collision.gameObject.name == "Table"))
        {
            hasLanded = true;

            Spawner spawner = FindFirstObjectByType<Spawner>();
            if (spawner != null)
            {
                spawner.OnFoodLanded();
            }
        }

        // 2. PHÂN TÍCH VẬT LÝ ĐỘ NGHIÊNG & MÔ-MEN XOAY (TORQUE)
        if (collision.gameObject.CompareTag("Food") || collision.gameObject.name == "Table")
        {
            // Lấy điểm tiếp xúc va chạm đầu tiên
            ContactPoint2D contact = collision.contacts[0];

            // Tính khoảng cách lệch giữa điểm va chạm (chân đế) và trọng tâm vật thể (Center of Mass)
            float offset = contact.point.x - transform.position.x;

            // Nếu va chạm lệch tâm quá 0.08 unit -> Tác động Moment lực xoay (Torque = F x d)
            if (Mathf.Abs(offset) > 0.08f)
            {
                // Lệch trái (offset < 0) -> Xoay phải (torque positive)
                // Lệch phải (offset > 0) -> Xoay trái (torque negative)
                float torqueMagnitude = -offset * 2.5f;

                // Tác động lực xoay ngay lập tức dạng Impulse
                rb.AddTorque(torqueMagnitude, ForceMode2D.Impulse);
            }
        }
    }
}