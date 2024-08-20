using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    public List <GameObject> craftObjects = new List<GameObject>();   
    public List <GameObject> gridPlanes = new List<GameObject>();
    private void Start()
    {
        for (int i = 0; i < craftObjects.Count; i++)
        {
            Instantiate(craftObjects[i], gridPlanes[i].transform);
            i++;
        }
    }
    void OnMouseDown()
    {
        print(name);
    }

}
