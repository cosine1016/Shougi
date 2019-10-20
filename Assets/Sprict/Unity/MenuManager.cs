using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart2P()
    {
        SceneManager.LoadScene(1);
    }

    public void GameStart1P(int hard)
    {
        switch (hard)
        {
            case 0:
                SceneManager.LoadScene(2);
                break;
        }
    }

    public void Tutorial()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
