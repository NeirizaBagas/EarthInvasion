using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp;
    public int enemyDamage;
    public float fireSpeed;
    [SerializeField] private float fireDelay;
    [SerializeField] private Transform firePoint;
    [SerializeField] private bool canShoot;
    public GameObject bulletPrefab;
    public int enemyScore;
    private GameManager gameManager;
    [SerializeField] private GameObject[] dropItems;

    void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found!");
        }
        canShoot = true; // Enemy bisa menembak di awal
    }

    void Update()
    {
        // Hanya tembak jika diperbolehkan
        if (canShoot)
        {
            StartCoroutine(ShootDelay());
        }
    }

    public void TakeDamage(int damage)
    {
        if (maxHp > 0)
        {
            maxHp -= damage;

            if (maxHp <= 0)
            {
                Drop();
                gameManager.enemyDestroy++;
                gameManager.score += enemyScore;
                Destroy(gameObject);
            }
        }
    }

    IEnumerator ShootDelay()
    {
        canShoot = false; // Matikan izin menembak sementara
        Shoot();
        yield return new WaitForSeconds(fireDelay); // Tunggu sesuai fireDelay
        canShoot = true; // Izinkan menembak lagi
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Memberikan kecepatan ke peluru
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(0, fireSpeed);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage += enemyDamage;
        bulletScript.speed += fireSpeed;
    }

    void Drop()
    {
        if (dropItems != null && dropItems.Length > 0)
        {
            //pilih item secara acak dari array
            int randomIndex = Random.Range(0, dropItems.Length);
            GameObject selectedDrop = dropItems[randomIndex];

            //Intantiate item di posisi enemy
            Instantiate(selectedDrop, transform.position, Quaternion.identity);
        }
    }
}
