using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTheme : MonoBehaviour
{
    [SerializeField] AudioSource menuTheme;

    void Start()
    {
        menuTheme.Play(); 
    }
}
