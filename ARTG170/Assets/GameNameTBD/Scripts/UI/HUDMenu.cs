using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.FullSerializer;

public class HUDMenu : MenuManager
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _selectedIndex = 0;


    [Header("Player Camera")]
    [SerializeField] private PlayerCam _playerCam;
    [Header("Player Info")]
    [SerializeField] private TMP_Text _healthText;
    [Header("Crosshair")]
    [SerializeField] private UnityEngine.UI.Image _crosshair;
    [SerializeField] private float _crosshairScale = 0.2f;
    [SerializeField] private Color _crosshairColor;
    [SerializeField] private TMP_Text _pickupTip;
    public bool isPickupVisible = false;
    public bool isFurnitureVisible = false;

    [Header("Inventory Hotbar")]
    [SerializeField] private int _numSlots = 5;
    [SerializeField] private int _slotSpacing = 0;
 
    [SerializeField] private List<Vector2> _slots;
    [SerializeField] private List<UnityEngine.UI.Image> _itemImages;
    [SerializeField] private Transform _hotbarOrigin;
    [SerializeField] private Sprite _slotImage;
    [SerializeField] private Sprite _selectionBorderImage;
    private GameObject _selectionBorder;
    private int _offsetFactor = 1;
    [Header("Scene")]
    public bool inHouseLevel = false;
    private string lastGameSceneName;
    [SerializeField] private PlayerController _controller;
    protected override void InnerAwake()
    {
        menuType = GameMenu.GameHUD;
    }
    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        instantiateTiledHotbar();
        _selectionBorder = new GameObject();
        
        UnityEngine.Assertions.Assert.IsNotNull(_pickupTip);
        initializeSelectionBorder();
        createCrosshair();
    }
    private void Update()
    {
        // On User Escape, we go to the settings.
        if (Input.GetKeyDown(KeyCode.Escape) && _uiManager.getCurrentMenu() == GameMenu.GameHUD)
        {
            _uiManager.GoToMenu(GameMenu.Settings);
        }
        updateHotbarSelection();
    }

    private void createCrosshair() {
        _crosshair.transform.localScale = new Vector3(_crosshairScale, _crosshairScale, _crosshairScale);
    }
    private void initializeSelectionBorder() {
        UnityEngine.UI.Image selectionImage = _selectionBorder.AddComponent<UnityEngine.UI.Image>();
        selectionImage.sprite = _selectionBorderImage;
        _selectionBorder.GetComponent<RectTransform>().SetParent(_canvas.transform);
        _selectionBorder.transform.localScale = new Vector3(1f, 1f, 1f);
        _selectionBorder.name = "Slot Border";

    }
    //TODO: Debug Evens (where no tile is at 0)
    private void instantiateTiledHotbar()
    {
        bool isEven = false;
        float slotOffset = 0;
        if (_numSlots % 2 == 0) { isEven  = true; }

        // This DOES NOT factor rotation! (Hotbar should always be flat/same y.)
        slotOffset = ((_slotImage.bounds.max.x - _slotImage.bounds.min.x) / _offsetFactor) + _slotSpacing - 150;
        Debug.Log($"Offset of the slot: {slotOffset}");

        // Handle tiles on the left + right of origin.
        int offCenterSlots = Mathf.FloorToInt(_numSlots / 2);
        int rightSlots = offCenterSlots;
        if (!isEven) { rightSlots += 1; }
        for (int slotIdx = -offCenterSlots; slotIdx < rightSlots; slotIdx++)
        {
            //bug happened when I changed everything over to Overlay from Camera so this is my fix for showing TA
            Vector3 slotPosition = new Vector3((slotOffset * slotIdx) + 900, _hotbarOrigin.transform.position.y, 0);
            // Generate a new tile.
            GameObject newSlot = new GameObject();
            newSlot.name = $"Slot {slotIdx + offCenterSlots}";
            UnityEngine.UI.Image newImage = newSlot.AddComponent<UnityEngine.UI.Image>();
            newImage.sprite = _slotImage;
            _itemImages.Add(newImage);
            newSlot.GetComponent<RectTransform>().SetParent(_canvas.transform);
            newSlot.transform.localScale = new Vector3((float)1/_offsetFactor, (float)1 /_offsetFactor, (float)1 /_offsetFactor);
            newSlot.transform.position = slotPosition;
            newSlot.transform.localPosition = new Vector3(newSlot.transform.localPosition.x, newSlot.transform.localPosition.y, 10);
            // Internally store the position of each tile.
            _slots.Add(new Vector2(newSlot.transform.localPosition.x, newSlot.transform.localPosition.y));
            Debug.Log($"Position: {slotPosition}");
            Debug.Log($"currentImageScale {newSlot.transform.localScale} {newSlot.transform.position}");
            
            newSlot.SetActive(true);
        }
        foreach (var img in _itemImages) {
            Debug.Log($"{img}");
        }
    }
    public Vector2 getInventorySlotPosition(int slotIdx)
    {
        // Handle Out Of Bounds.
        if (slotIdx > _numSlots || slotIdx < 0) { return new Vector2(-1, -1); }

        return _slots[slotIdx];
    }

    public void displaySpriteOnHotbar(Sprite itemImage, int slotIdx = -1)
    {
        Debug.Log($"SLOT: {slotIdx}, {_itemImages}");
        foreach (var img in _itemImages) {
            Debug.Log($"{img}");
        }
        // SlotIDX will default to -1 to indicate any free spot.
        UnityEngine.UI.Image imageHolder = _itemImages[slotIdx];
        imageHolder.sprite = itemImage;
        Debug.Log($"Displaying image {itemImage} on slot {slotIdx}");
    }

    public void removeSpriteOnHotBar(int slotIdx)
    {
        UnityEngine.UI.Image imageHolder = _itemImages[slotIdx];
        imageHolder.sprite = _slotImage;
    }

    public int getSelectedSlotIndex()
    {
        return Math.Abs(_selectedIndex);
    }

    // TODO: Disable so it only runs on scroll event.
    private void updateHotbarSelection()
    {
        _selectedIndex += (int)Input.mouseScrollDelta.y;
        if (_selectedIndex < 0 ) { _selectedIndex = _numSlots - 1; }
        _selectedIndex = _selectedIndex % _numSlots;
        // Move the selection image to the new slot.
        Vector2 selectionImagePlacement = getInventorySlotPosition(Math.Abs(_selectedIndex));
        _selectionBorder.transform.localPosition = new Vector3(selectionImagePlacement.x, selectionImagePlacement.y, 9);
    }

    public void SetHealth(int health, int maxHealth)
    {
        _healthText.text = $"Health: {health}/{maxHealth}";
    }

    public void displayPickupTooltip()
    {
        if (!isPickupVisible) {
            _pickupTip.text = "Pickup (F)";
            isPickupVisible = true;
        }
       
    }

    public void hidePickupTooltip() {
        if (isPickupVisible) {
            _pickupTip.text = "";
            isPickupVisible = false;
        }
        
    }
    public void displayFurnitureTooltip()
    {
       
        if (!isFurnitureVisible)
        {
            _pickupTip.text = "When next to furniture, Press (G) to drop Crystal ball";
            isFurnitureVisible = true;
        }
    }

    public void hideFurnitureTooltip()
    {
        
        if (isFurnitureVisible)
        {
            _pickupTip.text = "";
            isFurnitureVisible = false;
        }
    }

    // TODO: This will reset a player's progress since we reload the scene.
    // We need to additively load the scenes and then choose which is the active.
    public void loadHouse()
    {
        if (!inHouseLevel) {
            Scene lastGameScene = SceneManager.GetActiveScene();
            lastGameSceneName = lastGameScene.name;
            Debug.Log($"Saved Last Scene as {lastGameSceneName}");
            SceneManager.LoadScene("HouseLevel");
            _controller.spawned = false;
            inHouseLevel = true;
        }
        else {
            SceneManager.LoadScene(lastGameSceneName);
            // TODO: Apply Scene Persistence between scenes (player position, which coins have been picked up, and if enemy is killed.)
            inHouseLevel = false;
            _controller.spawned = false;
        }

    }

}
