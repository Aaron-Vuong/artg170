using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class BasicFollow : MonoBehaviour
{
    [SerializeField] private float viewDistance = 100.0f;
    [SerializeField] private float speed = 5.0f;


    [Header("Attack")]
    [SerializeField] private GameObject _eyeCast;

    [SerializeField] private AudioSource moveSound;
    [SerializeField] private AudioClip moving;



    private GameObject player;
    int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerCollider");
        layerMask = 1 << 10;
        layerMask = ~layerMask;
        moveSound.clip = moving;
        moveSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        PointToPlayer();
        DetectPlayer();
        
    }
    private void PointToPlayer()
    {
        Vector3 horizontalLook = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(horizontalLook);
    }
    private void DetectPlayer()
    {
        Vector3 direction = player.transform.position - _eyeCast.transform.position;
        RaycastHit hit;
        Vector3 origin = _eyeCast.transform.position;
        if (Physics.Raycast(origin, direction, out hit, viewDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"HIT THE PLAYER! {hit.collider.gameObject.tag}");
            if (hit.collider.gameObject.tag == "PlayerCollider")
            {
               
                Debug.Log("Finding Player!");
                Debug.DrawRay(origin, direction, Color.blue);
                transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 1, transform.position.z), new Vector3(player.transform.position.x, 1, player.transform.position.z), speed * Time.deltaTime);
                
            }
            else
            {
                Debug.DrawRay(origin, direction, Color.red);
            }
        }
    }

}
