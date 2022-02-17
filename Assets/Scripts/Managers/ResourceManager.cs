using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public T ReourceLoad<T>(string path) where T : UnityEngine.Object
    {
        var type = typeof(T);
        return Resources.Load<T>(path);
    }
}
