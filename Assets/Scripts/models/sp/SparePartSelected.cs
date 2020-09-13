using Lean.Touch;
using UnityEngine;

namespace models.sp
{
    public class SparePartSelected : SparePartBase
    {
        public override Transform GetSparePart()
        {
            return transform;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            PropagateSelectionToParent(false);
        }

        protected override void OnSelectHandler(LeanFinger finger)
        {
            base.OnSelectHandler(finger);
            
        }
    }
}