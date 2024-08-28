using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStartInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactText;
    [SerializeField] private string _questName;

    public void Interact(Transform transform)
    {
        if (!PixelCrushers.DialogueSystem.QuestLog.IsQuestActive(_questName))
        {
            PixelCrushers.DialogueSystem.QuestLog.StartQuest(_questName);
            Debug.Log(_questName + " quest has been started");
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.enabled = false;
        }
        
    }

    public string GetInteractText()
    {
        return _interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}