using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour {

    public enum objectPossiveisDirections { SEM_MOVIMENTO, INDO_PARA_CIMA, INDO_PARA_DIREITA, INDO_PARA_BAIXO, INDO_PARA_ESQUERDA };
    [SerializeField]
    private objectPossiveisDirections objectCurrentDirection;

    private SerVivo serVivoInfoComponente;

    // Lista contendo os ices que o player 
    // vai se movimentar na jogada
    List<IcesDefault> icesMovement;

    private Vector2 startPos;
    private bool directionChosen;
    private Vector2 direction;

    private int direcaoDoMovimentoI;
    private int direcaoDoMovimentoJ;
    private bool pegouInputDoPlayer;

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

    private void Start()
    {
        icesMovement = new List<IcesDefault>();

        ObjectCurrentDirection = objectPossiveisDirections.SEM_MOVIMENTO;

        transform.position = MapCreator.map[0, 0].gameObject.transform.position;

        serVivoInfoComponente = GetComponent(typeof(SerVivo)) as SerVivo;

        direcaoDoMovimentoI = 0;
        direcaoDoMovimentoJ = 0;

        
    }
    
    /*
    private void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            MovimentarPlayerNoMapa();
            if (podePegarInputDoPlayer)
            {
                ZerarDirecaoMovimentos();
                PegarInputDoPlayer();
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            MovimentarPlayerNoMapa();

            if (!podePegarInputDoPlayer)
                return;

            // Track a single touch as a direction control.
            if ((Input.touchCount > 0))
            {
                Touch touch = Input.GetTouch(0);

                // Handle finger movements based on touch phase.
                switch (touch.phase)
                {
                    // Record initial touch position.
                    case TouchPhase.Began:
                        startPos = touch.position;
                        directionChosen = false;
                        break;

                    // Determine direction by comparing the current touch position with the initial one.
                    case TouchPhase.Moved:
                        direction = touch.position - startPos;
                        break;

                    // Report that a direction has been chosen when the finger is lifted.
                    case TouchPhase.Ended:
                        directionChosen = true;
                        break;
                }
            }
            if (directionChosen)
            {
                // Something that uses the chosen direction...
                directionChosen = false;
                ZerarDirecaoMovimentos();
                PegarInputSwipe();
                direction = Vector3.zero;
            }
        }
    }
    */
    

    private void PegarInputDoPlayer()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_CIMA;
            direcaoDoMovimentoI = -1;
            direcaoDoMovimentoJ = 0;
            pegouInputDoPlayer = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_DIREITA;
            direcaoDoMovimentoI = 0;
            direcaoDoMovimentoJ = 1;
            pegouInputDoPlayer = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_BAIXO;
            direcaoDoMovimentoI = 1;
            direcaoDoMovimentoJ = 0;
            pegouInputDoPlayer = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_ESQUERDA;
            direcaoDoMovimentoI = 0;
            direcaoDoMovimentoJ = -1;
            pegouInputDoPlayer = true;
        }
    }

    private void PegarInputSwipe()
    {
        float x = direction.x;
        float y = direction.y;

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (y > 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_CIMA;
                direcaoDoMovimentoI = -1;
                direcaoDoMovimentoJ = 0;
                pegouInputDoPlayer = true;
            }
            else if (y < 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_BAIXO;
                direcaoDoMovimentoI = 1;
                direcaoDoMovimentoJ = 0;
                pegouInputDoPlayer = true;
            }
        }
        else if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (x > 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_DIREITA;
                direcaoDoMovimentoI = 0;
                direcaoDoMovimentoJ = 1;
                pegouInputDoPlayer = true;
            }
            else if (x < 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_ESQUERDA;
                direcaoDoMovimentoI = 0;
                direcaoDoMovimentoJ = -1;
                pegouInputDoPlayer = true;
            }
        }
    }

    public void ZerarDirecaoMovimentos()
    {
        direcaoDoMovimentoI = 0;
        direcaoDoMovimentoJ = 0;
    }
    
}
