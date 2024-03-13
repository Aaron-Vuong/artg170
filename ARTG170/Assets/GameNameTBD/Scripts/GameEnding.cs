using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnding : MonoBehaviour
{
    [Header("Winning")]
    [SerializeField] public GameObject table;
    [SerializeField] public GameObject carpet;
    [SerializeField] public GameObject bed;

    [SerializeField] public GameObject canvas;
   
    // Update is called once per frame
    void Update()
    {
        if(table == null && carpet == null && bed == null)
        {
            canvas.SetActive(true); 
        }
    }
}
