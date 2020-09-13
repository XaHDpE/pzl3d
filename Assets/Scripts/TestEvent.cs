using Lean.Touch;
using UnityEngine;

[RequireComponent(typeof(LeanSelectable))]
public class TestEvent : MonoBehaviour
{
    private LeanSelectable _ls;

    private void Awake()
    {
        _ls = transform.GetComponent<LeanSelectable>();
    }

    private void OnEnable()
    {
        _ls.OnSelect.AddListener(OnSelectedEvent);
        _ls.OnDeselect.AddListener(OnDeSelectedEvent);
    }
    
    private void OnDisable()
    {
        _ls.OnSelect.RemoveAllListeners();
        _ls.OnDeselect.RemoveAllListeners();
    }

    private void OnSelectedEvent(LeanFinger finger)
    {
        print("OnSelectedEvent");
    }

    private void OnDeSelectedEvent()
    {
        print("OnDeSelectedEvent");
    }
    
}