using System;
using Lean.Touch;
using UnityEngine;

namespace input
{
    [RequireComponent(typeof(LeanMultiUpdate))]
    [RequireComponent(typeof(LeanManualRotate))]
    [RequireComponent(typeof(LeanSelectable))]
    public class Rotatable : MonoBehaviour
    {
        private LeanManualRotate _lmr;
        private LeanMultiUpdate _lmu;
        private LeanSelectable _ls;
        private Camera _cam;

        private void Awake()
        {
            _ls = GetComponent<LeanSelectable>();
            _lmr = GetComponent<LeanManualRotate>();
            _lmu = GetComponent<LeanMultiUpdate>();
            _cam = Camera.main;
            ConfigureLeanScripts();
        }
        protected void OnEnable()
        {
            _lmu.OnDelta.AddListener(_lmr.RotateAB);
            
        }
        protected void OnDisable()
        {
            _lmu.OnDelta.RemoveAllListeners();
        }
        private void ConfigureLeanScripts()
        {

            _lmu.Use = new LeanFingerFilter(
                LeanFingerFilter.FilterType.AllFingers, 
                true, 
                0, 0, 
                _ls
            );
            
            _lmu.ScreenDepth = new LeanScreenDepth() { Camera = _cam };
        }
    }
}