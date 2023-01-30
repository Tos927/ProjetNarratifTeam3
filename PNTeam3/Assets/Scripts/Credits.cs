using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject creditsScreen;

    public void CloseCredits()
    {
        creditsScreen.SetActive(false);
    }
}
