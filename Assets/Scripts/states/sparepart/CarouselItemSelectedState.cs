using System;
using helpers;
using input;
using models.sparepart;
using models.state;
using states.controllers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace states.sparepart
{
    [Serializable]
    public class CarouselItemSelectedState : ObjectBaseState
    {

        int _rotationSpeed = 15;
        private Coroutine _ct;
        private InputManagerController _imc;
        
        
        // events and delegates
        public delegate void NewElementInTopBarDelegate(InputManagerController target);
        public static event NewElementInTopBarDelegate NewIsMovingInTopBar;
        
        public override void EnterState(InputManagerController inputManager)
        {
            Debug.Log($"CarouselItemSelectedState: {inputManager.sparePart} entered Selected");
            _imc = inputManager;
            ChangeScale(inputManager, CarouselSettings.UpScale);
            inputManager.sparePart.gameObject.AddComponent<Swipable>();
            // register events
            Swipable.SwipeUp += SwipeToTopBar;
        }

        public override void ExitState(InputManagerController inputManager)
        {
            // inputManager.sparePart.parent.GetComponent<ISwitchable>().Off();
            ChangeScale(inputManager, CarouselSettings.DownScale);
            Object.Destroy(inputManager.sparePart.gameObject.GetComponent<Swipable>());
            
            // unregister events
            Swipable.SwipeUp -= SwipeToTopBar;
        }

        public override void LogicUpdate(InputManagerController inputManager)
        {
            
        }

        public override void PhysicsUpdate(InputManagerController inputManager)
        {
            inputManager.transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
        }

        private void ChangeScale(MonoBehaviour imc, Vector3 sf)
        {
            var trn = _imc.transform;
            if (_ct != null) 
                imc.StopAllCoroutines();
            _ct = imc.StartCoroutine(TransformHelper.ChangeScaleByTime(trn, trn.localScale, sf, 0.2f));

        }

        private void SwipeToTopBar(Vector2 delta)
        {
            NewIsMovingInTopBar?.Invoke(_imc);
        }
        
        
    }
}