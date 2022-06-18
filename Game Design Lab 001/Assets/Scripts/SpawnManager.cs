using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    float groundDistance = -1.0f;
    void Start()
    {
        for (int j = 0; j < 1; j++) //spawn 1 greenEnemy
            spawnFromPooler(ObjectType.greenEnemy);

        for (int j = 0; j < 1; j++) //spawn 1 goomba
            spawnFromPooler(ObjectType.gombaEnemy);
    }


    void spawnFromPooler(ObjectType i)
    {
        // static method access
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);

        if (item != null) //if there exist an item that we can get
        {
            //set position and other necessary states
            item.transform.localScale = new Vector3(1, 1, 1);
            item.transform.position = new Vector3(Random.Range(-4.5f, 40.0f), groundDistance + item.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            item.SetActive(true);
        }
        else
        {
            Debug.Log("not enough items in the pool!");
        }
    }

    // Function to spawn a new enemy
    public void spawnNewEnemy()
    {
        Debug.Log("Spawn New Enemy!");
        ObjectType i = Random.Range(0, 2) == 0 ? ObjectType.gombaEnemy : ObjectType.greenEnemy;
        spawnFromPooler(i);
    }

}
