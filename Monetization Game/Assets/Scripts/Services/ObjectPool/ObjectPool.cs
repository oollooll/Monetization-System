using System.Collections.Generic;
using UnityEngine;

namespace Services.ObjectPool
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        public List<T> pool = new List<T>();
        private T prefab;
        private Transform parentTransform;

        public ObjectPool(T prefab, int initialPoolSize, Transform parent = null)
        {
            this.prefab = prefab;
            this.parentTransform = parent;

            for (int i = 0; i < initialPoolSize; i++)
            {
                T obj = Object.Instantiate(prefab);
                obj.gameObject.SetActive(false);
                pool.Add(obj);
            }
        }

        public T Get()
        {
            foreach (var obj in pool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
            
            T newObj = Object.Instantiate(prefab);
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}