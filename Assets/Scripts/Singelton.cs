using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static public T Instance { get; set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = (T) (object) this;
        }
    }

}