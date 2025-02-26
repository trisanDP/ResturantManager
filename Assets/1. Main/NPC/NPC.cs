// Shared Interfaces
using UnityEngine;
using UnityEngine.AI;

public interface IMovable {
    void MoveTo(Vector3 position);
    void StopMovement();
    bool IsMoving { get; }
}

// Base NPC Class (Optional)
public abstract class NPC : MonoBehaviour, IMovable, IInteractable {
    protected NavMeshAgent Agent;
    public bool IsMoving => Agent.velocity.magnitude > 0.1f;

    protected virtual void Awake() {
        Agent = GetComponent<NavMeshAgent>();
    }

    // Shared movement logic
    public void MoveTo(Vector3 position) {
        Agent.SetDestination(position);
    }

    public void StopMovement() {
        Agent.ResetPath();
    }

    // Abstract methods for child classes
    public abstract void OnFocusEnter();
    public abstract void OnFocusExit();
    public abstract void Interact(BoxController controller);
}