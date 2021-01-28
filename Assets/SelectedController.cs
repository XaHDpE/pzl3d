using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedController : MonoBehaviour
{
    private Button _button;
    public TopBarController topbar;
    
    private void Awake()
    {
        _button = GameObject.Find("CleanButton").GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Arrange);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void Arrange()
    {
        Debug.Log("Arrange started");
    }
    
}
