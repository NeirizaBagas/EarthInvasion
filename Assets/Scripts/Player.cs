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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = posisiB.transform;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            Shoot();
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
                // Drop item atau efek lain
                Destroy(gameObject);
            }
        }
    }
}
