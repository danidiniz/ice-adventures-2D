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
        if (contador > 0)
            contador--;

        // Primeiro passo nunca será executado
        // player não pode voltar de 0,0 para 0,0
        // dessa forma evito bugs
        if (steps.Count == 1)
        {
            return;
        }
        
        //if(steps.Peek().tipoDePasso == Passo.tiposDePasso.MOVIMENTAR)
        //{
        UndoMovimento temp = steps.Pop() as UndoMovimento;

        Debug.Log("Executando interactions do UndoMovimento (" + temp.id + "): " + temp.interactions.Count);

        if (temp.interactions.Count > 0)
        {
            // Executo lista de interactions
            foreach (UndoInteraction interaction in temp.interactions)
            {
                IUndoInteraction<ElementoDoMapa, ElementoDoMapa> obj = interaction.ElementoQueInteragiu as IUndoInteraction<ElementoDoMapa, ElementoDoMapa>;
                if (obj != null)
                {
                    //ElementoDoMapa temp2 = interaction.ElementoQueMovimentouEmCima.GetComponent<ElementoDoMapa>();
                    obj.ExecutarUndoInteraction(interaction.ElementoQueMovimentouEmCima, interaction.ElementoQueInteragiu);
                }
            }
            // Limpo a lista
            temp.interactions.Clear();
        }

        temp.ExecutarUndo();

        //}
    }

}
