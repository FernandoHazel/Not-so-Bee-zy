using System.Collections.Generic;
using UnityEngine;

public class BoxObjectPool : MonoBehaviour
{
    public static BoxObjectPool current;

    [Tooltip("Assign the box prefab.")]
    public Indicator1 pooledObject;
    [Tooltip("Initial pooled amount.")]
    public int pooledAmount = 1;
    [Tooltip("Should the pooled amount increase.")]
    public bool willGrow = true;

    List<Indicator1> pooledObjects;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        pooledObjects = new List<Indicator1>();

        for (int i = 0; i < pooledAmount; i++)
        {
            Indicator1 box = Instantiate(pooledObject);
            box.transform.SetParent(transform, false);
            box.Activate(false);
            pooledObjects.Add(box);
        }
    }

    /// <summary>
    /// Gets pooled objects from the pool.
    /// </summary>
    /// <returns></returns>
    public Indicator1 GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].Active)
            {
                return pooledObjects[i];
            }
        }
        if (willGrow)
        {
            Indicator1 box = Instantiate(pooledObject);
            box.transform.SetParent(transform, false);
            box.Activate(false);
            pooledObjects.Add(box);
            return box;
        }
        return null;
    }

    /// <summary>
    /// Deactive all the objects in the pool.
    /// </summary>
    public void DeactivateAllPooledObjects()
    {
        foreach (Indicator1 box in pooledObjects)
        {
            box.Activate(false);
        }
    }
}
