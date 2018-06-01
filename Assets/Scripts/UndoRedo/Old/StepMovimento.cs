using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StepMovimento : Step {

    public short posInicioI;
    public short posInicioJ;

    public StepMovimento(short i, short j)
    {
        posInicioI = i;
        posInicioJ = j;
    }

  

    public override void ExecutarStep()
    {
        StepMovimento temp = (StepMovimento)steps.Pop();
        short i = temp.posInicioI;
        short j = temp.posInicioJ;

        PlayerInfo.instance.setPosition(i, j);
        MonoBehaviour.FindObjectOfType<PlayerInfo>().gameObject.transform.position = MapCreator.map[i, j].gameObject.transform.position;

        // Se houve interações nesse Step, desfaço todas
        if (Interaction.interactions.ContainsKey(temp.stepKey))
        {
            for (int k = Interaction.interactions[temp.stepKey].Count; k > 0; k--)
            {
                Interaction tempRection = Interaction.interactions[temp.stepKey].Pop();
                switch (tempRection.tipoDaInteracao)
                {
                    case Interaction.tiposDeInteracao.QUEBRAR:
                        tempRection.elemento.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }
    
}
