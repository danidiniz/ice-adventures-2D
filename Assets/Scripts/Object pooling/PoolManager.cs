using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {
    
    Dictionary<int, Queue<IcesDefault>> poolDictionary = new Dictionary<int, Queue<IcesDefault>>();

    static PoolManager _instance;

    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    public void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<IcesDefault>());

            GameObject poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = transform;

            for (int i = 0; i < poolSize; i++)
            {
                GameObject temp = Instantiate(prefab) as GameObject;
                temp.SetActive(false);
                IcesDefault newIce = temp.GetComponent(typeof(IcesDefault)) as IcesDefault;
                
                poolDictionary[poolKey].Enqueue(newIce);
                newIce.gameObject.transform.SetParent(poolHolder.transform);
            }
        }
    }

    
    public void ReuseObject(GameObject prefab, Vector2 position, Quaternion rotation, short posI, short posJ)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            IcesDefault objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.gameObject.transform.position = position;
            objectToReuse.gameObject.transform.rotation = rotation;

            // Inicio a posição do novo elemento
            objectToReuse.InitIce(posI, posJ);

            // Setando parent e posição na hierarquia
            /*
            novoElemento.transform.parent = MapCreator.instance.mapIcesParent;
            novoElemento.transform.SetSiblingIndex(posI * MapCreator.instance.Colunas + posJ);
            // Setando nome
            novoElemento.name = novoElementoComponente.GetName() + "[" + posI + "][" + posJ + "]";
            */

            // Atualizando o map[]
            MapCreator.map[posI, posJ] = objectToReuse;

            MapCreator.map[posI, posJ].elementoEmCimaDoIce = null;

            objectToReuse.gameObject.SetActive(true);
        }
    }

    public void ReuseObjectEmCima(GameObject prefab, Vector2 position, Quaternion rotation, short posI, short posJ)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            IcesDefault objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.gameObject.transform.position = position;
            objectToReuse.gameObject.transform.rotation = rotation;

            // Inicio a posição do novo elemento
            objectToReuse.InitIce(posI, posJ);

            // Setando parent e posição na hierarquia
            /*
            novoElemento.transform.parent = MapCreator.instance.mapIcesParent;
            novoElemento.transform.SetSiblingIndex(posI * MapCreator.instance.Colunas + posJ);
            // Setando nome
            novoElemento.name = novoElementoComponente.GetName() + "[" + posI + "][" + posJ + "]";
            */

            // Atualizando o que está em cima do ice
            MapCreator.map[posI, posJ].elementoEmCimaDoIce = objectToReuse;

            objectToReuse.gameObject.SetActive(true);
        }
    }
    

    /*
    public class ObjectInstance
    {

        GameObject gameObject;
        Transform transform;

        bool hasPoolObjectComponent;
        PoolObject poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }
        }

        public void Reuse(Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive(true);
            transform.position = position;
            transform.rotation = rotation;

            if (hasPoolObjectComponent)
            {
                poolObjectScript.OnObjectReuse();
            }
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }
    */
    
}
