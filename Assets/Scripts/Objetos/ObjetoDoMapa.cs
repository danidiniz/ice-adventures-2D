using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoDoMapa : ElementoDoMapa {

    public bool isComum;
    [SerializeField]
    protected bool isQuebravel;
    [SerializeField]
    protected bool isPulavel;
    [SerializeField]
    protected bool isEmpurravel;
    [SerializeField]
    protected short quantidadeDeVezesQuePodeSerEmpurrada;

    private void Awake()
    {
        TipoDoElemento = MapCreator.tipoDeElemento.OBJETO;
    }

    #region Getters and Setters
    public bool IsCrateQuebravel
    {
        get
        {
            return isQuebravel;
        }

        set
        {
            isQuebravel = value;
        }
    }

    public bool IsCratePulavel
    {
        get
        {
            return isPulavel;
        }

        set
        {
            isPulavel = value;
        }
    }

    public bool IsCrateEmpurravel
    {
        get
        {
            return isEmpurravel;
        }

        set
        {
            isEmpurravel = value;
        }
    }

    public short QuantidadeDeVezesQueACratePodeSerEmpurrada
    {
        get
        {
            return quantidadeDeVezesQuePodeSerEmpurrada;
        }

        set
        {
            quantidadeDeVezesQuePodeSerEmpurrada = value;
        }
    }
    #endregion

    public override void CopiarInformacoesDesseElementoPara(ElementoDoMapa target)
    {

    }

}
