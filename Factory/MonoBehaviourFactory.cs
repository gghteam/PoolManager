namespace Pool {
    using UnityEngine;

    public class MonoBehaviourFactory<T> : IFactory<T> where T : MonoBehaviour
    {
        private string name;
        int index = 0;

        public MonoBehaviourFactory() : this("MonoBehaviour") { }

        public MonoBehaviourFactory(string name)
        {
            this.name = name;
        }

        public T Create()
        {
            GameObject tempGameObject = GameObject.Instantiate(new GameObject()) as GameObject;
            tempGameObject.name = name + index.ToString();
            T objectOfType = tempGameObject.AddComponent<T>();
            index++;
            return objectOfType;
        }
    }
}
