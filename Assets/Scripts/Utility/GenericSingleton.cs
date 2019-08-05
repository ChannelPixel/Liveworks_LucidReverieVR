using UnityEngine;

namespace ATT.Utility
{
    /// <summary>
    /// Generic implementation of Singleton allows for
    /// any object that needs to be a singleton and
    /// persist across scenes to inherit from this class.
    /// Reference: http://www.unitygeek.com/unity_c_singleton/
    /// </summary>
    public class GenericSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance = null;

        /// <summary>
        /// If instance is null, try and find it.
        /// If not found, create it, name it and add it.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        _instance = new GameObject("_" + typeof(T).Name).AddComponent<T>();
                        DontDestroyOnLoad(_instance);
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// For overriding in derived class.
        /// Ensures the object persists across scenes
        /// and any duplicates are destroyed.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}