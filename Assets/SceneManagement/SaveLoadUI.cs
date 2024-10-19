using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] Transform contentRoot;
    [SerializeField] GameObject buttonPrefab;

    // Start is called before the first frame update
    void OnEnable()
    {
        Debug.Log("Loading list of save files");
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 4; i++)
        {
            Instantiate(buttonPrefab, contentRoot);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
