using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBuner : MonoBehaviour
{
    [SerializeField] GameObject GM;
    [SerializeField] Material[] turnMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material = turnMat[GM.GetComponent<GameManager>().game.TurnSide - 1];
    }
}
