using Lean.Touch;
using UnityEngine;

namespace events
{
    public class CustomEventsManager : MonoBehaviour
    {
        // Delegates
        public delegate void FingerTapDelegate();
        public delegate void SparePartUnderRayCastDelegate(Transform sp, int subMeshIndex);
        public delegate void CompositeRotationRequestedDelegate();
        public delegate void SparePartMovementRequestedDelegate();
        public delegate void SparePartRotationRequestedDelegate();

    
        // Events
        public event FingerTapDelegate SparePartSelected;
        public event FingerTapDelegate NoneIsSelected;
        public event SparePartUnderRayCastDelegate SparePartUnderRayCast;
        public event CompositeRotationRequestedDelegate CompositeRotationRequested;
        public event SparePartMovementRequestedDelegate SparePartMovementRequested;
        public event SparePartRotationRequestedDelegate SparePartRotationRequested;
        
        // carousel events
        
        // camera events

        public void OnFingerTap(LeanFinger v)
        {
            print($"LeanSelectable.IsSelectedCount: {LeanSelectable.IsSelectedCount}");
            if (LeanSelectable.IsSelectedCount == 0)
            {
                print("NoneIsSelected");
                NoneIsSelected?.Invoke();
            }
            else
            {
                print("SparePartSelected");
                SparePartSelected?.Invoke();
            }
        }

        public void OnSparePartUnderRayCast(Transform sp, int subMeshIndex)
        {
            SparePartUnderRayCast?.Invoke(sp, subMeshIndex);
        }

    }
}
