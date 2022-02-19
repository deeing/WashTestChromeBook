using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlaySound("Main Menu Music");
    }
}
