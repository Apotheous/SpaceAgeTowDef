using UnityEngine;

public class PoolCleaner : MonoBehaviour
{
    public BasicPool pool;


    private void OnCollisionEnter(Collision collision)
    {
        //Asteroid collidable = collision.gameObject.GetComponent<Asteroid>();
        //if (collidable != null)
        //{
        //    pool.ReturnToPool(collision.gameObject);
        //}
    }

}
