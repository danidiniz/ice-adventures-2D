using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoRedo : MonoBehaviour
{

    // temporario
    public static int contador;



    public static Stack<Passo> steps;

    // Eu só crio o UndoMovimento quando o player termina de se movimentar,
    // então a lista de interactions estava sendo colocada no UndoMovimento 'anterior, ou no topo'
    // e não do UndoMovimento atual que está aguardando o player terminar o movimento pra ser criado.
    // Esse era o erro que estava atrapalhando tudo
    // Agora vou criar as interactions nessa lista temporaria, e no final do movimento eu atualizo
    // a lista do UndoMovimento que foi criado.
    public static List<UndoInteraction> interactionsTemp;

    private void Awake()
    {
        // temporario
        contador = 0;
        UndoInteraction.contadorInteract = 0;

        steps = new Stack<Passo>();
        interactionsTemp = new List<UndoInteraction>();
    }

    public static void ExecutarUndo()
    {
        // Se o player estiver parado e há elementos na listaTemporaria, significa
        // que houveram interações com o player parado
        if (!PlayerMovementAgrVai.playerEmMovimento && (interactionsTemp.Count > 0))
        {

        }


        // temporario
        if (contador > 0)
            contador--;

        // Primeiro passo nunca será executado
        // player não pode voltar de 0,0 para 0,0
        // dessa forma evito bugs
        if (steps.Count == 1)
        {
            return;
        }
        
        UndoMovimento temp = steps.Peek() as UndoMovimento;

        Debug.Log("Executando interactions do UndoMovimento (" + temp.id + "): interactions " + temp.interactions.Count);
        

        if (temp.interactions.Count > 0)
        {
            // MUITO IMPORTANTE:
            // estou tratando as interactions como ICE_INTERACTION e OJBETO_INTERACTION
            // na ordem em que salva as interações, os ObjetosDoMapa sempre salvam primeiro por questões de Orientação a Objeto
            // porém, quando executo o Undo de um ObjetoDoMapa, ele possivelmente irá se Reusar e vai para o Ice em que ele estava
            // mas como o Ice em que ele estava TAMBÉM PODE ter sofrido uma interação, quer dizer que se o Undo do ObjetoDoMapa
            // executar ANTES, o ObjetoDoMapa vai para cima do Ice que SOFREU uma interação.
            // ENTÃO, preciso executar SEMPRE o Undo dos Ices, para depois ir recolocando (ou não) os Objetos em seus lugares.
            // Esse passo é importante.
            temp.interactions.Sort(OrdenarPorTipoDeInteraction);

            // teste
            /*
            string s = "";
            for (int i = 0; i < temp.interactions.Count; i++)
            {
                s += "Interaction " + i + " instanceID: " + temp.interactions[i].HolderElementoQueSofreuInteraction.Elemento + ", ";
            }
            Debug.Log("Lista de interactions ordenada: " + s);
            */

            // Executo lista de interactions
            foreach (UndoInteraction interaction in temp.interactions)
            {
                IUndoInteraction<ElementoDoMapa, ElementoDoMapa, Passo.tiposDeInteraction> obj = interaction.HolderElementoQueSofreuInteraction as IUndoInteraction<ElementoDoMapa, ElementoDoMapa, Passo.tiposDeInteraction>;
                if (obj != null)
                {

                    //Debug.Log("Executando interaction do Elemento " + interaction.HolderElementoQueSofreuInteraction.Elemento + "[" + interaction.HolderElementoQueSofreuInteraction.PosI + "][" + interaction.HolderElementoQueSofreuInteraction.PosJ + "]");

                    //ElementoDoMapa temp2 = interaction.ElementoQueMovimentouEmCima.GetComponent<ElementoDoMapa>();
                    obj.ExecutarUndoInteraction(interaction.HolderElementoQueMovimentouEmCimaHolder);

                    /*
                    // Vejo se nessa interection tinha algum elemento em cima do ice
                    int id = interaction.ElementoAntesDoUndo.gameObject.GetInstanceID();
                    if (temp.interactionsObjetos.ContainsKey(id))
                    {
                        Debug.Log("1");
                        // Simplesmente coloco esse Objeto no Ice em que ele estava
                        // Executo o método Reuse para colocar um objeto na fila do mesmo tipo
                        // em cima do Ice
                        GameObject g = temp.interactionsObjetos[id].gameObject;
                        PoolManager.instance.ReuseObjectEmCima(g, g.transform.position, g.transform.rotation,
                            temp.interactionsObjetos[id].PosI, temp.interactionsObjetos[id].PosJ
                            );
                        Debug.Log("2");
                        // Agora copio as informações desse Objeto para o novo que veio da fila
                        temp.interactionsObjetos[id].CopiarInformacoesDesseElementoPara(
                            MapCreator.map[temp.interactionsObjetos[id].PosI, temp.interactionsObjetos[id].PosJ]
                            );
                        Debug.Log("3");
                    }
                    */
                }
            }
            // Limpo a lista
            temp.interactions.Clear();
        }

        steps.Pop();

        temp.ExecutarUndo();
    }

    static int OrdenarPorTipoDeInteraction(UndoInteraction e1, UndoInteraction e2)
    {
        return e1.tipoDaInteractionQueAconteceu.CompareTo(e2.tipoDaInteractionQueAconteceu);
    }

}
