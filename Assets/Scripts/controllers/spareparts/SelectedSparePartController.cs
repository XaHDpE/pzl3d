using Lean.Touch;
using UnityEngine;

namespace models.sp
{
    public class SelectedSparePartController : SparePartBase
    {
        
        protected override void Awake()
        {
            base.Awake();
            PropagateSelectionToParent(true);
        }
        
        public override Transform GetSparePart()
        {
            return transform;
        }

        protected override void OnSelectHandler(LeanFinger finger)
        {
            base.OnSelectHandler(finger);
            print("i'm selected, yo!");
            InvokeSparePartSelectedEvent(transform);
        }

        protected override void OnDeSelectHandler()
        {
            base.OnDeSelectHandler();
            print("i'm de-selected, yo yo yo!");
        }
        
    }
}