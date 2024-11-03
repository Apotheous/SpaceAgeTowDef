using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStorage : MonoBehaviour
{
    // Singleton instance
    public static PoolStorage Instance { get; private set; }
    public GameObject bulletPrefab;
    public int PoolSize;
    private List<GameObject> bulletPool;
    // Awake metodu ile Singleton örneðini oluþtur
    private void Awake()
    {
        // Eðer baþka bir instance varsa, bu nesneyi yok et
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Tek bir instance'a izin veriyoruz
        }
        else
        {
            Instance = this; // Bu instance'ý Singleton olarak belirle
            DontDestroyOnLoad(gameObject); // Bu objenin sahne deðiþikliklerinde yok olmasýný engelle
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new List<GameObject>();

        // Havuzu oluþtur
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.parent = transform;
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }


    void Update()
    {

    }

    // Örnek bir havuzdan eleman alma metodu
    public GameObject GetFromPool(string objectType)
    {
        // Kullanýlmayan bir mermiyi bul
        foreach (var bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        // Eðer hiç kullanýlmayan mermi yoksa, yeni bir tane oluþtur (ihtiyaca göre opsiyonel)
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.parent = transform;
        bulletPool.Add(newBullet);
        return newBullet;
    }
        public void ReturnToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    public void StartReturnBulletCoroutine(GameObject bullet, float delay)
    {
        StartCoroutine(ReturnBulletToPoolAfterTime(bullet, delay));
    }

    private IEnumerator ReturnBulletToPoolAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolStorage.Instance.ReturnToPool(bullet);
    }
}
