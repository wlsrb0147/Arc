using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);

        instance = this;

        DontDestroyOnLoad(gameObject);
    }
}