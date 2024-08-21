using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> playerTowers= new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Eðer baþka bir instance varsa, bu objeyi yok et
    }

    public void OnTowerClicked(GameObject tower)
    {
        foreach (var item in playerTowers)
        {
            if (tower.name != item.name)
            {
                item.GetComponent<TowerBuildManager>().planeClose(); // item'in planeClose metodunu çaðýr
                Debug.Log(item.name); // item'in adýný yazdýr
            }
            else
            {
                Debug.Log(tower.name); // Týklanan kuleyi yazdýr
            }
        }
    }

    // Diðer oyun yönetimi ile ilgili metotlar...
}
