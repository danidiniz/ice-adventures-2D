using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IceBuraco : IcesDefault
{
    public short posicaoDoOutroBuracoI;
    public short posicaoDoOutroBuracoJ;

    bool temCrateEmCima;

    private SpriteRenderer spriteDoBuraco;

    void Awake()
    {
        // TEMPORARIO
        posicaoDoOutroBuracoI = (short)(Random.Range(1, MapCreator.instance.Linhas));
        posicaoDoOutroBuracoJ = (short)(Random.Range(1, MapCreator.instance.Colunas));
        if(MapCreator.map[posicaoDoOutroBuracoI, posicaoDoOutroBuracoJ].Elemento != MapCreator.elementosPossiveisNoMapa.BURACO)
        {
            MapCreator.map[posicaoDoOutroBuracoI, posicaoDoOutroBuracoJ].SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.BURACO);
        }

        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = true;

        Elemento = MapCreator.elementosPossiveisNoMapa.BURACO;

        spriteDoBuraco = GetComponent<SpriteRenderer>();
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (Elemento == oQueEstaEmCima)
            return false;

        switch (oQueEstaEmCima)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:
                //Debug.Log("Player ja chegou");
                break;
        }
        return true;
        //Debug.Log("Player está em cima do " + name);
    }

    void Teleportar(ElementoDoMapa elemento)
    {
        // Verificar se o outro lado está vazio (pode ter uma Crate em cima, por exemplo)
        if(MapCreator.map[posicaoDoOutroBuracoI, posicaoDoOutroBuracoJ].Elemento == MapCreator.elementosPossiveisNoMapa.CRATE)
        {
            Debug.Log("Não foi possível teleportar pois há uma Crate do outro lado.");
        }
        else
        {
            elemento.transform.position = MapCreator.map[posicaoDoOutroBuracoI, posicaoDoOutroBuracoJ].transform.position;
            elemento.PosI = posicaoDoOutroBuracoI;
            elemento.PosJ = posicaoDoOutroBuracoJ;

            Debug.Log("Teleportou para [" + posicaoDoOutroBuracoI + "][" + posicaoDoOutroBuracoJ + "]");
        }

    }

    public override void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (MapCreator.instance.modoCriarMapaAtivado)
        {
            MapCreatorGUIManager.buracoSelecionado = this;
            // Se eu estiver no modo criação e deletar um buraco, preciso deletar o buraco relacionado a esse
            if (MapCreator.instance.elementoSelecionado != MapCreator.elementosPossiveisNoMapa.BURACO)
            {
                DeletarBuracos();
                MapCreatorGUIManager.buracoSelecionado = null;
            }
            // Agora caso o jogador queria ver as informações do buraco
            else if(MapCreator.instance.elementoSelecionado == MapCreator.elementosPossiveisNoMapa.BURACO)
            {
                // Se o buraco tiver outro buraco relacionado
                // instancio a arrow e movimento ela ate todos os buracos
                if((posicaoDoOutroBuracoI != -1) && (posicaoDoOutroBuracoJ != -1))
                {
                    // Se a arrow já estiver ativa na cena, reseto a posição dela pro buraco clicado
                    if (GameObject.Find(MapCreatorGUIManager.arrowParaMostrarBuracos.name))
                    {

                    }
                    MapCreatorGUIManager.instance.MostrarArrowNosBuracos();
                    MapCreatorGUIManager.buracoSelecionado = null;
                }

                // Se o buraco não tiver outro buraco relacionado
                // mostro uma mensagem dizendo para o jogador clicar pra onde esse buraco irá teleportar
                else{
                    Debug.Log(gameObject.name + " não possui outro buraco relacionado.");
                }
                if (true)
                {
                    // Se o jogador clicar em algum elemento que não seja outro buraco
                    // Transformo o elemento

                    // Se ele clicar em outro buraco, apenas seto as posições
                }
            }
        }
    }

    public void DeletarBuracos()
    {
        // Transformando no elemento selecionado apenas o buraco que o jogador clicou.
        // Os outros eu transformo em Ice.
        if(MapCreatorGUIManager.buracoSelecionado == this)
        {
            SerTransformadoEm(MapCreator.instance.elementoSelecionado);
        }
        else
        {
            SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE);
        }
        
        IceBuraco temp = MapCreator.map[posicaoDoOutroBuracoI, posicaoDoOutroBuracoJ].GetComponent(typeof(IceBuraco)) as IceBuraco;
        if(temp != null)
        {
            Debug.Log(MapCreator.map[posicaoDoOutroBuracoI, posicaoDoOutroBuracoJ].gameObject.name + " deletado.");
            temp.DeletarBuracos();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ElementoDoMapa comp = other.GetComponent(typeof(ElementoDoMapa)) as ElementoDoMapa;
        if(comp != null)
        {
            Teleportar(comp);
        }
        else
        {
            Debug.Log("Não foi possível teleportar o " + other.gameObject.name + " | classe IceBuraco | método OnTriggerEnter");
        }
    }
}
