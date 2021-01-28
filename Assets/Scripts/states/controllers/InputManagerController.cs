using System;
using System.Linq;
using models.state;
using states.sparepart;
using UnityEngine;

namespace states.controllers
{
    [Serializable]
    public class InputManagerController : MonoBehaviour
    {
        // public variables
        public Transform sparePart;

        public string stateName;
        public ObjectBaseState CurrentState { get; set; }

        private readonly CarouselItemSelectedState _spSelectedInCarouselState = new CarouselItemSelectedState();
        private readonly CarouselItemIdleState _spIdleState = new CarouselItemIdleState();
        private readonly TopBarSparePartState _spInTopBar = new TopBarSparePartState();

        // Internal State methods
        private void TransitionToState(ObjectBaseState state)
        {
            stateName = state.GetType().Name;
            CurrentState?.ExitState(this);
            CurrentState = state;
            CurrentState.EnterState(this); 
            // print($"object: {sparePart}, entered state : {state}");
        }
        
        // MonoBehaviour events for states re-definition 
        private void OnEnable()
        {
            sparePart = transform;
        }

        private void Start()
        {
            TransitionToState(_spIdleState);
        }

        private void Update()
        {
            CurrentState.LogicUpdate(this);
        }

        private void FixedUpdate()
        {
            CurrentState.PhysicsUpdate(this);
        }

        // External transition methods
        public void MoveToSelectedInList()
        {
            TransitionToState(_spSelectedInCarouselState);
        }

        public void MoveToIdle()
        {
            TransitionToState(_spIdleState);
        }

        public void MoveToSelectedInTopBarState()
        {
            TransitionToState(_spInTopBar);
        }
    


    }
}
