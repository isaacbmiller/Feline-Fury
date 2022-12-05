using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonClickDetector : MonoBehaviour
{
    public Button button1;
    public Button button2;

    void OnEnable()
    {
        //Register Button Events
        button1.onClick.AddListener(() => buttonCallBack(button1));
        button2.onClick.AddListener(() => buttonCallBack(button2));

    }

    private void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == button1)
        {
            //Your code for button 1
            SceneManager.LoadScene("Level 1");
        }

        if (buttonPressed == button2)
        {
            //Your code for button 2
            SceneManager.LoadScene("Level 2");
        }


    }

    void OnDisable()
    {
        //Un-Register Button Events
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
    }
}