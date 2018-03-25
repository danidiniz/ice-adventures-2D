using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementoDoMapa : MonoBehaviour {

    public bool isWalkable;
    public bool pararMovimentoDeQuemPassarPorCima;

    [SerializeField]
    protected short posI;
    [SerializeField]
    protected short posJ;

    [SerializeField]
    protected MapCreator.elementosPossiveisNoMapa elemento;
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

    public MapCreator.elementosPossiveisNoMapa Elemento
    {
        get
        {
            return elemento;
        }

        set
        {
            elemento = value;
        }
    }

    protected MapCreator.tipoDeElemento TipoDoElemento
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
    }

}
