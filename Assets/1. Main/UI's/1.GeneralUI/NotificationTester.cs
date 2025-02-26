using UnityEngine;
using System.Collections;

public class NotificationTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ShowNotifications());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ShowNotifications() {
        GameNotificationManager.Instance.ShowNotification("Hello", 3f);
        yield return new WaitForSeconds(1f);

        GameNotificationManager.Instance.ShowNotification("Hi", 2f);
        yield return new WaitForSeconds(1f);

        GameNotificationManager.Instance.ShowNotification("Welcome!", 4f);
        yield return new WaitForSeconds(1f);

        GameNotificationManager.Instance.ShowNotification("Goodbye", 2f);
    }
}
