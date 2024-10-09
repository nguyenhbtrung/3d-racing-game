using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VehiclesPool : MonoBehaviour
{
    public static VehiclesPool Instance;
    private List<GameObject> pooledObject;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int maxObject;

    public List<GameObject> PooledObject { get => pooledObject; set => pooledObject = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        PooledObject = new List<GameObject>();
        int average = maxObject / prefabs.Length;
        GameObject temp;
        for (int i = 0; i < average; i++)
        {
            foreach (GameObject prefab in prefabs)
            {
                temp = Instantiate(prefab, transform);
                temp.SetActive(false);
                PooledObject.Add(temp);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < maxObject; i++)
        {
            if (!PooledObject[i].activeInHierarchy)
            {
                PooledObject[i].SetActive(true);
                return PooledObject[i];
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject pooledObject)
    {
        pooledObject.SetActive(false);
    }
}
