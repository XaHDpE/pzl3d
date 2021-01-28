using System;
using models.sparepart;
using models.state;
using states.controllers;
using UnityEngine;

namespace states.sparepart
{
    public class TopBarSparePartState : ObjectBaseState
    {
        
        public delegate void ItemIsBackToCarouselDelegate(InputManagerController target);
        public static event ItemIsBackToCarouselDelegate ItemIsBackToCarousel;

        private InputManagerController _inputManager;
        
        public override void EnterState(InputManagerController inputManager)
        {
            _inputManager = inputManager;
            Debug.Log($"{_inputManager.sparePart} moved to TopBarSparePartState");
            _inputManager.sparePart.gameObject.AddComponent<Swipable>();
            // register events
            // Swipable.SwipeDown += SwipeToCarousel;
        }

        public override void ExitState(InputManagerController inputManager)
        {
            // Swipable.SwipeDown += SwipeToCarousel;
        }

        public override void LogicUpdate(InputManagerController inputManager) {}

        public override void PhysicsUpdate(InputManagerController inputManager) { }
        
        private void SwipeToCarousel(Vector2 delta)
        {
            ItemIsBackToCarousel?.Invoke(_inputManager);
        }
        
    }
}