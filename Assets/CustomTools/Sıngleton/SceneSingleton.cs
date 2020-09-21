using UnityEngine;

public class SceneSingleton<T> : MonoBehaviour
    where T : Component
{
    static T s_instance = null;

    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = Object.FindObjectOfType<T>();
            }

            return s_instance;
        }
    }
}