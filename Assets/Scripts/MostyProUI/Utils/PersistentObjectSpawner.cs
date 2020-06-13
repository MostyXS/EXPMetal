using UnityEngine;

namespace MostyProUI
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        
        [SerializeField] GameObject persistentObjectsPrefab = null;
        static bool hasSpawned = false;
        private void Awake()
        {
            if (hasSpawned) return;
            GameObject persistentObjects = Instantiate(persistentObjectsPrefab);
            DontDestroyOnLoad(persistentObjects);
            hasSpawned = true;
        }
    }
}