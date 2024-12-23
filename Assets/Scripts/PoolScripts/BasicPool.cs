using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPool : MonoBehaviour
{

    [System.Serializable]
    public class Pool_Class
    {
        [ColoredHeader("Prefab list of Objects to be Produced")]
        public GameObject[] pool_Elements;
        public int pool_Object_Size;
        public float obj_Min_Size, obj_Max_Size;
    }

    public Pool_Class pool_Class;
 
    [SerializeField]private Queue<GameObject> my_Pool_Queue;

    void Start()
    {
        my_Pool_Queue = new Queue<GameObject>();
        ElementsProduction(pool_Class.pool_Elements, pool_Class.pool_Object_Size);
    }

    private void ElementsProduction(GameObject[] obj, int object_size)
    {
        for (int i = 0; i < object_size; i++)
        {
            foreach (GameObject obj2 in obj)
            {
                GameObject my_object = Instantiate(obj2);
                float astScale = Random.Range(pool_Class.obj_Min_Size, pool_Class.obj_Max_Size);
                // Yuvarlama (3 ondalık basamak)
                astScale = (float)System.Math.Round(astScale, 3);
                my_object.transform.localScale = new Vector3(astScale, astScale, astScale);
                my_object.transform.parent = transform;
                my_object.transform.position = transform.position;
                my_object.transform.transform.rotation = transform.rotation;
                my_object.SetActive(false);
                my_Pool_Queue.Enqueue(my_object); 
            }
        }
    }

    public GameObject GetFromPool()
    {
        if (my_Pool_Queue.Count > 0)
        {
            GameObject obj = my_Pool_Queue.Dequeue();
            obj.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }

        return null; 
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);

        obj.transform.parent = transform;
       
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero; // Rigidbody'nin velocity sıfırlanıyor
            rb.angularVelocity = Vector3.zero; // Angular velocity sıfırlanıyor
        }
        else
        {
            Debug.LogWarning($"Rigidbody bulunamadı: {obj.name}");
        }
        my_Pool_Queue.Enqueue(obj); 
    }

    public void StartReturnBulletCoroutine(GameObject obj, float delay)
    {
        StartCoroutine(ReturnBulletToPoolAfterTime(obj, delay));
    }

    private IEnumerator ReturnBulletToPoolAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(obj);
    }
}
