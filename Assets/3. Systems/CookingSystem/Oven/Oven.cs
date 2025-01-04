using System.Collections;
using UnityEngine;

public class Oven : CookingTable, IUpdateObserver {
    [Header("Oven Settings")]
    public Transform DoorTransform;
    public Light OvenLight;
    public float OpenAngle = 90f;
    public float ClosedAngle = 0f;
    public float DoorRotationSpeed = 5f;

    private bool _isDoorOpen;

    #region Unity Methods
    private void OnEnable() {
        UpdateManager.RegisterObserver(this);
    }
    public void ObservedUpdate() {
        UpdateDoorRotation();
        UpdateLightState();
    }

    private void OnDestroy() {
        UpdateManager.UnregisterObserver(this);
    }
    #endregion

    #region Interaction Methods
    public override void OnFocusEnter() {
        if(!_isCooking) {
            SetDoorState(true);
        }
        base.OnFocusEnter();
    }

    public override void OnFocusExit() {
        if(!_isCooking) {
            SetDoorState(false);
        }
        base.OnFocusExit();
    }
    #endregion

    #region Cooking Management
/*    protected override void TryStartCooking(FoodBoxObject foodBox, BoxController controller) {
        base.TryStartCooking(foodBox, controller);

        // Close the door if cooking starts successfully
        if(_activeCookingCoroutines.ContainsKey(foodBox)) {
            _isCooking = true;
            SetDoorState(false);
        }
    }*/

/*    protected override IEnumerator ProcessFood(CookingSlot slot) {
        while(slot.RemainingTime > 0) {
            slot.UpdateTime(Time.deltaTime);
            yield return null;
        }

*//*        CompleteCooking(slot);*//*
        _isCooking = false;
        SetDoorState(true); // Open the door after cooking completes
    }*/
    #endregion

    #region Door and Light Management
    private void SetDoorState(bool isOpen) {
        if(!_isCooking)
            _isDoorOpen = isOpen;
        else
            _isDoorOpen = false;
            
    }

    private void UpdateDoorRotation() {
        float targetAngle = _isDoorOpen ? OpenAngle : ClosedAngle;
        DoorTransform.localRotation = Quaternion.Lerp(DoorTransform.localRotation, Quaternion.Euler(0, targetAngle, 0), Time.deltaTime * DoorRotationSpeed);
    }

    private void UpdateLightState() {
        if(OvenLight != null) {
            OvenLight.enabled = _isCooking || _isDoorOpen;
        }
    }


    #endregion
}