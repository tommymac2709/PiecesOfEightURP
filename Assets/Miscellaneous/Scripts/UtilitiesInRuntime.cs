using UnityEngine;

public class UtilitiesInRuntime : MonoBehaviour
{
    [SerializeField] InventorySO inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha9)) 
        {
            inventory.Save();
        }

        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            inventory.Load();
        }

    }

    private void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[16];
    }
}
