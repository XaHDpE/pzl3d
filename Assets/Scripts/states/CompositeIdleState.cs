using models.state;
using states.controllers;
using UnityEngine;

namespace states
{
    public class CompositeIdleState : ObjectBaseState
    {
        public override void EnterState(InputManagerController inputManager)
        {
            Debug.Log("i'm in CompositeIdleState.");
        }

        public override void ExitState(InputManagerController inputManager)
        {
            Debug.Log("out of CompositeIdleState.");
        }
        
    }
    
}