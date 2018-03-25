using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Step : MonoBehaviour {

    [SerializeField]
    protected enum tipoDeStep
    {
    };

    protected abstract void ExecutarStep();

}
