using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public static GameManager instanse;

    public List<GameObject> my_Towers = new List<GameObject>();

    public List<GameObject> my_Towers_Builders = new List<GameObject>();
    

    private void Awake()
    {
        if (instanse == null)
            instanse = this;
        else
            Destroy(gameObject);
    }

    public void OnTowerClicked(GameObject tower)
    {
        Debug.Log("+-+-++--++ = " + tower.name);

        foreach (var item in my_Towers_Builders)
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
