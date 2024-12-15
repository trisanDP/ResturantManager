using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {
    [Header("NPC Settings")]
    public float EatingSpeed = 1f; // Speed of eating food

    private FoodBoxObject _currentFood;
    private bool _isEating = false;

    #region Eating Logic
    public void StartEating(FoodBoxObject food) {
        if(_isEating) return;

        _currentFood = food;
        _isEating = true;
        Debug.Log($"{name} started eating {_currentFood.FoodName}.");
        StartCoroutine(EatFood());
    }

    private IEnumerator EatFood() {
        float eatTime = _currentFood.FoodItem.GetCurrentCookingStage(_currentFood.CurrentStageIndex).Duration / EatingSpeed;
        yield return new WaitForSeconds(eatTime);

        Debug.Log($"{name} finished eating {_currentFood.FoodName}.");
        _currentFood = null;
        _isEating = false;
    }
    #endregion

    #region Teleport Logic
    public void TeleportTo(Vector3 position, Quaternion rotation) {
        transform.SetPositionAndRotation(position, rotation);
    }
    #endregion
}

/*public class BoxController : MonoBehaviour {
    private FoodBoxObject _carriedBox;

    public bool HasCarriedBox() => _carriedBox != null;

    public FoodBoxObject GetCarriedBox() => _carriedBox;

    public void PickUpBox(FoodBoxObject box) {
        _carriedBox = box;
    }

    public void ClearCarriedBox() {
        _carriedBox = null;
    }

    public NPC SpawnNPC() {
        GameObject npcPrefab = Resources.Load<GameObject>("NPCPrefab"); // Ensure the prefab exists in Resources folder
        return Instantiate(npcPrefab).GetComponent<NPC>();
    }
}
*/