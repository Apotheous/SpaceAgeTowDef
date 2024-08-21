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
            Destroy(gameObject); // E�er ba�ka bir instance varsa, bu objeyi yok et
    }

    public void OnTowerClicked(GameObject tower)
    {
        foreach (var item in playerTowers)
        {
            if (tower.name != item.name)
            {
                item.GetComponent<TowerBuildManager>().planeClose(); // item'in planeClose metodunu �a��r
                Debug.Log(item.name); // item'in ad�n� yazd�r
            }
            else
            {
                Debug.Log(tower.name); // T�klanan kuleyi yazd�r
            }
        }
    }

    // Di�er oyun y�netimi ile ilgili metotlar...
}
