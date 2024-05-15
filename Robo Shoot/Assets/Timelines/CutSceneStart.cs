using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneStart : MonoBehaviour
{

    public GameObject playerReal;
    public GameObject cutsceneCamera;
    public GameObject playerNoReal;

    private void Start()
    {
        Invoke("CutSceneFinish", 20f);
    }

    public void CutSceneFinish()
    {
        playerNoReal.SetActive(false);
        playerReal.SetActive(true);
        cutsceneCamera.SetActive(false);
    }



}
