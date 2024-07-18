using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogicGameObject;

    [SerializeField] AudioSource weaponSource;

    [SerializeField] AudioClip swingOne;

    [SerializeField] AudioClip swingTwo;

    [SerializeField] AudioClip hitOne;
    [SerializeField] AudioClip hitTwo;

    public void EnableHitbox()
    {
        weaponLogicGameObject.SetActive(true);
       
    }

    public void DisableHitBox()
    {
        weaponLogicGameObject.SetActive(false);
    }

    public void SwingOne()
    {
        weaponSource.Stop();
        weaponSource.PlayOneShot(swingOne);
    }

    public void SwingTwo()
    {
        weaponSource.Stop();
        weaponSource.PlayOneShot(swingTwo);
    }

    public void HitOne()
    {
        weaponSource.PlayOneShot(hitOne);
    }

    public void HitTwo()
    {
        weaponSource.PlayOneShot(hitTwo);
    }

}
