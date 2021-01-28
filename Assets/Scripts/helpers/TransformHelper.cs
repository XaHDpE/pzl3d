using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace helpers
{
    public static class TransformHelper
    {

        public static IEnumerator MoveFromTo(Transform what, Vector3 a, Vector3 b, float speed) {
            var step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
            float t = 0;
            while (t <= 1.0f) {
                t += step; // Goes from 0 to 1, incrementing by step each time
                what.position = Vector3.Lerp(a, b, t);
                yield return new WaitForFixedUpdate();
            }
            what.position = b;
        }
        
        public static IEnumerator ScaleFromTo(Transform what, Vector3 a, Vector3 b, float speed) {
            while (Vector3.SqrMagnitude(a - b) > 0.001)
            {
                Debug.Log($"a: {a}, b: {b}, res: {Vector3.SqrMagnitude(a - b)}");
                what.localScale = Vector3.Lerp(a, b, Time.fixedDeltaTime * speed);
                yield return new WaitForFixedUpdate();
            }
            what.localScale = b;
        }      
        
        public static IEnumerator MoveFromToByTime(Transform what, Vector3 a, Vector3 b, float seconds) {
            float t = 0;
            while (t <= 1.0f) {
                t += Time.fixedDeltaTime / seconds; // Goes from 0 to 1, incrementing by step each time
                what.position = Vector3.Lerp(a, b, t);
                yield return new WaitForFixedUpdate();
            }
            what.position = b;
        }
        
        public static IEnumerator MoveFromToByTime(Transform what, Vector3 a, Vector3 b, float seconds, bool isLocal) {
            float t = 0;
            while (t <= 1.0f) {
                t += Time.fixedDeltaTime / seconds; // Goes from 0 to 1, incrementing by step each time
                if (isLocal)
                {
                    what.localPosition = Vector3.Lerp(a, b, t);
                }
                else
                {
                    what.position = Vector3.Lerp(a, b, t);                    
                }
                yield return new WaitForFixedUpdate();
            }
            what.position = b;
        }
        public static IEnumerator RotateTo(Transform what, Quaternion finalRotation, float rotateSpeed)
        {
            while (!Approximately(what.rotation, finalRotation, 0.00001f))
            {
                what.rotation = Quaternion.RotateTowards(
                    what.rotation, 
                    finalRotation, 
                    Time.fixedDeltaTime * rotateSpeed); 
                yield return new WaitForFixedUpdate();
            }
        }
        
        public static IEnumerator RotateByTime(Transform what, Quaternion targetRotation, float duration)
        {
            var passedTime = 0f;
            var startRotation = what.rotation;

            while(passedTime < duration)
            {
                var lerpFactor = passedTime / duration;
                lerpFactor = Mathf.SmoothStep(0, 1, lerpFactor);
                what.rotation = Quaternion.Lerp(startRotation, targetRotation, lerpFactor);
                passedTime += Mathf.Min(duration - passedTime, Time.deltaTime);
                yield return null;
            }
            what.rotation = targetRotation;
        }

        public static IEnumerator ChangeScaleByTime(Transform what, Vector3 initScale, Vector3 targetScale, float timeTakes)
        {
            float elapsedTime = 0;
            while (elapsedTime < timeTakes)
            {
                what.localScale = Vector3.Lerp(initScale, targetScale, (elapsedTime / timeTakes));
                elapsedTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
        public static Bounds GetHierarchicalBounds(GameObject go)
        {
            var boundsArray = new List<Bounds>();
            var center = Vector3.zero;

            foreach ( var child in go.GetComponentsInChildren<Transform>())
            {
                if (!child.gameObject.TryGetComponent(out MeshRenderer itemRenderer)) continue;
                if (itemRenderer == null) continue;
                var bounds = itemRenderer.bounds;
                center += bounds.center;
                boundsArray.Add(bounds);
            }

            center /= go.transform.childCount;
            var totalBound = new Bounds(center, Vector3.zero);
 
            foreach (var t in boundsArray)
            {
                totalBound.Encapsulate(t);
            }

            // find center
            /*var xCenter = (totalBound.min.x + totalBound.max.x) / 2;
            var yCenter = (totalBound.min.y + totalBound.max.y) / 2;
            var zCenter = (totalBound.min.z + totalBound.max.z) / 2;
            
            totalBound.center = new Vector3(xCenter, yCenter, zCenter);*/
            
            return totalBound;
        }

        public static IEnumerator MoveScale(MonoBehaviour context, Transform what, Vector3 where, Vector3 finalScale, float timeTaken)
        {
            var moveFunc = context.StartCoroutine(
                MoveFromToByTime(what, what.position, where, timeTaken));

            var scaleFunc = context.StartCoroutine(
                ChangeScaleByTime(what, what.localScale, finalScale, timeTaken));

            //wait until all of them are over
            yield return moveFunc;
            yield return scaleFunc;
        }

        public static IEnumerator SyncRotationAndPosition(MonoBehaviour ctx, Transform source, Transform target, float timeTaken)
        {
            var moveFunc = ctx.StartCoroutine(
                MoveFromToByTime(target, target.position, source.position, timeTaken));

            var rotateFunc = ctx.StartCoroutine(
                RotateByTime(target, source.rotation, timeTaken));

            //wait until all of them are over
            yield return moveFunc;
            yield return rotateFunc;
        }

        public static IEnumerator WrapOnAction(MonoBehaviour ctx, IEnumerator func, Action resultAction)
        {
            var execFunc = ctx.StartCoroutine(func);
            yield return execFunc;
            
        }
        
        public static IEnumerator MoveRotateScale(
            MonoBehaviour ctx,
            Transform target,
            Vector3 finalPosition,
            Quaternion finalRotation,
            Vector3 finalScale,
            float timeTaken)
        {
            var moveFunc = ctx.StartCoroutine(
                MoveFromToByTime(target, target.position, finalPosition, timeTaken));
            
            var rotateFunc = ctx.StartCoroutine(
                RotateByTime(target, finalRotation, timeTaken));

            var scaleFunc = ctx.StartCoroutine(
                ChangeScaleByTime(target, target.localScale, finalScale, timeTaken));

            yield return rotateFunc;
            yield return moveFunc;
            yield return scaleFunc;
        }
        
        public static bool Approximately(this Quaternion quatA, Quaternion value, float acceptableRange)
        {
            return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
        }
        
    }
}