using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BulletObjectPool : NetworkBehaviour
{
    public static BulletObjectPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    public override void OnStartLocalPlayer()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
}
