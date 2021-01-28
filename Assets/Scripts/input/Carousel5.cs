using System;
using System.Collections;
using System.Collections.Generic;
using gizmo;
using helpers;
using Lean.Touch;
using models.sparepart;
using states.controllers;
using states.sparepart;
using UnityEngine;
using cs = input.CarouselSettings;
using th = helpers.TransformHelper;

namespace input
{
    public class Carousel5 : MonoBehaviour
    {
        
        public delegate void CarouselInitializedDelegate(Vector3 carouselExtents, Vector3 lookPoint, Vector3 lookDirection);
        public static event CarouselInitializedDelegate CarouselArranged;

        public Transform itemsParent;
        public float deltaAngle;

        [SerializeField]
        private List<ObjectStateManager> carouselItems;
        private LeanThresholdDelta _ltd;
        private Transform _rayCaster;
        private Quaternion _previousRotation;
        private bool _rotationInProgress;
        
        // angle calculation
        private float _alpha;

        [SerializeField]
        private int currNum;
        private int _currNumTmp, _totalItems;
        [SerializeField]
        private int prevNum =-1;
        private bool _arrangementDone;

        private void OnEnable()
        {
            TopBarController.TopBarRearranged += RemoveItem;
            TopBarSparePartState.ItemIsBackToCarousel += AddItem;
        }

        private void OnDisable()
        {
            TopBarController.TopBarRearranged -= RemoveItem;
            TopBarSparePartState.ItemIsBackToCarousel -= AddItem;
        }

        private void Awake()
        {
            Arrange();
        }

        private void Arrange()
        {
            _ltd = GetComponent<LeanThresholdDelta>();
            
            _arrangementDone = false;
            InitSetup();
            carouselItems = SetCarouselItems(itemsParent);
            DoInternal(true);
        }

        private void AddItem(InputManagerController imc)
        {
            _arrangementDone = false;
            carouselItems.Add(imc.transform.GetComponent<ObjectStateManager>());
            DoInternal(false);
        }
        
        private void RemoveItem()
        {
            _arrangementDone = false;
            RearrangeArray(currNum, ref carouselItems);
            DoInternal(false);
        }

        private void DoInternal(bool isInit)
        {
            carouselItems.ForEach(item => item.transform.SetParent(_rayCaster));
            ArrangeInCircle(_rayCaster, carouselItems, cs.Radius, isInit);
            // reset parent rotation
            transform.rotation = Quaternion.identity;
            carouselItems.ForEach(item => item.transform.SetParent(transform));
        }
        

        private void SetGlobalVars(List<ObjectStateManager> items)
        {
            _totalItems = items.Count;
            deltaAngle = 360.0f / _totalItems;
            currNum = 0;
            prevNum = -1;
            _ltd.Threshold = deltaAngle;
        }

        private void FixedUpdate()
        {
            if (!_arrangementDone) return;
            
            _alpha = (transform.eulerAngles.y + deltaAngle * 1 / 2) % 360;
            _currNumTmp = Mathf.RoundToInt((_alpha - _alpha % deltaAngle) / deltaAngle);
            currNum = (_totalItems - _currNumTmp)%_totalItems;
            
            //Debug.Log($"_alpha: {_alpha}, _currNum: {_currNum}, _currNumTmp: {_currNumTmp}, transform.eulerAngles.y: {transform.eulerAngles.y}");

            if (currNum != prevNum)
            {
                if (prevNum > -1)
                {
                    carouselItems[prevNum].imc.MoveToIdle();
                }
                carouselItems[currNum].imc.MoveToSelectedInList();
            }
            prevNum = currNum;
        }

        private void CreateRc()
        {
            _rayCaster = new GameObject("raycaster").transform;
            var transform1 = transform;
            _rayCaster.position = transform1.position;
            _rayCaster.rotation = transform1.rotation;
        }
        private void InitSetup()
        {
            _ltd.Current = Vector3.zero;
            transform.rotation = Quaternion.identity;
            CreateRc();
        }
        private List<ObjectStateManager> SetCarouselItems(Transform container)
        {
            var res = new List<ObjectStateManager>(Array.ConvertAll(
                container.GetComponentsInChildren<Transform>(), SparePartConverter.TransformToCarouselItem
            ));
            res.RemoveAt(0);
            return res;
        }
        


        private static void RearrangeArray(int excludeIndex, ref List<ObjectStateManager> elementsArray)
        {
            var elementsTotal = elementsArray.Count;
            var nextIndex = (excludeIndex + 1) % elementsTotal;
            var prevIndex = (excludeIndex  + (elementsTotal)) % elementsTotal;
            var remainingCount = elementsTotal - nextIndex;
            var tmpList = elementsArray.GetRange(nextIndex, remainingCount);    
            tmpList.AddRange(elementsArray.GetRange(0, prevIndex));
            elementsArray = tmpList;
        }

        private void ArrangeInCircle(Transform top, List<ObjectStateManager> items, float radius, bool isInit)
        {
            var allMovers = new IEnumerator[items.Count];
            for (var index = 0; index < items.Count; index++)
            {
                var (nPos, nRot) = GetCirclePosAndRot(top.position, index, items.Count, radius);
                allMovers[index] = th.MoveRotateScale(this, items[index].transform, nPos, nRot, cs.DownScale, 0.3f);
            }
            StartCoroutine(OrderElements(allMovers, () =>
            {
                if (isInit)
                {
                    var transform2 = transform;
                    var fwd = transform2.forward;
                    CarouselArranged?.Invoke(
                        ShowMeshBounds.CalculateLocalBounds(transform2).extents,
                        transform.position + fwd * cs.Radius,
                        -fwd
                    );
                }
                SetGlobalVars(items);
                _arrangementDone = true;
            }));
        }

        private static (Vector3 pos, Quaternion) GetCirclePosAndRot(Vector3 parentPosition, int idx, int totalItems, float radius)
        {
            var angle = idx * Mathf.PI * 2 / totalItems;
            var x = Mathf.Sin(angle) * radius;
            var z = Mathf.Cos(angle) * radius;
            var pos = parentPosition + new Vector3(x, 0, z);
            var angleDegrees = -angle * Mathf.Rad2Deg;
            return (pos, Quaternion.Euler(0, angleDegrees, 0));
        }

        [Obsolete]
        private void ArrangeToCircle12(Transform carouselCenter, List<ObjectStateManager> items, float radius, bool isInit)
        {
            _totalItems = carouselItems.Count;
            deltaAngle = 360.0f / _totalItems;

            _ltd.Threshold = deltaAngle;
            var centerPos = carouselCenter.position;
            var idx = 0;
            var allMovers = new IEnumerator[items.Count];
            
            foreach (var item in items)
            {
                var angle = idx * Mathf.PI * 2 / _totalItems;
                var x = Mathf.Sin(angle) * radius;
                var z = Mathf.Cos(angle) * radius;
                var pos = centerPos + new Vector3(x, 0, z);
                var angleDegrees = -angle * Mathf.Rad2Deg;
                var rot = Quaternion.Euler(0, angleDegrees, 0);
                
                item.transform.rotation = rot;
                item.transform.SetParent(carouselCenter);

                item.imc.MoveToIdle(); 

                allMovers[idx++] = th.MoveScale(this, item.transform, pos, cs.DownScale, 0.3f);
            }

            StartCoroutine(OrderElements(allMovers, () =>
            {
                var transform2 = transform;
                var fwd = transform2.forward;
                if (CarouselArranged != null && isInit)
                    CarouselArranged.Invoke(
                        ShowMeshBounds.CalculateLocalBounds(transform2).extents,
                        transform.position + fwd * cs.Radius,
                        -fwd
                    );
                _arrangementDone = true;
            }));
        }

        private IEnumerator OrderElements(IEnumerator[] movers, Action callback)    
        {
            var cors = new List<Coroutine>();
            
            foreach (var mover in movers)
            {
                cors.Add(StartCoroutine(mover));
            }
            
            foreach (var cor in cors)
            {
                yield return cor;
            }
            callback();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
//            Gizmos.DrawWireSphere(selectedObject.transform.position, 5);
        }
    }
}