using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage; // Nilai damage projectile
    public float speed; // Kecepatan projectile
    private Rigidbody2D rb;
    private Transform player; // Referensi ke transform player

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Temukan objek player di scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;

            // Hitung arah dari projectile ke player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed; // Gerakkan projectile ke arah player

            // Set rotasi projectile berdasarkan arah
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Hitung sudut dalam derajat
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Rotasikan projectile
        }
        else
        {
            Debug.LogWarning("You Lose!"); // Peringatan jika player tidak ditemukan
            rb.linearVelocity = Vector2.down * speed; // Default gerakan ke bawah jika player tidak ditemukan
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Berikan damage ke player jika tertabrak projectile
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }
            Destroy(gameObject); // Hancurkan projectile setelah memberikan damage
        }
        else
        {
            Destroy(gameObject); // Hancurkan projectile setelah menabrak objek apapun
        }
    }
}
