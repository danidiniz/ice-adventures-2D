using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public struct Posicao
    {
        public short i;
        public short j;
    }

    public Posicao posicaoDoStart;
    public Posicao posicaoDoEnd;

    static GameController _instance;

#region Getters and Setters
#endregion

    public static GameController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        posicaoDoStart.i = 0;
        posicaoDoStart.j = 0;
        posicaoDoEnd.i = 0;
        posicaoDoEnd.j = 0;
    }

}
