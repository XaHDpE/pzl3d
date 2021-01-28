using Lean.Touch;
using models.state;
using states.controllers;
using UnityEngine;

namespace states.sparepart
{
    public class CarouselItemIdleState : ObjectBaseState
    {
        public override void EnterState(InputManagerController inputManager)
        {
            // Debug.Log($"CarouselItemIdleState: {inputManager.sparePart} entered Idle");
        }

        public override void ExitState(InputManagerController inputManager) {}

        public override void LogicUpdate(InputManagerController inputManager) {}

        public override void PhysicsUpdate(InputManagerController inputManager) { }
        
    }
}