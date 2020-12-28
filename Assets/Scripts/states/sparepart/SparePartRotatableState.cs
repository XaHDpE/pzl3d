using input;
using models.state;
using states.controllers;

namespace states.sparepart
{
    public class SparePartRotatableState : ObjectBaseState
    {
        private Rotatable _rotationComponent;
        
        public override void EnterState(InputManagerController inputManager)
        {
            // inputManager.sparePart.GetComponent<ISwitchable>().On();
            _rotationComponent = inputManager.sparePart.gameObject.AddComponent<Rotatable>();
        }

        public override void ExitState(InputManagerController inputManager)
        {
            // inputManager.sparePart.GetComponent<ISwitchable>().Off();
            _rotationComponent.enabled = false;
        }
        
    }
}