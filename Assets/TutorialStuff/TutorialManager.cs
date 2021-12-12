using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps; //for putting popups that the player sees
    private int popUpIndex;
    private bool isDone = false;
    [SerializeField] VariableJoystick joystick;


    void Update()
    {

        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUps[popUpIndex].SetActive(true);
            }
            else
            {
                popUps[popUpIndex].SetActive(false);
            }
        }
        //---------------------------------------------------------
        if (popUpIndex == 0)
        {
            if (joystick.Vertical != 0 && !isDone)
            {
                isDone = true;
                Debug.Log("test");
                popUpIndex++;
            }

        }
        else if (popUpIndex == 1)
        {

        }


        Debug.Log(popUps.Length);
    }
}
              
