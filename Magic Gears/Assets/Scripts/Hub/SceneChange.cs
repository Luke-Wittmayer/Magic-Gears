using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(int level)
    {
        if(level == 1)
        {
            SceneManager.LoadScene("Dialogue1.1");
        }
        else if (level == 2)
        {
            SceneManager.LoadScene("Dialogue2.1");
        }
        else if (level == 3)
        {
            SceneManager.LoadScene("Dialogue3.1");
        }
        else if (level == 0)
        {
            SceneManager.LoadScene("Main Menu");
        }

    }
}
