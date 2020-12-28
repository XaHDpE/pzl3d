using System;
using System.Linq;
using camera;
using settings;
using states.controllers;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace input
{
	public class CreateCarousel : MonoBehaviour
	{

		public InputManagerController inputController;
		public ObjectReferences refs;
		
		public Transform carouselParent;
		private Transform[] _carouselObjects;
		public bool resetCenterRotation = true;
		public float distanceFromCenter = 15.0f;
		public bool assumeObject = true;
		public int chosenObject, previousObject; 
		public float speedOfRotation = 0.1f;
	
		private const float Diameter = 360.0f; 
		private Transform _theRayCaster;
		private float _angle, _newAngle;
		private bool _firstTime = true;
		private float _journeyUpLength = 0;
		
		 
		// downscaled size
		private const float DS = 0.5f;
		// upscale size
		private const float US = 1.5f;
		private Vector3 _smallSize;
		
		// top default position
		private Vector3 _topDefaultPos;
		

		private Transform[] GetItems()
		{
			return carouselParent.GetComponentsInChildren<Transform>().Where((source, index) => index != 0).ToArray();
		}

		private void Awake()
		{
			// cache components
		}

		private void Start ()
		{
			CreateInfrastructure();
			_carouselObjects = GetItems();
			Initialize();
		}

		private void CreateInfrastructure()
		{
			_smallSize = new Vector3(DS, DS ,DS);
			var raycastHolder = new GameObject ();
			raycastHolder.name = "RaycastPicker";
			_theRayCaster = raycastHolder.transform;
			_theRayCaster.position = transform.position; // place it at the positon of the carousel center			
		}

		private void Initialize()
		{

			if (resetCenterRotation) {
				transform.rotation = Quaternion.identity;//reset the rotation of the carousel center
			}

			previousObject = -1;

			_angle = Diameter / _carouselObjects.Length;//calculate the angle according to the number of elements
			var objectAngle = _angle;//create a temp value that keeps track of the angle of each element

			foreach (var currItem in _carouselObjects)
			{
				SetCarouselItem(currItem, objectAngle);
				objectAngle += _angle;
			}

			if (_carouselObjects.Length % 2 != 0) {
				var rotateAngle = _angle + _angle / 2;
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, rotateAngle, transform.eulerAngles.z);
				_newAngle = rotateAngle;
			}
			else 
			{
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, _angle, transform.eulerAngles.z);
				_newAngle = _angle;
			}

			_theRayCaster.position = transform.position;
			
			var objectName = "";
			RaycastHit hit;
			if (Physics.Raycast(transform.position, -_theRayCaster.forward, out hit, distanceFromCenter )) 
			{
				objectName = hit.collider.name;
			}

			if (objectName == _carouselObjects[0].name) return;
			
			for (var c = 0; c < _carouselObjects.Length; c++) 
			{
				if (_carouselObjects[c].name != objectName) continue;
				float angleMultiplier = c++;
				var trAngle = transform.eulerAngles;
				transform.eulerAngles = new Vector3 (trAngle.x, trAngle.y + _angle * angleMultiplier, trAngle.z);
				_newAngle = trAngle.y;
				break;
			}
		}
		
		
		private void SetCarouselItem(Transform currItem, float objectAngle)
		{
			var parent = transform;
			var parentPos = parent.position;
			currItem.position = parentPos;
			currItem.rotation = Quaternion.identity;
			currItem.parent = parent;
			currItem.position = new Vector3 (parentPos.x, parentPos.y, parentPos.z + distanceFromCenter);
			currItem.localScale = new Vector3(DS, DS, DS); 
			currItem.RotateAround (parentPos, new Vector3 (0, 1, 0), objectAngle);
		}
		
		private void FixedUpdate () 
		{
			if (assumeObject == false) 
			{
				_theRayCaster.position = transform.position;
				RaycastHit hit;
				if (Physics.Raycast (transform.position, -_theRayCaster.forward, out hit, distanceFromCenter)) {
					Debug.Log (hit.collider.name);//display in the console which element is detected
				}
			}

			var newRotation = Quaternion.AngleAxis(_newAngle, Vector3.up);
			
			transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speedOfRotation);
			// Debug.Log($"newRotation: {Vector3.Dot(newRotation.eulerAngles, transform.rotation.eulerAngles)}");
			
			_carouselObjects[chosenObject].localScale =
				Vector3.Lerp(_carouselObjects[chosenObject].localScale, new Vector3(US,US,US), speedOfRotation);

			if (previousObject > -1)
			{
				_carouselObjects[previousObject].localScale = 
					Vector3.Lerp(_carouselObjects[previousObject].localScale, _smallSize, speedOfRotation);
			}

			// emit the newItemIsPlacedInFront 
			if (Mathf.Abs(Quaternion.Dot(transform.rotation, newRotation)) > 0.999999f)
			{
			}

			if (_journeyUpLength > 0 )
			{
				//if (Vector3.Distance(_carouselObjects[previousObject].position, _topDefaultPos) < 0.)
				_carouselObjects[chosenObject].position = Vector3.Lerp(
					_carouselObjects[chosenObject].position, 
					_topDefaultPos, 
					15f * Time.fixedDeltaTime);
			}

		}

		public void RotateTheCarouselLeft()
		{
			previousObject = chosenObject;
			if (_firstTime)
			{
				_newAngle = transform.eulerAngles.y;
				_newAngle += _angle;
				_firstTime = false; // stop this piece of code from running in the future
			}
			else
			{
				_newAngle += _angle;
			}

			if (!assumeObject) return;
			if (chosenObject <= 0) {
				chosenObject = _carouselObjects.Length - 1;
			} else {
				chosenObject--;
			}
			// Debug.Log(_carouselObjects[chosenObject].name);
		}

		public void RotateTheCarouselRight()
		{
			previousObject = chosenObject;
			if (_firstTime)
			{
				_newAngle = transform.eulerAngles.y;
				_newAngle -= _angle;
				_firstTime = false;
			}
			else
			{
				_newAngle -= _angle;
			}
			if (!assumeObject) return;
			if (chosenObject >= _carouselObjects.Length-1) {
				chosenObject = 0;
			} else {
				chosenObject++;
			}
			// Debug.Log(_carouselObjects[chosenObject].name);
		}

		public void SwipeUp()
		{
			_topDefaultPos = refs.mainCam.GetComponent<Camera>().ViewportToWorldPoint(
				new Vector3(0.5f, 0.8f, ScreenConstants.DefaultDepth));
			_journeyUpLength = Vector3.Distance(_carouselObjects[chosenObject].position, _topDefaultPos);
			Debug.Log($"_journeyUpLength: {_journeyUpLength}");
				
		}

		public void DoStaff()
		{
			// inputController.sparePart = _carouselObjects[idx];
			// inputController.EnableSpRotation();
			
			Debug.Log($"name: {SettingsReader.Instance.GameSettings.OnSpSelectedTop.name}");
			// ProcessSelected();
			// Test();
		}

		
		
		private void ProcessSelected()
		{
			// var od = _carouselObjects[chosenObject].gameObject.AddComponent<ObjectDebugger>();
			// od.SetText(chosenObject.ToString());

			//Test();
			// Initialize();

			
			/*var topLeftGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
			topLeftGo.gameObject.layer = LayerMask.NameToLayer("Green");
			topLeftGo.transform.position = refs.uiCam.GetComponent<Camera>().ViewportToWorldPoint(
				new Vector3(0, 1, ScreenConstants.DefaultDepth));
			
			
			topLeftGo.transform.SetParent(refs.selectedPartsTop.transform);
			topLeftGo.transform.localScale = new Vector3(1, 1, 1);
			refs.uiCam.SetTarget(topLeftGo.transform);*/
			
			refs.mainCam.SetTarget(_carouselObjects[chosenObject]);
			
		}

		private void ExcludeFromCarousel(int idx)
		{
			// _carouselObjects[chosenObject].SetParent(uiParent.transform);
			_carouselObjects = _carouselObjects.Where((val, index) => index != chosenObject).ToArray();
		} 

		private void Test()
		{
			
			var bounds = _carouselObjects[chosenObject].GetComponent<MeshFilter>().sharedMesh.bounds;

		}

	}
}
