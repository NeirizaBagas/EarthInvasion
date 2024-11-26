using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp;
    public GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireDelay;
    [SerializeField] private bool canShoot;
    [SerializeField] private GameObject[] dropItems;

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
