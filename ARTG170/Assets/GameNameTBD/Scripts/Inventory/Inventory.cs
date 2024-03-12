using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Inventory : MonoBehaviour
{
    public int maxItems = 5;
    public List<GameObject> items = new List<GameObject>();
    private void Start()
    {
        // Preinitialize list with nulls
        for (int i = 0; i < maxItems; i++) { items.Add(null); }
    }
    public bool AddItem(GameObject itemToAdd, int slotIdx)
    {  
        if (items[slotIdx] == null)
        {
            items[slotIdx] = itemToAdd;
            Debug.Log(itemToAdd.ToString());
            return true;
        }
        return false;
    }
    public GameObject RemoveItem(int slotIdx)
    {
        if (items[slotIdx] == null)
        {
            return null;
        }
        GameObject savedGameObject = items[slotIdx];
        items[slotIdx] = null;
        return savedGameObject;
    }
    public bool isFull()
    {
        return (!items.Contains(null));
    }
    public void Clear()
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject item = RemoveItem(i);
            Destroy(item);
        }
    }
}
