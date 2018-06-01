using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    Dictionary<int, Queue<ElementoDoMapa>> poolDictionary = new Dictionary<int, Queue<ElementoDoMapa>>();

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
            poolDictionary.Add(poolKey, new Queue<ElementoDoMapa>());

            GameObject poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = transform;

            for (int i = 0; i < poolSize; i++)
            {
                // temp
                int r = Random.Range(0, 100);


                GameObject temp = Instantiate(prefab) as GameObject;

                // temp
                temp.name += " " + r.ToString();

                temp.SetActive(false);
                ElementoDoMapa newIce = temp.GetComponent(typeof(ElementoDoMapa)) as ElementoDoMapa;

                poolDictionary[poolKey].Enqueue(newIce);
                newIce.gameObject.transform.SetParent(poolHolder.transform);

                // temp
                if (newIce.Elemento == MapCreator.elementosPossiveisNoMapa.ICE)
                    Debug.Log("Coloquei na fila " + temp.name);
            }
        }
    }


    public void ReuseObject(GameObject prefab, Vector2 position, Quaternion rotation, short posI, short posJ)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            // Como os elementos não são 'destruídos' automaticamente,
            // o Object Pooling estava dando erro uma hora, pois estava
            // re usando um elemento que estava ativo na cena..
            // Dessa forma eu não permito isso
            while (poolDictionary[poolKey].Peek().gameObject.activeSelf)
            {
                // Tiro elemento ativo do início da fila
                ElementoDoMapa temp = poolDictionary[poolKey].Dequeue();
                // Coloco ele no final da fila
                poolDictionary[poolKey].Enqueue(temp);
                // Faço isso até encontrar o elemento que está desativado para poder usá-lo

                //Debug.Log("Elemento " + temp.gameObject.name + " estava ativo na cena e foi para o final da fila.");
            }

            ElementoDoMapa objectToReuse = poolDictionary[poolKey].Dequeue();

            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.gameObject.transform.position = position;
            objectToReuse.gameObject.transform.rotation = rotation;

            // Inicio a posição do novo elemento
            objectToReuse.setPosition(posI, posJ);

            // Setando parent e posição na hierarquia
            /*
            novoElemento.transform.parent = MapCreator.instance.mapIcesParent;
            novoElemento.transform.SetSiblingIndex(posI * MapCreator.instance.Colunas + posJ);
            // Setando nome
            novoElemento.name = novoElementoComponente.GetName() + "[" + posI + "][" + posJ + "]";
            */

            // Atualizando o map[]
            MapCreator.map[posI, posJ] = (IcesDefault)objectToReuse;

            MapCreator.map[posI, posJ].elementoEmCimaDoIce = null;

            objectToReuse.gameObject.SetActive(true);
        }
    }

    public void ReuseObjectEmCima(GameObject prefab, Vector2 position, Quaternion rotation, short posI, short posJ)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            ElementoDoMapa objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.gameObject.transform.position = position;
            objectToReuse.gameObject.transform.rotation = rotation;

            // Inicio a posição do novo elemento
            objectToReuse.setPosition(posI, posJ);

            // Setando parent e posição na hierarquia
            /*
            novoElemento.transform.parent = MapCreator.instance.mapIcesParent;
            novoElemento.transform.SetSiblingIndex(posI * MapCreator.instance.Colunas + posJ);
            // Setando nome
            novoElemento.name = novoElementoComponente.GetName() + "[" + posI + "][" + posJ + "]";
            */

            // Atualizando o que está em cima do ice
            MapCreator.map[posI, posJ].elementoEmCimaDoIce = (ObjetoDoMapa)objectToReuse;

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
