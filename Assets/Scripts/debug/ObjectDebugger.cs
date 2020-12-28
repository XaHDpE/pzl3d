using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace debug
{
    public class ObjectDebugger : MonoBehaviour
    {
        private Collider _collider;
        private Camera _cam;
        
        public void SetText(string text)
        {
            var extents = transform.GetComponent<Collider>().bounds.extents;
            var maxExtent = new List<float> {extents.x, extents.y, extents.z}.Max();
            var newPos = transform.position + (transform.up * maxExtent);
            
            var textGameObj = new GameObject($"{transform.name} debug [{text}] ");
            textGameObj.transform.SetParent(transform);
            textGameObj.transform.position = newPos;
            
            var myText = textGameObj.AddComponent<TextMeshPro>();
            myText.text = text;
            myText.fontSize = 6;
            myText.autoSizeTextContainer = true;
        }
        
    }
}