using System.Collections.Generic;
using UnityEngine;


// Enemy Types (enumeration)
public enum ObjectType{
    gombaEnemy = 0, //goomba
    greenEnemy = 1 //koopa
}

// A class to define the data structure of an Object metadata to be spawned into the pool
[System.Serializable] //makes it editable in the inspector
public class ObjectPoolItem
{
   public int amount;
   public GameObject prefab;
   public bool expandPool;
   public ObjectType type;
}

// A class to define the data structure of an Object in the pool
public class ExistingPoolItem
{
    public GameObject gameObject;
    public ObjectType type;
    public ExistingPoolItem(GameObject gameObject, ObjectType type){
        // reference input 
        this.gameObject = gameObject;
        this.type = type;
    }
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance; //static variable, where only one should exist in the scene

    public List<ObjectPoolItem> itemsToPool; // types of different object to pool (e.g. 2 = two types of monsters: goomba and koopa)
    public List<ExistingPoolItem> pooledObjects; // a list of all objects in the pool, of all types
    void Awake()
    {
        SharedInstance = this;
        pooledObjects = new List<ExistingPoolItem>();
        Debug.Log("ObjectPooler Awake");
        foreach (ObjectPoolItem item in itemsToPool) //for each enemy type
        {
            for (int i = 0; i < item.amount; i++) //for each count of an enemy type (e.g. 3 goombas means call this 3 times)
            {
                // this 'pickup' a local variable, but Unity will not remove it since it exists in the scene
                GameObject pickup = (GameObject)Instantiate(item.prefab);
                pickup.SetActive(false); //disable the object initially (inactive)
                pickup.transform.parent = this.transform;
                ExistingPoolItem e = new ExistingPoolItem(pickup, item.type); //local variable e  containing reference to newly instantiated ExistingPoolItem
                pooledObjects.Add(e); //add to the list of objects in the pool
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // this method can be called by other scripts to get pooled object by its type defined as enum earlier, or simly as tag as you like
    // there's no "return" object to pool method. Simply set it as unavailable
    public GameObject GetPooledObject(ObjectType type) //Public method to return GameObject reference of one of the requested object in the pool when available, takes in parameter of "type" of tenemy
    {
        // return inactive pooled object if it matches the type 
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy && pooledObjects[i].type == type) //if the pooled object is not active and matches the type asked
            {
                return pooledObjects[i].gameObject; //get reference to gameObject in the pool
            }
        }
        // this will be called no more active object is present, item to expand pool if required 
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.type == type)
            {
                if (item.expandPool) //if expandPool = true for the ObjectPoolItem (enemy type)
                {
                    GameObject pickup = (GameObject)Instantiate(item.prefab);
                    pickup.SetActive(false);
                    pickup.transform.parent = this.transform;
                    pooledObjects.Add(new ExistingPoolItem(pickup, item.type));
                    return pickup;
                }
            }
        }

        // we will return null IF and only IF the type doesn't match with what is defined in the itemsToPool. 
        return null;
    }

}
