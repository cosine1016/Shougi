using System.Collections;
using System.Collections.Generic;
using Assets.Sprict.Field;
using UnityEngine;

public class PieceOutPut : MonoBehaviour
{
    [SerializeField] GameObject[] PiecePrehabs;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EntitySpown(Piece piece)
    {
        GameObject entity = Instantiate(PiecePrehabs[piece.Kind]);
        entity.GetComponent<EntityPiece>().ID = piece.ID;
        entity.GetComponent<EntityPiece>().side = piece.Side;
        entity.GetComponent<EntityPiece>().kind = piece.Kind;
    }

}
