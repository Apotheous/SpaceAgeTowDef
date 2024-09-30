using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SO_Controller : MonoBehaviour
{
    public List<SO_Enemy_Obj> poolObjects = new List<SO_Enemy_Obj>();
    [SerializeField] bool isObjectUnderParent;
    [SerializeField] int initPoolSize;
    int objInt;
    private void Awake()
    {
        foreach (var obj in poolObjects)
        {
            CreatePoolObject(obj, initPoolSize);
        }
    }

    void CreatePoolObject(SO_Enemy_Obj go,int callSize)
    {
        for (int i = 0; i < callSize; i++)
        {
            GameObject newObject = Instantiate(go.myPref);
            newObject.gameObject.SetActive(false);
            newObject.transform.position = Vector3.up * 1000;
            if (isObjectUnderParent) newObject.transform.parent = transform;
        }
    }


}

