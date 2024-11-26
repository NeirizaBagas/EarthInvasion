using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp;
    public GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireDelay;
    [SerializeField] private bool canShoot;
    [SerializeField] private GameObject drop;

    void Start()
    {
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
        GameObject peluru = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }

    void Drop()
    {
        if (drop != null)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }
}
