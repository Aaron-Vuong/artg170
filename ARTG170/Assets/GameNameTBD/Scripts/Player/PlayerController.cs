using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [Header("Audio")]
    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip weaponAttackSoundClip;
    [SerializeField] private AudioClip interactSoundClip;
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private AudioClip chestSoundClip;
    [SerializeField] private AudioClip jumpSoundClip;

    [Header("Weapons")]
    [SerializeField] private GameObject dagger;
    private Animation daggerAnimation;

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Player Data")]
    private Inventory _inventory;
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
    public bool spawned = true;
    List<GameObject> visited_spawnPoints = new List<GameObject>();

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode interactKey = KeyCode.F;
    public KeyCode dropKey = KeyCode.G;
    

    [Header("Ground Check")]
    public PlayerCollider gCheck;
    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Player Spawn")]
    private string current_scene = "MainMenuScene";

    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    // Start is called before the first frame update
    private void Start()
    {
        // Preserve the Player.
        DontDestroyOnLoad(this.gameObject);
        _inventory = GetComponent<Inventory>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerAudioSource = GetComponent<AudioSource>();
        daggerAnimation = dagger.GetComponent<Animation>();
        readyToJump = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!spawned)
        {
            Debug.Log("Placing Player in specific SpawnPoint location!");
            spawned = placePlayerAtSpawnPoint(Vector3.zero);
        }
        RaycastHit hit;
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
            _hudMenu.SetHealth(health, maxHealth);
        }
        //ground check
        //grounded = Physics.Raycast(transform.position + new Vector3(0, -1, 0), Vector3.down, 1f);
        //Debug.DrawRay(transform.position + new Vector3(0, -1f, 0), Vector3.down, Color.yellow);
        grounded = gCheck.grounded;

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
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        
        Debug.DrawRay(mousePosition, _camera.transform.forward * 5, Color.green);
        if (Physics.Raycast(ray, out hit, 5f))
        {
            Debug.Log($"Hovering over something! {hit.collider.gameObject.name}");
            if (hit.collider != null)
            {
                // Display key to pickup.
                if (hit.collider.gameObject.tag == "Pickup")
                {
                    _hudMenu.displayPickupTooltip();
                }
                else if(hit.collider.gameObject.tag == "Furniture")
                {
                    _hudMenu.displayFurnitureTooltip();
                }
                else if (hit.collider.gameObject.tag == "Chest") //chest
                {
                    // play chest animation
                    // play chest sound

                    Animation newAnim = hit.collider.GetComponent<Animation>();
                    newAnim.Play();
                    BoxCollider box = hit.collider.gameObject.GetComponent<BoxCollider>();
                    box.enabled = false;
                    // Play audio for chest opening
                    playerAudioSource.PlayOneShot(chestSoundClip);
                }
                else {
                    _hudMenu.hidePickupTooltip();
                    _hudMenu.hideFurnitureTooltip();
                }
                
            }
        }
        else {
            _hudMenu.hidePickupTooltip();
            _hudMenu.hideFurnitureTooltip();
        }

        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            ray = _camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider != null)
                {
                    // Use the hit variable to determine what was clicked on.
                    hit.collider.gameObject.GetComponent<MobStats>().TakeDamage();
                    //play animation
                    daggerAnimation.Play();
                    // Play audio for the swing of the weapon.
                    playerAudioSource.PlayOneShot(weaponAttackSoundClip);
                }
            }

            


        }
    }

    public bool placePlayerAtSpawnPoint(Vector3? new_position)
    {
        // We can request to not place a PlayerAtSpawnPoint as well.
        if (new_position == null)
        {
            return false;
        }
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        // We poll for a different scene.
        if (scene == null || scene.name == current_scene || scene.name == "Loading")
        {
            return false;
        }
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Debug.Log($"{spawnPoints}");
        // Look through all of the different spawnPoints and choose the latest one.
        if (spawnPoints.Length > 0) {
            GameObject target_spawn = spawnPoints[spawnPoints.Length - 1];
            Debug.Log(target_spawn.name);
            // Add this to the list of visited spawn points.
            visited_spawnPoints.Add(target_spawn);
            Debug.Log($"Changing player position to {target_spawn.transform.position}");
            transform.position = target_spawn.transform.position;
            current_scene = scene.name;
            return true;
        }
        else
        {
            Debug.Log("SpawnPoints not present! Can't move player to valid position!");
            return false;
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
        playerAudioSource.PlayOneShot(jumpSoundClip);
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
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        Debug.Log("Sending RayCast!!!");
        if (Physics.Raycast(ray, out hit, 5f))
        {
            Debug.Log($"Hit Something! {hit.collider.gameObject.name}");
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.tag == "Pickup");
                /* If the item can be picked up and you are hovering over an empty slot. 
                 * TODO: Or a slot that contains the same item (increment number.)
                 */
                if (hit.collider.gameObject.tag == "Pickup" && _inventory.items[_hudMenu.getSelectedSlotIndex()] == null)
                {
                    /*
                     * Despawn model
                     * Display it in hotbar.
                     * Manage the Inventory.
                     */
                    ItemInstance itemInstance = hit.collider.GetComponent<ItemInstance>();
                    Debug.Log($"ITEM INSTANCE1: {itemInstance}");
                    hit.collider.gameObject.SetActive(false);
                    hit.collider.gameObject.transform.parent = transform;
                    _hudMenu.displaySpriteOnHotbar(itemInstance.itemType.icon, _hudMenu.getSelectedSlotIndex());
                    _inventory.AddItem(hit.collider.gameObject, _hudMenu.getSelectedSlotIndex());

                    // Play audio for the pickup
                    playerAudioSource.PlayOneShot(interactSoundClip);
                }
               

            }
        }
    }

    private void DropItem()
    {
        int selectedIdx = _hudMenu.getSelectedSlotIndex();
        GameObject storedObject = _inventory.RemoveItem(selectedIdx);
        Debug.Log($"Item: {storedObject} was dropped!");
        if (storedObject != null)
        {
            Debug.Log("not null");
            _hudMenu.removeSpriteOnHotBar(selectedIdx);
            // Re-enable physics on this object.
            storedObject.GetComponent<Rigidbody>().isKinematic = false;
            // Re-parent to scene;
            storedObject.transform.parent = null;
            // Place it above our head and make visible again.
            storedObject.transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
            // Rotate so it bounces off the player's head.
            storedObject.transform.rotation = Random.rotation;
            storedObject.SetActive(true);
        }
        else
        {
            Debug.Log("null");
        }
    }

    public void SubtractHealth()
    {
        health -= 1;
        playerAudioSource.PlayOneShot(damageSoundClip);
        _hudMenu.SetHealth(health, maxHealth);
        if (health == 0)
        {
            SceneChangeManager.Load(SceneChangeManager.Scene.MainGameScene);
            health = 5;
            _hudMenu.SetHealth(health, maxHealth);
            spawned = false;
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

    public void loadLevel2()
    {
        SceneChangeManager.Load(SceneChangeManager.Scene.Level2);
        // IMPORTANT: Make us look for the next spawnpoint on the scene load.
        spawned = false;
    }

    public void loadLevel3()
    {
        SceneChangeManager.Load(SceneChangeManager.Scene.Level3);
        // IMPORTANT: Make us look for the next spawnpoint on the scene load.
        spawned = false;
    }
}
