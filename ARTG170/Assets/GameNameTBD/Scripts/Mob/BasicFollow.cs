using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;


public enum MobType
{
    Scorpion,
    Snake,
    Slime
}

public class BasicFollow : MonoBehaviour
{
    [SerializeField] private float viewDistance = 100.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private MobType mob_type;

    [Header("Attack")]
    [SerializeField] private GameObject _eyeCast;

    [SerializeField] private AudioSource moveSound;
    [SerializeField] private AudioClip moving;
    private Animator mAnimator;
    private bool isWalking = false;



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
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PointToPlayer();
        DetectPlayer();
        if (isWalking)
        {
            //Debug.Log("iswalking");
            mAnimator.SetTrigger("Walk");
        }
       
    }
    private void PointToPlayer()
    {
        //float orig_x_rotation = transform.rotation.x;
        //Debug.Log(transform.rotation);
        Vector3 horizontalLook = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(horizontalLook);
        // TODO: This is a simple modeling change to apply rotation. Hardcoded for snake.
        if (mob_type == MobType.Snake) { transform.RotateAround(transform.position, transform.right, 0); }
        if (mob_type == MobType.Slime) { 
            //transform.RotateAround(transform.position, transform.right, -90);
            transform.RotateAround(transform.position, transform.up, 0);
            transform.RotateAround(transform.position, Vector3.up, 90);


         }
        
        //transform.rotation = new Quaternion(orig_x_rotation, transform.rotation.y, transform.rotation.z, transform.rotation.w);
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
                transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), speed * Time.deltaTime);
                isWalking = true;
            }
            else
            {
                
                Debug.DrawRay(origin, direction, Color.red);
            }
        }
    }

}
