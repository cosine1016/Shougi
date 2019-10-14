using System.Collections;
using System.Collections.Generic;
using Assets.Sprict.Field;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public FieldController game;
    [SerializeField] GameObject[] cells;

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
        //foreach (GameObject item in cells)
        //{
        //    Vector3 pos = item.transform.position;
        //    pos.z = 2f;
        //    item.transform.position = pos;
        //}
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

    public void MosueOver(int id)
    {
        Piece piece = PieceController.PieceFromID(game.field, id);
        List<ActionDate> ActionList; 
        ActionList =  PieceController.PieceActionList(game.field, piece);
        foreach(ActionDate item in ActionList)
        {
            if(item.MoveOrTurn == 0)
            {
                Vector3 pos = cells[item.MoveX + item.MoveY * 3].transform.position;
                pos.z = 0.5f;
                cells[item.MoveX + item.MoveY * 3].transform.position = pos;
            }
        }
    }

}
