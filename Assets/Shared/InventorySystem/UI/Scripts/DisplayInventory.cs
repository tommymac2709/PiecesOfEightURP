using Language.Lua;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();

    public GameObject inventoryPrefab;
    public InventorySO inventory;
    public GameObject gridParent;

    [SerializeField] GameObject toolTipVisual;
    [SerializeField] TextMeshProUGUI weaponDisplayName;
    [SerializeField] Image weaponDisplaySprite;
    [SerializeField] TextMeshProUGUI weaponDisplayDescription;

    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    PlayerStateMachine stateMachine;

    private void Awake()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
    }

    private void Start()
    {
        stateMachine = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
        CreateSlots();
    }


    private void Update()
    {
        UpdateSlotDisplay();
    }

    public void UpdateSlotDisplay()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if (_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? " " : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = " ";
            }
        }
    }

    private void CreateSlots()
    {
        Debug.Log("Created slots");
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        if (itemsDisplayed != null)
        {
            Debug.Log("Created dictionary");
        }
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, gridParent.transform, false);
            Debug.Log("Instantiated " + obj.name);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClicked(obj); });


            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }

    

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);

    }

    private void OnClicked(GameObject obj)
    {
        stateMachine.Fighter.EquipWeapon(itemsDisplayed[obj].item.weaponConfig);
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj) )
        {
            mouseItem.hoverItem = itemsDisplayed[obj];

            if (itemsDisplayed[obj].ID < 0) return;

            toolTipVisual.SetActive(true);
            weaponDisplayName.text = inventory.database.GetItem[itemsDisplayed[obj].ID].itemTooltip.itemName;
            weaponDisplayDescription.text = inventory.database.GetItem[itemsDisplayed[obj].ID].itemTooltip.itemDescription;
            weaponDisplaySprite.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].itemTooltip.itemSprite;

        }

    }

    
    private void OnDrag(GameObject obj)
    {
        
            if (mouseItem.obj != null)
            {
                mouseItem.obj.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();  //new input mouse position
            }
        
    }

    private void OnDragEnd(GameObject obj)
    {
        if (mouseItem.hoverObj)
        {
            inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
           // inventory.RemoveItem(itemsDisplayed[obj].item);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }

    private void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();

        rt.sizeDelta = new Vector2(80, 80);
        mouseObject.transform.SetParent(obj.transform);
        if (itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;

        }
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }

    private void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
        toolTipVisual.SetActive(false);
        weaponDisplayName.text = null;
        weaponDisplayDescription.text = null;
        weaponDisplaySprite.sprite = null;

    }
}

public class MouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}