using models.state;
using states.controllers;

namespace states
{
    public class CompositeRotateState : ObjectBaseState
    {
        public override void EnterState(InputManagerController inputManager)
        {
            // Object.FindObjectOfType<CameraScriptsManager>().EnableCameraMovement();
            
        }

        public override void ExitState(InputManagerController inputManager)
        {
            // Object.FindObjectOfType<CameraScriptsManager>().DisableCameraMovement();
        }
        
    }
    
}