using UnityEngine;
using UnityEngine.Events;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogicGameObject;

    [SerializeField] float weaponDamageBuff;

    [SerializeField] GameObject weaponPrefab = null;

    [SerializeField] Transform handTransform = null;    


   

    public void EnableHitbox()
    {
        weaponLogicGameObject.SetActive(true);
    } 

    public void DisableHitBox()
    {
        weaponLogicGameObject.SetActive(false);

    }
}
