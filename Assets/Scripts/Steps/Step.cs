using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step: MonoBehaviour {

    // contando numero de steps
    public static int keyCount;

    // se houver interaction, crio a chave pra esse step
    public int stepKey;

    // Por enquanto, novo Step é criado no Script 
    //de movimento do player
    public static Stack<Step> steps;

    [SerializeField]
    protected enum tipoDeStep
    {
        MOVIMENTO
    };

    private void Awake()
    {
        keyCount = 0;
        steps = new Stack<Step>();
    }

    public void SetarKey()
    {
        stepKey = keyCount;
    }

    public virtual void ExecutarStep() { }

}
