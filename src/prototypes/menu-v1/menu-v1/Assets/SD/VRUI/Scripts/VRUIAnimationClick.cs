using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using System;

public class VRUIAnimationClick : MonoBehaviour {

    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float scaleOffset = -0.1f;
    [SerializeField] private VRInteractiveItem button;

    private bool isActive = false;

    public event Action OnClick;
    public event Action OnAnimationComplete;

    // Use this for initialization
    void Start () {
        button.OnClick += OnButtonClick;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnButtonClick()
    {
        if (!isActive)
        {
            StartCoroutine(Animate());
        }
    }

    private IEnumerator Animate()
    {
        if(OnClick != null)
            OnClick();

        isActive = true;
        Vector3 baseScale = gameObject.transform.localScale;
        float timer = 0f;
        while (timer < duration)
        {
            float factor = Mathf.Sin(Mathf.Lerp(0f, 1f, timer / duration) * Mathf.PI);
            gameObject.transform.localScale = baseScale / (1 - factor * scaleOffset);

            timer += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.localScale = baseScale;
        isActive = false;

        if (OnAnimationComplete != null)
            OnAnimationComplete();
    }
}
