using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementoDoMapa : MonoBehaviour {

    [SerializeField]
    protected short posI;
    [SerializeField]
    protected short posJ;

    [SerializeField]
    protected MapCreator.elementosPossiveisNoMapa tipo;

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

    public MapCreator.elementosPossiveisNoMapa Tipo
    {
        get
        {
            return tipo;
        }

        set
        {
            tipo = value;
        }
    }
    #endregion

    public virtual void OnMouseDown()
    {
    }

}
