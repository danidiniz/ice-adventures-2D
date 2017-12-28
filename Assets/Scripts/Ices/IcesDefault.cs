using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class IcesDefault : ElementoDoMapa, IColliderIce<MapCreator.elementosPossiveisNoMapa, ElementoDoMapa> {

    // Todo ice tem um tipo
    // Todo ice é clicável
    //Todo ice tem uma posição i,j
    // Todo ice tem um sprite?
    // Todo ice tem um método "collider" pra quando o player estiver em cima

    /*
protected enum tiposDeIce { ICE, CRATE, BURACO, START, END };
[SerializeField]
private tiposDeIce tipo;
*/


    // Variavel para saber o que o ice era antes de ser transformado
    // Por exemplo se ele é um IceQuebradoComCrate e alguem quebra essa Crate,
    // como eu saberia o que ela era antes de ser um IceQuebradoComCrate?
    // Dessa forma facilita
    public MapCreator.elementosPossiveisNoMapa tipoDeIceAntesDeSerTransformado;

    public bool isWalkable;
    public bool pararMovimentoDeQuemPassarPorCima;

    public enum coisasQuePodemEstarEmCimaDoIce
    {
        PLAYER,
        PINGUIM,
        CRATE, CRATE_COM_PINGUIM_1, CRATE_COM_PINGUIM_2, CRATE_COM_PINGUIM_3, CRATE_COM_PINGUIM_4,
        ESPECIAL_PEIXE, ESPECIAL_ASAS,
        START, END,
        ONDA_DA_ORCA, URSO_POLAR,
    };

    //[SerializeField]
    //private MapCreator.elementosPossiveisNoMapa tipo;

    //[SerializeField]
    //protected short posI;
    //[SerializeField]
    //protected short posJ;

    /*
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
*/

    public void InitIce(short _i, short _j)
    {
        posI = _i;
        posJ = _j;
    }
    


    public virtual bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        //Debug.Log(elementoEmCimaDoIce.name + "["+PosI+"]["+PosJ+"]");
        if (Tipo == oQueEstaEmCima)
            return false;
        return true;
    }
    
    public override void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (MapCreator.instance.modoCriarMapaAtivado)
        {
            // Verificando se o elemento selecionado é o Start ou End
            // se for, só posso ter 1 deles no mapa,
            // então verifico se já existe algum no mapa e deleto se houve
            // depois crio o novo Start ou End no ice que a pessoa clicou
            switch (MapCreator.instance.elementoSelecionado)
            {
                case MapCreator.elementosPossiveisNoMapa.START:
                    if(MapCreator.map[GameController.instance.posicaoDoStart.i, GameController.instance.posicaoDoStart.j].Tipo == MapCreator.elementosPossiveisNoMapa.START)
                    {
                        MapCreator.map[GameController.instance.posicaoDoStart.i, GameController.instance.posicaoDoStart.j].SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE);
                    }
                    GameController.instance.posicaoDoStart.i = PosI;
                    GameController.instance.posicaoDoStart.j = PosJ;
                    break;
                case MapCreator.elementosPossiveisNoMapa.END:
                    if (MapCreator.map[GameController.instance.posicaoDoEnd.i, GameController.instance.posicaoDoEnd.j].Tipo == MapCreator.elementosPossiveisNoMapa.END)
                    {
                        MapCreator.map[GameController.instance.posicaoDoEnd.i, GameController.instance.posicaoDoEnd.j].SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE);
                    }
                    GameController.instance.posicaoDoEnd.i = PosI;
                    GameController.instance.posicaoDoEnd.j = PosJ;
                    break;
            }

            SerTransformadoEm(MapCreator.instance.elementoSelecionado);
        }
        else
        {
//            Debug.Log("Cliquei no " + name);
        }
    }

    protected string GetName()
    {
        switch (Tipo)
        {
            case MapCreator.elementosPossiveisNoMapa.ICE:
                return "Ice";
            case MapCreator.elementosPossiveisNoMapa.CRATE:
                return "Crate";
            case MapCreator.elementosPossiveisNoMapa.BURACO:
                return "Buraco";
            case MapCreator.elementosPossiveisNoMapa.START:
                return "Start";
            case MapCreator.elementosPossiveisNoMapa.END:
                return "End";
            default:
                return "Elemento não existe [" + posI + "][" + posJ + "]";
        }
    }

    public virtual void SerTransformadoEm(MapCreator.elementosPossiveisNoMapa elemento)
    {
        GameObject prefabDoElemento = MapCreator.instance.RetornarElemento(elemento);
        IcesDefault novoElementoComponente = prefabDoElemento.GetComponent(typeof(IcesDefault)) as IcesDefault;
        // Transforma esse elemento apenas se forem de tipos diferentes
        if (Tipo != novoElementoComponente.Tipo)
        {
            // Instancio o elemento
            GameObject novoElemento = Instantiate(prefabDoElemento, transform.position, Quaternion.identity) as GameObject;

            // Pegando componente desse novoElemento para incializá-lo
            novoElementoComponente = novoElemento.GetComponent(typeof(IcesDefault)) as IcesDefault;

            // Inicio a posição do novo elemento com a posição desse elemento que está sendo transformado
            novoElementoComponente.InitIce(posI, posJ);

            // Setando parent e posição na hierarquia
            novoElemento.transform.parent = MapCreator.instance.mapIcesParent;
            novoElemento.transform.SetSiblingIndex(posI * MapCreator.instance.Colunas + posJ);
            // Setando nome
            novoElemento.name = novoElementoComponente.GetName() + "[" + posI + "][" + posJ + "]";

            // Atualizando o map[]
            MapCreator.map[posI, posJ] = novoElementoComponente;

            MapCreator.map[posI, posJ].tipoDeIceAntesDeSerTransformado = Tipo;

//            Debug.Log("Destruí o " + name + " e criei um " + novoElemento.name);

            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Não foi possível transformar o elemento " + GetName() + " em " + novoElementoComponente.GetName());
        }
    }
}
