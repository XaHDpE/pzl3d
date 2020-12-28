using models.state;
using states.controllers;

namespace states.sparepart
{
    public class SparePartMoveState : ObjectBaseState
    {

        public override void EnterState(InputManagerController inputManager)
        {
            // inputManager.sparePart.parent.GetComponent<ISwitchable>().On();
        }

        public override void ExitState(InputManagerController inputManager)
        {
            // inputManager.sparePart.parent.GetComponent<ISwitchable>().Off();
        }
        
    }
}