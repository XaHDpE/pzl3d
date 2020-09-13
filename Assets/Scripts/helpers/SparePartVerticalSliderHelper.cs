using System;
using camera;
using Lean.Touch;
using models.sp;
using UnityEngine;

namespace helpers
{
    public class SparePartVerticalSliderHelper : MonoBehaviour
    {

        private LeanSwap _ls;
        public CameraFollowNew cam;

        private void OnEnable()
        {
            SparePartBase.SelectedNotification += DisableSlider;
            SparePartBase.DeselectedNotification += EnableSlider;
        }
        
        private void OnDisable()
        {
            SparePartBase.DeselectedNotification -= EnableSlider;
            SparePartBase.SelectedNotification -= DisableSlider;
        }

        private void Start()
        {
            _ls = GetComponent<LeanSwap>();
        }

        public void SetTarget()
        {
            cam.SetTarget(_ls.Prefabs[_ls.Index]);
        }

        private void DisableSlider(Transform sp)
        {
            foreach (var childLfs in GetComponentsInChildren<LeanFingerSwipe>())
            {
                print($"disabling slider in {childLfs.name}");
                childLfs.gameObject.SetActive(false);
            }
        }
        
        private void EnableSlider()
        {
            print("EnableSlider");
            foreach (var childLfs in GetComponentsInChildren<LeanFingerSwipe>())
            {
                print($"enabling slider in {childLfs.name}");
                childLfs.gameObject.SetActive(true);
            }
        }
        
    }
}