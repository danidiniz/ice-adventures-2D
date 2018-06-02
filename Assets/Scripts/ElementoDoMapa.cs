using System;
using UnityEngine;

public class ElementoDoMapa : MonoBehaviour {

    public bool isWalkable;
    public bool pararMovimentoDeQuemPassarPorCima;

    [SerializeField]
    protected short posI;
    [SerializeField]
    protected short posJ;

    [SerializeField]
    private MapCreator.elementosPossiveisNoMapa elementoNoMapa;
    [SerializeField]
    private MapCreator.tipoDeElemento tipoDoElemento;

    [SerializeField]
    string nameDoElemento;

    #region Getters and Setters
    public short PosI
    {
        get
        {
            return posI;
        }

        set
        {
            posI = value;
        }
    }

    public short PosJ
    {
        get
        {
            return posJ;
        }

        set
        {
            posJ = value;
        }
    }

    public string NameDoElemento
    {
        get
        {
            return nameDoElemento;
        }

        set
        {
            nameDoElemento = value;
        }
    }

    public MapCreator.elementosPossiveisNoMapa ElementoNoMapa
    {
        get
        {
            return elementoNoMapa;
        }

        set
        {
            elementoNoMapa = value;
        }
    }

    public MapCreator.tipoDeElemento TipoDoElemento
    {
        get
        {
            return tipoDoElemento;
        }

        set
        {
            tipoDoElemento = value;
        }
    }
    #endregion

    public void setPosition(short i, short j)
    {
        posI = i;
        posJ = j;
    }

    public virtual void OnMouseDown()
    {
        if (MapCreator.instance.modoCriarMapaAtivado)
        {
            
        }
    }

    public virtual void SerTransformadoEm(MapCreator.elementosPossiveisNoMapa elemento)
    {
        GameObject prefabDoElemento = MapCreator.instance.RetornarElemento(elemento);

        PoolManager.instance.ReuseObject(prefabDoElemento, transform.position, transform.rotation, posI, posJ);

        gameObject.SetActive(false);
    }

    // Nada mais é que um Construtor
    public virtual void CopiarInformacoesDesseElementoPara(ElementoDoMapa target)
    {
        try
        {
            // Informações importantes de qualquer Elemento do Mapa
            // MAIS IMPORTANTE DE TODAS : POSICAO. DEMOREI MAS DESCOBRI. -.-'
            // tipo do elemento
            // tipo de elemento
            // isWalkable
            // pararMovimentoDeQuemPassarPorCima

            //Debug.Log("Vai copiar elemento " + Elemento + " para target que é " + target.Elemento);

            target.setPosition(posI, posJ);
            target.TipoDoElemento = tipoDoElemento;
            target.isWalkable = isWalkable;
            target.pararMovimentoDeQuemPassarPorCima = pararMovimentoDeQuemPassarPorCima;

        }
        catch (Exception e)
        {
            Debug.Log("Nao copiou ElementoDoMapa. Erro: " + e);
        }
        
    }

    public virtual void ResetarInformacoesDoElemento()
    {
    }

}
