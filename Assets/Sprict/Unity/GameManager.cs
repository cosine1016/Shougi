using System.Collections;
using System.Collections.Generic;
using Assets.Sprict.Field;
using Assets.Sprict.AI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Field game;
    [SerializeField] bool vsCPU;
    [SerializeField] int CPULevel;
    [SerializeField] GameObject[] cells;
    [SerializeField] GameObject rotateR;
    [SerializeField] GameObject rotateL;
    Piece CurrentPiece;
    [SerializeField] GameObject End;
    [SerializeField] Material[] EndBaner;
    [SerializeField] AudioClip ti;
    [SerializeField] AudioClip akahara;
    public bool isChoice;
    IAgent AI;
    

    // Start is called before the first frame update
    void Start()
    {
        game = new Field();
        PieceController.init();
        game.InitilizedRandomGame();
        SpawnAll();
        switch (CPULevel)
        {
            case 1:
                AI = new Rand();
                break;
            case 2:
                // depth, numAdopt
                AI = new BruteForce(2, 20);
                break;
            default:
                AI = new Rand();
                break;
        }
        if (game.TurnSide == 2)
        {
            StartCoroutine("CPUwait");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            isChoice = false;
            foreach(GameObject item in cells)
            {
                item.GetComponent<BoadCell>().colorChangeMu();
            }
            rotateL.SetActive(false);
            rotateR.SetActive(false);
        }
    }

    public void SpawnAll()
    {
        foreach(Piece item in game.Player1)
        {
            gameObject.GetComponent<PieceOutPut>().EntitySpown(item);
        }
        foreach (Piece item in game.Player2)
        {
            gameObject.GetComponent<PieceOutPut>().EntitySpown(item);
        }
    }


    public void PieceChoice(int id)
    {
        foreach (GameObject item in cells)
        {
            item.GetComponent<BoadCell>().colorChangeMu();
        }
        rotateL.SetActive(false);
        rotateR.SetActive(false);
        if (PieceController.PieceFromID(game, id).Side == game.TurnSide)
        {
            if (vsCPU && PieceController.PieceFromID(game, id).Side == 2) return;
            CurrentPiece = PieceController.PieceFromID(game, id);
            isChoice = true;
            List<ActionDate> ActionList;
            ActionList = PieceController.PieceActionList(game, CurrentPiece);
            foreach (ActionDate item in ActionList)
            {
                if (item.MoveOrTurn == 0)
                {
                    cells[item.MoveX + item.MoveY * 3].GetComponent<BoadCell>().colorChangeYerrow();
                }
            }

            rotateL.SetActive(true);
            rotateR.SetActive(true);
            Vector3 posPiece;
            posPiece.x = (CurrentPiece.PosX - 1) * 1.75f;
            posPiece.y = -CurrentPiece.PosY * 1.75f + 4.5f;
            posPiece.z = -1;
            if (CurrentPiece.Side == 1)
            {
                rotateL.transform.position = new Vector3((float)(posPiece.x + 0.75), (float)(posPiece.y + 0.75), posPiece.z);
                rotateR.transform.position = new Vector3((float)(posPiece.x - 0.75), (float)(posPiece.y + 0.75), posPiece.z);
                rotateL.transform.eulerAngles = new Vector3(0, 0, 180f);
                rotateR.transform.eulerAngles = new Vector3(0, 0, 180f);
            }
            else
            {
                rotateL.transform.position = new Vector3((float)(posPiece.x - 0.75), (float)(posPiece.y - 0.75), posPiece.z);
                rotateR.transform.position = new Vector3((float)(posPiece.x + 0.75), (float)(posPiece.y - 0.75), posPiece.z);
                rotateL.transform.eulerAngles = new Vector3(0, 0, 0);
                rotateR.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (isChoice == true)
            {
                Piece enemy = PieceController.PieceFromID(game, id);
                if (CanMove(enemy.PosX, enemy.PosY))
                {
                    game = PieceController.PieceDeath(game, enemy.ID);
                    Move(enemy.PosX, enemy.PosY);
                }
            }
        }
    }

    public bool CanMove(int x, int y)
    {
        return PieceController.PieceCanMoveJudge(game, CurrentPiece.ID, CurrentPiece.Side, x, y);
    }

    public void Move(int x, int y)
    {
        game  = PieceController.PieceSet(game, CurrentPiece.ID, x, y);
        game.ChangeSide();
        isChoice = false;
        foreach (GameObject item in cells)
        {
            item.GetComponent<BoadCell>().colorChangeMu();
        }
        rotateL.SetActive(false);
        rotateR.SetActive(false);
        if (game.JudgeWinner() != 0)
        {
            GameEnd(game.JudgeWinner());
        }
        GetComponent<AudioSource>().PlayOneShot(ti);
        if (vsCPU && game.TurnSide == 2 && !game.isEnd)
        {
            StartCoroutine("CPUwait");
        }
    }

    public void Rotate(int direc)
    {
        game  = PieceController.PieceRotate(game , CurrentPiece.ID, direc);
        game.ChangeSide();
        isChoice = false;
        foreach (GameObject item in cells)
        {
            item.GetComponent<BoadCell>().colorChangeMu();
        }
        rotateL.SetActive(false);
        rotateR.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(ti);
        if (vsCPU && game.TurnSide == 2)
        {
            StartCoroutine("CPUwait");
        }
    }

    IEnumerator CPUwait()
    {
        yield return new WaitForSeconds(1);
        CPUACtion();
    }


    void CPUACtion()
    {
        ActionDate action = AI.Search(game);
        CurrentPiece = PieceController.PieceFromID(game, action.ID);
        if(action.MoveOrTurn == 0)
        {
            int enemyid = game .IDs[action.MoveX, action.MoveY];
            if (enemyid > 0)
            {
                game  = PieceController.PieceDeath(game , enemyid);
            }
            Move(action.MoveX, action.MoveY);
        }
        else
        {
            Rotate(action.Turn);
        }
    }


    void GameEnd(int winner)
    {
        End.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(akahara);
        End.GetComponent<Renderer>().material = EndBaner[winner - 1];
    }
    
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }


}
