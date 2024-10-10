using UnityEngine;

public class UtilitiesInRuntime : MonoBehaviour
{
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void OnApplicationQuit()
    {
        
    }
}
