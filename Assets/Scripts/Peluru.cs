using UnityEngine;

public class Peluru : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        else if (collision.gameObject.tag == "Drop")
        {
            DropItem drop = collision.GetComponent<DropItem>();

            if (drop != null)
            {
                // Referensi ke player
                Player player = FindAnyObjectByType<Player>();
                if (player != null)
                {
                    drop.ApplyEffect(player);
                }
            }
            Destroy(collision.gameObject); // Hancurkan item
        }

        Destroy(gameObject);
    }

    public void BoostDamage(int boostDamage)
    {
        damage += boostDamage;
    }
}
