using System.Collections.Generic;
using UnityEngine;

public class MyPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    private static List<GameObject> blocks = new List<GameObject>();

    public static MyPool instance { get; private set; }

    public GameObject GetFromPool()
    {
        int index = Random.Range(0, blocks.Count);
        GameObject gm = blocks[index];
        blocks.RemoveAt(index);
        gm.SetActive(true);
        return gm;
    }
    
    public void PutInPool(GameObject gm)
    {
        blocks.Add(gm);
        gm.SetActive(false);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        foreach (var item in prefabs)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject gm = Instantiate(item, transform);
                blocks.Add(gm);
                gm.SetActive(false);
            }
        }
    }
}
