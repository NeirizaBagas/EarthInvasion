using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum EffectType {FireSpeed, BonusDamage, Regen}
    public EffectType effectType;
    public int healingAmount;
    public float boostSpeed;
    public int boostDamage;
    public Peluru peluru;
    public float dropSpeed; //Kecepatan drop
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(0, -dropSpeed); //Kecepatan gerak kebawah
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                ApplyEffect(player);
            }

            //Objek hancur saat atau tidak diambil
            Destroy(gameObject);
        }
    }

    void ApplyEffect(Player player)
    {
        switch (effectType)
        {
            case EffectType.Regen:
                player.health += healingAmount; //Nambahin darah
                break;

            case EffectType.BonusDamage:
                peluru.damage += boostDamage; //Nambah damage
                break;
            case EffectType.FireSpeed:
                player.fireDelay -= boostSpeed; //Nambah speed
                break;
        }

        // Contoh: Anda bisa menambahkan efek visual atau suara di sini
        Debug.Log($"Effect Applied: {effectType}");
    }


}
