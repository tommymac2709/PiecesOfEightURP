using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeObjectSaveManager : MonoBehaviour, ISaveable
{
    [SerializeField] public List<GameObject> objects = new List<GameObject>();

    public object CaptureState()
    {
        var objectsToAdd = FindObjectsOfType<RuntimeObject>();
        foreach (RuntimeObject obj in objectsToAdd)
        {
            objects.Add(obj.gameObject);
        }
        return objects.ToArray();
    }

    public void RestoreState(object state)
    {
        foreach (GameObject obj in objects)
        {
            Instantiate(obj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var objectsToAdd = FindObjectsOfType<RuntimeObject>(); 
        foreach (RuntimeObject obj in objectsToAdd)
        {
            objects.Add(obj.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
