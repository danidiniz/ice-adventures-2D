using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour {

    public float movementSpeed;

    public enum objectPossiveisDirections { SEM_MOVIMENTO, INDO_PARA_CIMA, INDO_PARA_DIREITA, INDO_PARA_BAIXO, INDO_PARA_ESQUERDA };
    [SerializeField]
    private objectPossiveisDirections objectCurrentDirection;
    
    public enum objetosQuePodemSeMovimentar
    {
        PLAYER,

    };

    // Variável para outras classes decidirem se pode movimentar
    public bool pararMovimentoDoPlayerNoIce;
    
    public bool podeAnimarMovimento;
    // Direção que o player está se movimentando (para inverter direção apenas multiplico por -1)
    public short direcaoDoMovimentoI;
    public short direcaoDoMovimentoJ;

    public bool jaPassouNoIce;
    // Lista para salvar os Ices que o método de movimento passou
    // Dessa forma eu posso movimentar o player de ice em ice
    // Isso foi necessário pois da outra forma eu executaria o Teleport do buraco, por exemplo, antes do player chegar na posição do buraco
    public List<IcesDefault> icesQueOPlayerVaiSeMovimentar; 

    protected SerVivo serVivoInfoComponente;

    #region Getters and Setters
    public objectPossiveisDirections ObjectCurrentDirection
    {
        get
        {
            return objectCurrentDirection;
        }

        set
        {
            objectCurrentDirection = value;
        }
    }
    #endregion

    protected virtual void Start()
    {
        ObjectCurrentDirection = objectPossiveisDirections.SEM_MOVIMENTO;

        transform.position = MapCreator.map[0, 0].gameObject.transform.position;

        
        pararMovimentoDoPlayerNoIce = false;
        podeAnimarMovimento = !pararMovimentoDoPlayerNoIce;

        serVivoInfoComponente = GetComponent(typeof(SerVivo)) as SerVivo;
    }


    protected void MovimentandoPlayerAnimacao()
    {
        transform.position = Vector3.MoveTowards(transform.position, MapCreator.map[serVivoInfoComponente.PosI, serVivoInfoComponente.PosJ].gameObject.transform.position, movementSpeed * Time.deltaTime);
    }

    
    
    

        /*
    protected bool MovimentarPlayerNoMapa2(short direcaoDoMovimentoI, short direcaoDoMovimentoJ)
    {
        if (MapCreator.instance.VerificarSeEstaDentroDoMapa((short)(serVivoInfoComponente.PosI + direcaoDoMovimentoI), (short)(serVivoInfoComponente.PosJ + direcaoDoMovimentoJ)))
        {

            if(MapCreator.map[serVivoInfoComponente.PosI + direcaoDoMovimentoI, serVivoInfoComponente.PosJ + direcaoDoMovimentoJ].isWalkable)
            {
                serVivoInfoComponente.PosI += direcaoDoMovimentoI;
                serVivoInfoComponente.PosJ += direcaoDoMovimentoJ;
                

            }








            pararMovimentoDoPlayerNoIce = true;
            podeMovimentarPegarInputDoPlayer = false;
            return false;
        }
        if (podeMovimentarPegarInputDoPlayer)
        {
            if (MapCreator.map[serVivoInfoComponente.PosI + direcaoDoMovimentoI, serVivoInfoComponente.PosJ + direcaoDoMovimentoJ].isWalkable)
            {
                serVivoInfoComponente.PosI += direcaoDoMovimentoI;
                serVivoInfoComponente.PosJ += direcaoDoMovimentoJ;
                MapCreator.map[serVivoInfoComponente.PosI, serVivoInfoComponente.PosJ].AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa.PLAYER, serVivoInfoComponente);

                pararMovimentoDoPlayerNoIce = MapCreator.map[serVivoInfoComponente.PosI, serVivoInfoComponente.PosJ].pararMovimentoDeQuemPassarPorCima;

                MovimentarPlayerNoMapa(direcaoDoMovimentoI, direcaoDoMovimentoJ);
            }
        }
        else
        {
            return false;
        }
        return true;
    }
    */
}
