using System.Collections;
using System.Collections.Generic;
using Assets.Sprict.Field;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public FieldController game;
    [SerializeField] GameObject[] cells;
    [SerializeField] GameObject rotateR;
    [SerializeField] GameObject rotateL;
    Piece CurrentPiece;
    public bool isChoice; 
    

    // Start is called before the first frame update
    void Start()
    {
        game = new FieldController();
        PieceController.init();
        game.InitilizedRandomGame();
        SpawnAll();
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
        foreach(Piece item in game.field.Player1)
        {
            gameObject.GetComponent<PieceOutPut>().EntitySpown(item);
        }
        foreach (Piece item in game.field.Player2)
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
        if (PieceController.PieceFromID(game.field, id).Side == game.TurnSide)
        {
            CurrentPiece = PieceController.PieceFromID(game.field, id);
            isChoice = true;
            List<ActionDate> ActionList;
            ActionList = PieceController.PieceActionList(game.field, CurrentPiece);
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
                Piece enemy = PieceController.PieceFromID(game.field, id);
                if (CanMove(enemy.PosX, enemy.PosY))
                {
                    Move(enemy.PosX, enemy.PosY);
                    game.field = PieceController.PieceDeath(game.field, enemy.ID);
                    if(enemy.ID == 0)
                    {
                        GameEnd(2);
                    }
                    if(enemy.ID == 1)
                    {
                        GameEnd(1);
                    }
                    
                    int lest = 0;
                    foreach(Piece item in game.field.Player1)
                    {
                        if (!item.isDeath) lest++;
                    }
                    if (lest == 2) GameEnd(0);
                    
                }
            }
        }
    }

    public bool CanMove(int x, int y)
    {
        return PieceController.PieceCanMoveJudge(game.field, CurrentPiece.ID, CurrentPiece.Side, x, y);
    }

    public void Move(int x, int y)
    {
        game.field = PieceController.PieceSet(game.field, CurrentPiece.ID, x, y);
        game.ChangeSide();
        isChoice = false;
        foreach (GameObject item in cells)
        {
            item.GetComponent<BoadCell>().colorChangeMu();
        }
        rotateL.SetActive(false);
        rotateR.SetActive(false);
    }

    public void Rotate(int direc)
    {
        int side = CurrentPiece.Side * 2 - 3;
        game.field = PieceController.PieceRotate(game.field, CurrentPiece.ID, side * direc);
        game.ChangeSide();
        isChoice = false;
        foreach (GameObject item in cells)
        {
            item.GetComponent<BoadCell>().colorChangeMu();
        }
        rotateL.SetActive(false);
        rotateR.SetActive(false);
    }

    void GameEnd(int winner)
    {

    }

}
