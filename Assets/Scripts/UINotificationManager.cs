using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINotificationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notifText;
    [SerializeField] private Transform textTransform;

    [SerializeField] private RectTransform notifTextReactTransform;
    [SerializeField] private bool effectIn;
    [SerializeField] private bool effectSlow;
    [SerializeField] private bool effectOut;

    [SerializeField] private float timer;
    [SerializeField] private float timeForNotificationDisplay;

    [SerializeField] private int timeForNormalNotificationDisplay; 
    [SerializeField] private int timeForQueuedNotificationDisplay;

    [SerializeField] private bool isNotificationActive;
    [SerializeField] private bool isNotificationPending;

    [SerializeField] private List<string> notificationList = new List<string>();

    [SerializeField] private static UINotificationManager Instance;

    private void Start()
    {
        //if (Instance != null)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //Instance = this;
        //GameObject.DontDestroyOnLoad(this.gameObject);

        notifTextReactTransform = notifText.rectTransform;
        notifTextReactTransform.anchoredPosition = new Vector2(0, -300);
        textTransform.gameObject.SetActive(false);
        notifText.text = "";
    }

    private void Update()
    {
       
        if (notificationList.Count > 0)
        {
            isNotificationPending = true;
            timeForNotificationDisplay = timeForQueuedNotificationDisplay;
        }
        else
        {
            isNotificationPending = false;
            timeForNotificationDisplay = timeForNormalNotificationDisplay;
        }


        if (isNotificationActive)
        {
            timer += Time.deltaTime;
            Effect();
            if (timer >= timeForNotificationDisplay)
            {
                notifText.text = "";
                isNotificationActive = false;
                timer = 0;
                ResetEffect();
            }
        }

        if (isNotificationPending && !isNotificationActive)
        {
            SetText(notificationList[0]);
            notificationList.RemoveAt(0);
        }
    }

    public void SetText(string text)
    {
        if (!isNotificationActive)
        {
            notifText.text = text;
            isNotificationActive = true;
        }
        else
        {
            bool isNotificationAlreadyReceived = false;
            foreach (var item in notificationList)
            {
                if (text == item)
                {
                    isNotificationAlreadyReceived = true; break;
                }
            }

            if (!isNotificationAlreadyReceived) notificationList.Add(text);

        }
        
    }

    public void SetGameOverText()
    {
        notificationList.Clear();
        SetText("you died");
    }

    private void Effect()
    {
        textTransform.gameObject.SetActive(true);
        
    }

    private void ResetEffect()
    {
        textTransform.gameObject.SetActive(false);
        notifTextReactTransform.anchoredPosition = new Vector2(0, -300);
    }
}
