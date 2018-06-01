using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoRedo : MonoBehaviour {
    
    public static Stack<Passo> steps;

    private void Awake()
    {
        steps = new Stack<Passo>();
    }

    public static void ExecutarUndo()
    {
        if(steps.Peek().tipoDePasso == Passo.tiposDePasso.MOVIMENTAR)
        {
            UndoMovimento temp = steps.Pop() as UndoMovimento;
            temp.ExecutarUndo();

            //Debug.Log("Quantidade de interactions nesse Step: " + temp.interactions.Count);

            if (temp.interactions != null)
            {
                // Executo lista de interactions
                foreach (UndoInteraction interaction in temp.interactions)
                {
                    IUndoInteraction<ElementoDoMapa> obj = interaction.ElementoQueInteragiu as IUndoInteraction<ElementoDoMapa>;
                    if (obj != null)
                    {
                        obj.ExecutarUndoInteraction(interaction.ElementoQueMovimentouEmCima);
                    }
                }
                // Limpo a lista
                temp.interactions.Clear();
            }
        }
    }
    
}
