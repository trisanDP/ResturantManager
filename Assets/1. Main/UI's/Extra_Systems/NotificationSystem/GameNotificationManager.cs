using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameNotificationManager : MonoBehaviour {
    public static GameNotificationManager Instance; // Singleton pattern for easy access

    public GameObject notificationPrefab; // Prefab for the notification GameObject
    public Transform notificationPanel; // Parent object for notifications
    public float verticalSpacing = 30f; // Space between notifications
    public int maxSimultaneousNotifications = 3; // Max notifications displayed at once

    private Queue<NotificationData> notificationQueue = new Queue<NotificationData>(); // Queue for pending notifications
    private List<GameObject> activeNotifications = new List<GameObject>(); // List of currently displayed notifications

    private void Awake() {
        // Singleton setup
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        // Ensure the notification prefab is inactive at the start
        if(notificationPrefab != null) {
            notificationPrefab.SetActive(false);
        }
    }

    public void ShowNotification(string message, float time) {
        // Create a new notification data object
        NotificationData newNotification = new NotificationData {
            message = message,
            displayTime = time
        };

        // Add it to the queue
        notificationQueue.Enqueue(newNotification);

        // Process the queue
        ProcessQueue();
    }

    private void ProcessQueue() {
        // If there are active notifications and we've reached the max limit, wait
        if(activeNotifications.Count >= maxSimultaneousNotifications) {
            return;
        }

        // If there are notifications in the queue, display the next one
        if(notificationQueue.Count > 0) {
            NotificationData nextNotification = notificationQueue.Dequeue();
            StartCoroutine(DisplayNotification(nextNotification));
        }
    }

    private IEnumerator DisplayNotification(NotificationData notification) {
        // Instantiate the notification GameObject
        GameObject newNotification = Instantiate(notificationPrefab, notificationPanel);
        newNotification.SetActive(true); // Activate the GameObject

        // Update the notification text
        TextMeshProUGUI notificationText = newNotification.GetComponentInChildren<TextMeshProUGUI>();
        if(notificationText != null) {
            notificationText.text = notification.message;
        }

        // Add it to the list of active notifications
        activeNotifications.Add(newNotification);

        // Position the new notification
        UpdateNotificationPositions();

        // Fade in
        CanvasGroup canvasGroup = newNotification.GetComponent<CanvasGroup>();
        if(canvasGroup == null) {
            canvasGroup = newNotification.AddComponent<CanvasGroup>();
        }

        float elapsedTime = 0f;
        while(elapsedTime < 0.5f) { // Fade-in duration
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Wait for the display time
        yield return new WaitForSeconds(notification.displayTime);

        // Fade out
        elapsedTime = 0f;
        while(elapsedTime < 0.5f) { // Fade-out duration
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        // Remove the notification from the active list and destroy it
        activeNotifications.Remove(newNotification);
        Destroy(newNotification);

        // Update positions of remaining notifications
        UpdateNotificationPositions();

        // Process the next notification in the queue
        ProcessQueue();
    }

    private void UpdateNotificationPositions() {
        // Update the positions of all active notifications
        for(int i = 0; i < activeNotifications.Count; i++) {
            RectTransform rectTransform = activeNotifications[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0f, -(activeNotifications.Count - 1 - i) * verticalSpacing);
        }
    }



    private struct NotificationData {
        public string message;
        public float displayTime;
    }
}