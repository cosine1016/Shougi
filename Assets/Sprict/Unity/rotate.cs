using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    [SerializeField] GameObject GM;
    [SerializeField] int direc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            GM.GetComponent<GameManager>().Rotate(direc);
        }
    }

}
