public interface IInteractable {
    // Called when the player interacts with the object
    void Interact();

    // Called when the player starts looking at the object
    void OnFocusEnter();

    // Called when the player stops looking at the object
    void OnFocusExit();
}
