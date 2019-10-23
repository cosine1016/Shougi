using System.Collections;
using System.Collections.Generic;
using Assets.Sprict.Field;
using UnityEngine;

public class EntityPiece : MonoBehaviour
{
    [SerializeField]GameObject GM;
    public int kind;
    public int ID;
    public int side;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        UpDataPos();
    }

    public void UpDataPos()
    { 
        Field field = GM.GetComponent<GameManager>().game;
        Piece piece = PieceController.PieceFromID(field, ID);
        if (piece.isDeath == true)
        {
            gameObject.SetActive(false);
        }
        Vector3 pos = new Vector3();
        pos.x = (piece.PosX - 1) * 1.75f;
        pos.y = - piece.PosY * 1.75f + 4.5f ;
        pos.z = 0;
        gameObject.transform.position = pos;
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, piece.Angle * 90f);
        if(piece.Side == 1)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            GM.GetComponent<GameManager>().PieceChoice(ID);
        }
    }


}
