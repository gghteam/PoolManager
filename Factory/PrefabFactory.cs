namespace Pool
{
    using UnityEngine;

    public class PrefabFactory<T> : IFactory<T> where T : MonoBehaviour
    {
        private GameObject prefab;
        private string name;
        int index = 0;

        public PrefabFactory(GameObject prefab) : this(prefab, prefab.name) { }

        public PrefabFactory(GameObject prefab, string name)
        {
            this.prefab = prefab;
            this.name = name;
        }

        public T Create()
        {            
            GameObject tempGameObject = GameObject.Instantiate(prefab) as GameObject;
            tempGameObject.name = name + index.ToString();
            T objectOfType = tempGameObject.GetComponent<T>();
            if (objectOfType == null) tempGameObject.AddComponent<T>();
            index++;
            return objectOfType;
        }
    }
}

