using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingInFurniture : MonoBehaviour
{
    [SerializeField] private GameObject furniture;

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Pickup")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            furniture.SetActive(true);
        }
    }
}
