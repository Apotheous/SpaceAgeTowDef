using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerMan : MonoBehaviour
{
    public GameObject towerMain;
    public GameObject builder;
    TowerBuildManager towerBuildManager;
    public GameObject currentTurret;

    public UnityEvent onChildAdded;


    private int previousChildCount;

    void Start()
    {
        if (onChildAdded == null)
            onChildAdded = new UnityEvent();

        // Dinleyiciyi event'e ekleyin
        onChildAdded.AddListener(AddedChild);

        // Ba�lang�� �ocuk say�s�n� belirleyin
        previousChildCount = builder.transform.childCount;

        towerBuildManager = builder.GetComponent<TowerBuildManager>();
        towerBuildManager.StartSpawnTurrets();
        towerBuildManager.planeClose();
    }

    void Update()
    {
        int currentChildCount = builder.transform.childCount;

        // �ocuk say�s� artt���nda event'i tetikleyin
        if (currentChildCount > previousChildCount)
        {
            onChildAdded.Invoke();
            previousChildCount = currentChildCount; // G�ncelleme
        }
    }

    public void AddedChild()
    {
        towerBuildManager.planeClose();
    }

}
