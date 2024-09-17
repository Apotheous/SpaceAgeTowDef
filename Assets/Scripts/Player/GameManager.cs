using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    //public static GameManager Instance;
    public List<GameObject> playerTowers= new List<GameObject>();

    //private void Awake()
    //{
    //    if (Instance == null)
    //        Instance = this;
    //    else
    //        Destroy(gameObject);
    //}

    public void OnTowerClicked(GameObject tower)
    {
        Debug.Log("+-+-++--++ = " + tower.name);

        foreach (var item in playerTowers)
        {
            if (tower.name != item.name)
            {
                item.GetComponent<TowerBuildManager>().planeClose();
                Debug.Log("+-+-++--++ = " + tower.name);
                Debug.Log("+-+-++--++ = " + item.name);
            }
            else
            {
                Debug.Log("+-+-++--++ = " + tower.name);
                Debug.Log("+-+-++--++ = " + item.name);
            }
        }
    }
}
