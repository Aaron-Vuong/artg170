using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera _camera;
    [Header("Player Data")]
    private Inventory _inventory;
    private float playerHeight = 1;
    [SerializeField] private HUDMenu _hudMenu;
    // PLAYER HP
    [Header("HP")]
    public int health = 5;
    public int maxHealth = 5;

    private float explosionRadius = 20f;
    private float explosionForce = 500f;

    [SerializeField] private bool canAttack = true;
    [SerializeField] private int attackTimeout = 3;
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode interactKey = KeyCode.F;
    public KeyCode dropKey = KeyCode.G;
    public KeyCode houseKey = KeyCode.H;

    [Header("Ground Check")]
    public PlayerCollider gCheck;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    // Start is called before the first frame update
    private void Start()
    {
        _inventory = GetComponent<Inventory>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        GameStateManager.Instance.hudMenu.SetHealth(health, maxHealth);

    }

    // Update is called once per frame
    private void Update()
    {
        if (_camera == null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (child.tag == "MainCamera")
                {
                    _camera = child.GetComponent<Camera>();
                    break;
                }
            }

        }

        if (_hudMenu == null)
        {
            _hudMenu = GameStateManager.Instance.hudMenu;
        }
        //ground check
        //grounded = Physics.Raycast(transform.position + new Vector3(0, -1, 0), Vector3.down, 1f);
        //Debug.DrawRay(transform.position + new Vector3(0, -1f, 0), Vector3.down, Color.yellow);
        Debug.Log($"isGrounded: {gCheck.grounded}");
        grounded = gCheck.grounded;
        //Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        MyInput();
        SpeedControl();

        //handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        Debug.DrawRay(new Vector3(transform.position.x, 2, transform.position.z), _camera.transform.forward * 3, Color.green);


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider != null)
                {
                    // Use the hit variable to determine what was clicked on.
                    hit.collider.gameObject.GetComponent<MobStats>().TakeDamage();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

       //when to jump
       if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

       if (Input.GetKey(interactKey))
       {
            Interact();
       }

       if (Input.GetKey(dropKey))
        {
            DropItem();
        }

       if(Input.GetKey(houseKey))
        {
            SceneManager.LoadScene("HouseLevel");
        }
    }

    private void MovePlayer()
    {
        //calculate movement
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
           // Debug.Log("grounded");

        }
        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            //Debug.Log("NOT grounded");
        }
        
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Interact()
    {
        RaycastHit hit;
        Debug.Log("Sending RayCast!!!");
        if (Physics.Raycast(new Vector3(transform.position.x, 2, transform.position.z), _camera.transform.forward, out hit, 3f))
        {
            Debug.Log($"Hit Something! {hit.collider.gameObject.name}");
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Pickup" && !_inventory.isFull())
                {
                    // If there are slots in your inventory.
                    // Despawn model
                    // Display it in hotbar.
                    // Manage the Inventory.
                    ItemInstance itemInstance = hit.collider.GetComponent<ItemInstance>();
                    Debug.Log($"ITEM INSTANCE1: {itemInstance}");
                    hit.collider.gameObject.SetActive(false);
                    _hudMenu.displaySpriteOnHotbar(itemInstance.itemType.icon, _hudMenu.getSelectedSlotIndex());
                    _inventory.AddItem(hit.collider.gameObject, _hudMenu.getSelectedSlotIndex());
                }
            }
        }
    }

    private void DropItem()
    {
        int selectedIdx = _hudMenu.getSelectedSlotIndex();
        GameObject storedObject = _inventory.RemoveItem(selectedIdx);
        if (storedObject != null)
        {
            _hudMenu.removeSpriteOnHotBar(selectedIdx);
            // Place it below our feet and make visible again.
            storedObject.transform.position = new Vector3(transform.position.x, 4, transform.position.z);
            storedObject.SetActive(true);
        }
    }

    public void SubtractHealth()
    {
        health -= 1;
        GameStateManager.Instance.hudMenu.SetHealth(health, maxHealth);
        if (health == 0)
        {
            SceneChangeManager.Load(SceneChangeManager.Scene.GameOver);
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "PlayerCollider" && canAttack)
        {
            other.gameObject.transform.parent.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
            other.gameObject.GetComponent<PlayerController>().SubtractHealth();
            // Start cooling down for the next attack.
            Debug.Log("ATTACKED!");
            canAttack = false;
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(attackTimeout);
        canAttack = true;
    }
}
