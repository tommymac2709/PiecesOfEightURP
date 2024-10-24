using Unity.VisualScripting;
using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// To be placed at the root of a Pickup prefab. Contains the data about the
    /// pickup such as the type of item and the number.
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        // STATE
        InventoryItem item;
        int number = 1;

        // CACHED REFERENCE
        Inventory inventory;

        // LIFECYCLE METHODS

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            inventory = player.GetComponent<Inventory>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag != "Player"  && collision.gameObject.tag != "Pickup")
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.isKinematic = true;
                GetComponent<BoxCollider>().enabled = false;
            }
            
        }

        // PUBLIC

        /// <summary>
        /// Set the vital data after creating the prefab.
        /// </summary>
        /// <param name="item">The type of item this prefab represents.</param>
        /// <param name="number">The number of items represented.</param>
        public void Setup(InventoryItem item, int number)
        {
            this.item = item;
            if (!item.IsStackable())
            {
                number = 1;
            }
            this.number = number;
        }

        public InventoryItem GetItem()
        {
            return item;
        }

        public int GetNumber()
        {
            return number;
        }

        public void PickupItem()
        {
            Fighter playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            StatsEquipment equipment = GameObject.FindWithTag("Player").GetComponent<StatsEquipment>();
            var weapon = item as WeaponConfig;
            if (equipment.GetItemInSlot(weapon.GetAllowedEquipLocation()) == null)
            {
                Destroy(gameObject);
                equipment.AddItem(weapon.GetAllowedEquipLocation(), weapon);
                return;
            }
            else if (equipment.GetItemInSlot(weapon.GetAllowedEquipLocation()) != null)
            {
                bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
                if (foundSlot)
                {
                    Destroy(gameObject);
                }


            

            
            
               
              
                
                
                
            }
        }

        public bool CanBePickedUp()
        {
            return inventory.HasSpaceFor(item);
        }
    }
}