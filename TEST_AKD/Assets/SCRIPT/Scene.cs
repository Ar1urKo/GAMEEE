using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class Scene : MonoBehaviour
{
    public void GoToLevel(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
