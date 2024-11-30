using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //Darah
    public int health;

    //Gerak
    public GameObject posisiA;
    public GameObject posisiB;
    public int speed;
    private Transform currentPoint;
    private Rigidbody2D rb;

    //Attack
    [SerializeField] private GameObject peluruPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int fireSpeed; //kecepatan peluru
    [SerializeField] private bool canShoot; //tes bisa nembak
    public float fireDelay; //jeda nembak

    //Durasi Efek
    public int defaultDamage;
    private Coroutine bonusDamageCoroutine;
    private Coroutine fireSpeedCoroutine;
    public float bonusDamageDuration;
    public float fireSpeedDuration;
    private float originalFireDelay;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found!");
        }

        rb = GetComponent<Rigidbody2D>();
        currentPoint = posisiB.transform;
        canShoot = true;

        originalFireDelay = fireDelay;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            Shoot();
        }

        if (health > 100)
        {
            health = 100;
        }
    }

    private void Move()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == posisiB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == posisiB.transform)
        {
            currentPoint = posisiA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == posisiA.transform)
        {
            currentPoint = posisiB.transform;
        }
    }

    void Shoot()
    {
        // Membuat peluru baru dari prefab
        GameObject peluru = Instantiate(peluruPrefab, firePoint.position, Quaternion.identity);

        // Memberikan kecepatan ke peluru
        Rigidbody2D rb = peluru.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(0, fireSpeed);

        Peluru peluruScript = peluru.GetComponent<Peluru>();
        peluruScript.damage += defaultDamage;

        canShoot = false; // Mencegah tembakan berikutnya sampai peluru ini menghilang

        // Reset mekanisme tembakan setelah peluru menghilang
        Destroy(peluru, 2f); // Hancurkan peluru setelah 2 detik
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        // Tunggu sebentar sebelum mengizinkan tembakan berikutnya
        yield return new WaitForSeconds(fireDelay);
        canShoot = true;
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;

            if (health <= 0)
            {
                gameManager.GameOver();
                Destroy(gameObject);
            }
        }
    }

    public void ActivateBonusDamage(int extraDamage, float duration)
    {
        //Jika efek sedang aktif, hentikan dulu
        if (bonusDamageCoroutine != null)
        {
            StopCoroutine(bonusDamageCoroutine);
        }

        // Mulai efek baru
        bonusDamageCoroutine = StartCoroutine(BonusDamageCoroutine(extraDamage, duration));
    }

    public void ActivateFireSpeed(float newFireSpeed, float duration)
    {
        if (fireSpeedCoroutine != null)
        {
            StopCoroutine (fireSpeedCoroutine);
        }

        // Mulai efek baru
        fireSpeedCoroutine = StartCoroutine(FireSpeedCoroutine(newFireSpeed, duration));
    }

    private IEnumerator BonusDamageCoroutine(int extraDamage, float duration)
    {
        defaultDamage += extraDamage; // Tambah bonus damage
        print("Damage+");
        yield return new WaitForSeconds(duration); // Tunggu durasi efek
        defaultDamage -= extraDamage; // Kembalikan nilai damage
        print("Damage-");
    }

    private IEnumerator FireSpeedCoroutine(float newFireSpeed, float duration)
    {
        fireDelay = newFireSpeed; // Kurangi delay
        print("Speed+");
        yield return new WaitForSeconds(duration); // Tunggu durasi efek
        fireDelay = originalFireDelay; // Kembalikan delay ke nilai asli
        print("Speed-");
    }
}
