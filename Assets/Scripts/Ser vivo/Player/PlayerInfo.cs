using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : SerVivo {

    // Criar a classe especiais
    // interface?

    public enum tiposDeEspecial
    {
        NENHUM,
        QUEBRA_TUDO
    };

    public tiposDeEspecial especialAtivo;

    static PlayerInfo _instance;

    public PlayerInfo()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = true;
        TipoDoElemento = MapCreator.tipoDeElemento.SER_VIVO;
        ElementoNoMapa = MapCreator.elementosPossiveisNoMapa.PLAYER;
    }

    public static PlayerInfo instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerInfo>();
            }
            return _instance;
        }
    }
    
    void Awake ()
    {
        PosI = 0;
        PosJ = 0;

        especialAtivo = tiposDeEspecial.NENHUM;
	}
}
