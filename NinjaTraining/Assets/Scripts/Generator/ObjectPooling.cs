using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] List<Dart> dartPrefabs = new List<Dart>();
    [SerializeField] Queue<Dart> dartObjectesQueue = new Queue<Dart>();

    [SerializeField] List<Item> itemPrefabs = new List<Item>();
    [SerializeField] Queue<Item> itemObjectesQueue = new Queue<Item>();

    private void Awake()
    {
        SetupQueue();
    }

    private void SetupQueue()
    {
        for (int i = 0; i < dartPrefabs.Count; i++)
        {
            dartObjectesQueue.Enqueue(dartPrefabs[i]);
            dartPrefabs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            itemObjectesQueue.Enqueue(itemPrefabs[i]);
            itemPrefabs[i].gameObject.SetActive(false);
        }
    }

    public Dart DartObjectPool()
    {
        var dartObject = dartObjectesQueue.Dequeue();
        dartObject.gameObject.SetActive(true);
        return dartObject;
    }

    public void DartObjectPick(Dart dartObject)
    {
        dartObjectesQueue.Enqueue(dartObject);
        dartObject.gameObject.SetActive(false);
    }

    public Item ItemObjectPool()
    {
        var itemObject = itemObjectesQueue.Dequeue();
        itemObject.gameObject.SetActive(true);
        return itemObject;
    }

    public void ItemObjectPick(Item itemObject)
    {
        itemObjectesQueue.Enqueue(itemObject);
        itemObject.gameObject.SetActive(false);
    }
}
