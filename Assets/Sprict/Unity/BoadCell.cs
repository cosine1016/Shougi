using System.Collections;
using System.Collections.Generic;
using Assets.Sprict.Field;
using UnityEngine;

public class BoadCell : MonoBehaviour
{
    [SerializeField] GameObject GM;
    [SerializeField] Material mu;
    [SerializeField] Material yerrow;
    [SerializeField] Vector2Int pos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void colorChangeYerrow()
    {
        GetComponent<Renderer>().material = yerrow;  
    }

    public void colorChangeMu()
    {
        GetComponent<Renderer>().material = mu;
    }

    public void OnMouseOver()
    {
        if(GM.GetComponent<GameManager>().isChoice && Input.GetMouseButton(0) && GM.GetComponent<GameManager>().CanMove(pos.x, pos.y))
        {
            int enemyid = GM.GetComponent<GameManager>().game.IDs[pos.x, pos.y];
            if (enemyid > 0)
            {
                GM.GetComponent<GameManager>().game = PieceController.PieceDeath(GM.GetComponent<GameManager>().game, enemyid);
            }
            GM.GetComponent<GameManager>().Move(pos.x, pos.y);
        }
    }
}
