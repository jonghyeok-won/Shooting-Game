using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabCacheData
{
    public string filePath;
    public int cacheCount;
}

public class PrefabCacheSystem
{
    Dictionary<string, Queue<GameObject>> Caches = new Dictionary<string, Queue<GameObject>>();

    public void GenerateCache(string filePath, GameObject gameObject, int cacheCount)
    {
        if(Caches.ContainsKey(filePath)) 
        {
            Debug.LogWarning("Already cache generated! filePath = " + filePath);
            return;
        }
        else
        {
            Queue<GameObject> queue = new Queue<GameObject> ();
            for (int i=0; i<cacheCount; i++)
            {
                GameObject go = Object.Instantiate<GameObject>(gameObject);
                go.SetActive(false);
                queue.Enqueue(go);
            }

            Caches.Add(filePath, queue);
        }
    }

    // Start is called before the first frame update
    public GameObject Archive(string filePath)
    {
        if(!Caches.ContainsKey(filePath)) 
        {
            Debug.LogError("Archive Error! no Cache generated! filePath = " + filePath);
            return null;
        }

        if (Caches[filePath].Count == 0)
        {
            Debug.LogWarning("Archive problem! not enough Count");
            return null;
        }

        GameObject go = Caches[filePath].Dequeue();
        go.SetActive(true);

        return go;
    }

    public bool Restore(string filePath, GameObject gameObject)
    {
        if(!Caches.ContainsKey(filePath)) 
        {
            Debug.LogError("Restore Error! no Cache generated! filePath = " + filePath);
            return false;
        }

        gameObject.SetActive(false);

        Caches[filePath].Enqueue(gameObject); return true;
    }

}
