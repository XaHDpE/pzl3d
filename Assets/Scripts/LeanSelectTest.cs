using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
[RequireComponent(typeof(LeanSelect))]
public class LeanSelectTest : MonoBehaviour
{
    private LeanSelect _ls;
    // Start is called before the first frame update
    private void Awake()
    {
        _ls = GetComponent<LeanSelect>();
    }

    private void Start()
    {
        print($"object: {transform}, AutoDeselect: {_ls.AutoDeselect}");
    }
    
}
