using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

namespace events
{
    public class CustomEventsManager : MonoBehaviour
    {
        // Delegates
        public delegate void FingerTapDelegate();
        public delegate void SparePartUnderRayCastDelegate(Transform sp, int subMeshIndex);
    
        // Events
        public event FingerTapDelegate SparePartSelected;
        public event FingerTapDelegate NoneIsSelected;
        public event SparePartUnderRayCastDelegate SparePartUnderRayCast;
        
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
