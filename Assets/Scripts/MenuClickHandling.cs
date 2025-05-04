using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuClickHandling : MonoBehaviour
{

    public AddBuilding addBuildingScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // panel game object
    public GameObject panel;

    // Update is called once per frame
    void Update()
    {

    }

    public void ZoomMenuToClose()
    {
        StartCoroutine(AnimatePanel());
    }



    private IEnumerator AnimatePanel()
    {
        // Animate the panel to scale down to 0 over 0.5 seconds
        float duration = 0.5f;
        Vector3 startScale = panel.transform.localScale;
        Vector3 endScale = new Vector3(0, 0, 0);
        Vector3 startPosition = panel.transform.localPosition;
        Vector3 endPosition = new Vector3(0, -1200, 0);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            // panel.transform.localScale = Vector3.Lerp(startScale, endScale, normalizedTime);
            panel.transform.localPosition = Vector3.Lerp(startPosition, endPosition, normalizedTime);
            yield return null;
        }

        // Ensure the final scale and position are set
        // panel.transform.localScale = endScale;
        panel.transform.localPosition = endPosition;
    }
}
