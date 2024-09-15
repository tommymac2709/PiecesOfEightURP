using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject _containerGameObject;
    [SerializeField] private PlayerInteract _playerInteract;
    [SerializeField] private TextMeshProUGUI _interactText;

    private void Update()
    {
        if (_playerInteract.GetInteractableObject() != null)
        {
            Show(_playerInteract.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }

    private void Show(IInteractable interactable)
    {
        _containerGameObject.SetActive(true);
        _interactText.text = interactable.GetInteractText();

        
    }

    private void Hide()
    {
        _containerGameObject.SetActive(false);

    }
}
