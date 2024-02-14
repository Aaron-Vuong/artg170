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

public class HUDMenu : MenuManager
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _selectedIndex = 0;

    [Header("Player Info")]
    [SerializeField] private TMP_Text _healthText;

    [Header("Inventory Hotbar")]
    [SerializeField] private int _numSlots = 5;
    [SerializeField] private int _slotSpacing = 0;
    [SerializeField] private List<Vector2> _slots;
    [SerializeField] private List<UnityEngine.UI.Image> _itemImages;
    [SerializeField] private Transform _hotbarOrigin;
    [SerializeField] private Sprite _slotImage;
    [SerializeField] private Sprite _selectionBorderImage;
    private GameObject _selectionBorder;
    private int _offsetFactor = 2;
    protected override void InnerAwake()
    {
        menuType = GameMenu.GameHUD;
    }
    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        instantiateTiledHotbar();
        _selectionBorder = new GameObject();
        UnityEngine.UI.Image selectionImage = _selectionBorder.AddComponent<UnityEngine.UI.Image>();
        selectionImage.sprite = _selectionBorderImage;// new Vector3((float)1 / _offsetFactor, (float)1 / _offsetFactor, (float)1 / _offsetFactor);
        _selectionBorder.GetComponent<RectTransform>().SetParent(_canvas.transform);
        _selectionBorder.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        _selectionBorder.name = "Slot Border";
    }
    private void Update()
    {
        /*
        // On User Escape, we go to the settings.
        if (Input.GetKeyDown(KeyCode.Escape) && _uiManager.getCurrentMenu() == GameMenu.GameHUD)
        {
            _uiManager.GoToMenu(GameMenu.Settings);
        }
        */
        updateHotbarSelection();
    }

    //TODO: Debug Evens (where no tile is at 0)
    private void instantiateTiledHotbar()
    {
        bool isEven = false;
        float slotOffset = 0;
        if (_numSlots % 2 == 0) { isEven  = true; }

        // This DOES NOT factor rotation! (Hotbar should always be flat/same y.)
        slotOffset = ((_slotImage.bounds.max.x - _slotImage.bounds.min.x) / _offsetFactor) + _slotSpacing;
        Debug.Log($"Offset of the slot: {slotOffset}");

        // Handle tiles on the left + right of origin.
        int offCenterSlots = Mathf.FloorToInt(_numSlots / 2);
        int rightSlots = offCenterSlots;
        if (!isEven) { rightSlots += 1; }
        for (int slotIdx = -offCenterSlots; slotIdx < rightSlots; slotIdx++)
        {
            Vector3 slotPosition = new Vector3((slotOffset * slotIdx), _hotbarOrigin.transform.position.y, 10);
            // Internally store the position of each tile.
            _slots.Add(new Vector2(slotPosition.x, slotPosition.y));
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
            Debug.Log($"Position: {slotPosition}");
            Debug.Log($"currentImageScale {newSlot.transform.localScale} {newSlot.transform.position}");
            
            newSlot.SetActive(true);
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
        _selectionBorder.transform.position = new Vector3(selectionImagePlacement.x, selectionImagePlacement.y, 9);
        // TODO: UGLY CONSTANTS!!! (Figure out the image scaling problem here.)
        _selectionBorder.transform.localPosition = new Vector3(selectionImagePlacement.x * 4.429f, selectionImagePlacement.y * 4.5f, 9);

        //Debug.Log($"Selected Slot {_selectedIndex}, {selectionImagePlacement}, {_selectionBorder.transform.position}");
    }

    public void SetHealth(int health, int maxHealth)
    {
        _healthText.text = $"Health: {health}/{maxHealth}";
    }


    public void Houselevel()
    {
        SceneManager.LoadScene("HouseLevel");
    }
}
