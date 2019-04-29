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

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void FadeIn(Action callback, float callbackDelay = 0f)
    {
        m_OnFadeInOutFinish = callback;
        m_CallbackDelay = callbackDelay;
        m_CallbackReady = false;
        m_Animator.SetTrigger("FadeIn");
    }

    public void SetCardText(string text)
    {
        Text.text = text + " Start";
    }

    public void FadeOut(Action callback, float callbackDelay = 0f)
    {
        Debug.Log("Card Fade Out");
        m_OnFadeInOutFinish = callback;
        m_CallbackDelay = callbackDelay;
        m_CallbackReady = false;
        m_Animator.SetTrigger("FadeOut");
    }

    public void OnFadeEnd()
    {
        m_CallbackReady = true;
    }

    // Update is called once per frame
    void Update()
    {
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
}
