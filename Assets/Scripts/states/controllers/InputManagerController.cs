using events;
using models.state;
using states.sparepart;
using UnityEngine;

namespace states.controllers
{
    public class InputManagerController : MonoBehaviour
    {
        public Transform sparePart;
        public CustomEventsManager eventMgr;

        private ObjectBaseState CurrentState { get; set; }

        private readonly SparePartRotatableState _spRotateState = new SparePartRotatableState();
        private readonly SparePartMoveState _spMoveState = new SparePartMoveState();
        private readonly SparePartIdleState _spIdleState = new SparePartIdleState();
        public readonly CompositeIdleState CompositeIdleState = new CompositeIdleState();
        private readonly CompositeRotateState _compositeRotState = new CompositeRotateState();

        private void OnEnable()
        {
            // eventMgr.SparePartMovementRequested += EnableSpMovement;
            // eventMgr.CompositeRotationRequested += EnableCompositeMovement;
            // eventMgr.SparePartRotationRequested += EnableSpRotation;
        }

        public void EnableSpRotation()
        {
            TransitionToState(_spRotateState);
        }

        public void EnableSpMovement()
        {
            TransitionToState(_spMoveState);
        }

        public void EnableCompositeMovement()
        {
            TransitionToState(_compositeRotState);
        }

        private void EnableSpIdle()
        {
            TransitionToState(_spIdleState);
        }
    
        private void TransitionToState(ObjectBaseState state)
        {
            CurrentState?.ExitState(this);
            CurrentState = state;
            CurrentState.EnterState(this); 
            // print($"object: {sparePart}, entered state : {state}");
        }
    
        private void Start()
        {
            TransitionToState(_compositeRotState);
        }

    }
}
