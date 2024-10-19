using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] Transform contentRoot;
    [SerializeField] GameObject buttonPrefab;

    // Start is called before the first frame update
    void OnEnable()
    {
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();


        //Debug.Log("Loading list of save files");
        //foreach (Transform child in contentRoot)
        //{
        //    Destroy(child.gameObject);
        //}

        foreach (string save in wrapper.ListSaves())
        {
            GameObject buttonInstance = Instantiate(buttonPrefab, contentRoot);
            TMP_Text tmpText =  buttonInstance.GetComponentInChildren<TMP_Text>();
            tmpText.text = save;
            Button button = buttonInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() =>
            {
                wrapper.LoadGame(save);
            });
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
