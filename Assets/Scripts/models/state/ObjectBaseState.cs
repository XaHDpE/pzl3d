using System;
using states.controllers;

namespace models.state
{
    public abstract class ObjectBaseState
    {
        public abstract void EnterState(InputManagerController inputManager);

        public abstract void ExitState(InputManagerController inputManager);

        public abstract void LogicUpdate(InputManagerController inputManager);
        
        public abstract void PhysicsUpdate(InputManagerController inputManager);

        // public abstract void OnCollisionEnter(InputManagerController inputManager);

    }
    
}