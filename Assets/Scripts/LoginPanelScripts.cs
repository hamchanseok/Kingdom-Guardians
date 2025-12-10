using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class LoginPanelScripts : MonoBehaviour
{
    public GameObject loginpanel;

    public void showLoginPanel()
    {
        if (loginpanel != null)
        {
            loginpanel.SetActive(true);
        }
    }


}
