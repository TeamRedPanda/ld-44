using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleCardView : MonoBehaviour
{
    private Animator m_Animator;
    private Action m_OnFadeInOutFinish;
    private float m_CallbackDelay;

    public TMPro.TextMeshProUGUI Text;

    private bool m_CallbackReady;

    private class DelayedTrigger
    {
        public float TriggerTime;
        public string Trigger;
    }

    private List<DelayedTrigger> m_DelayedTriggers = new List<DelayedTrigger>();

    // Start is called before the first frame update
    void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void FadeIn(float startDelay = 0f, Action callback = null, float callbackDelay = 0f)
    {
        m_OnFadeInOutFinish = callback;
        m_CallbackDelay = callbackDelay;
        m_CallbackReady = false;

        if (startDelay == 0f) {
            m_Animator.SetTrigger("FadeIn");
        } else {
            QueueTrigger(startDelay, "FadeIn");
        }
    }

    public void SetCardText(string text)
    {
        Text.text = text + " Start";
    }

    public void FadeOut(float startDelay = 0f, Action callback = null, float callbackDelay = 0f)
    {
        Debug.Log("Card Fade Out");
        m_OnFadeInOutFinish = callback;
        m_CallbackDelay = callbackDelay;
        m_CallbackReady = false;

        if (startDelay == 0f) {
            m_Animator.SetTrigger("FadeOut");
        } else {
            QueueTrigger(startDelay, "FadeOut");
        }
    }

    private void QueueTrigger(float startDelay, string trigger)
    {
        DelayedTrigger dt = new DelayedTrigger();
        dt.TriggerTime = Time.time + startDelay;
        dt.Trigger = trigger;

        m_DelayedTriggers.Add(dt);
    }

    public void OnFadeEnd()
    {
        m_CallbackReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDelayedTriggers();

        // If animation is still playing or we already called the callback.
        if (m_CallbackReady == false || m_OnFadeInOutFinish == null)
            return;

        m_CallbackDelay -= Time.deltaTime;

        // Not enough time passed yet.
        if (m_CallbackDelay > 0)
            return;

        m_OnFadeInOutFinish?.Invoke();
        m_OnFadeInOutFinish = null;
    }

    private void UpdateDelayedTriggers()
    {
        for (int i = m_DelayedTriggers.Count - 1; i >= 0; i--) {
            var trigger = m_DelayedTriggers[i];

            Debug.Log($"{trigger.Trigger} : {trigger.TriggerTime} out of {Time.time}");
            if (trigger.TriggerTime <= Time.time) {
                Debug.Log($"Playing delayed trigger {trigger.Trigger}");
                m_Animator.SetTrigger(trigger.Trigger);
                m_DelayedTriggers.RemoveAt(i);
            }
        }
    }
}
