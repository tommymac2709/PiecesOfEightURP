using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogicGameObject;

    public void EnableHitbox()
    {
        weaponLogicGameObject.SetActive(true);
    } 

    public void DisableHitBox()
    {
        weaponLogicGameObject.SetActive(false);
    }
}
