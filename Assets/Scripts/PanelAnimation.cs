using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelAnimation : MonoBehaviour
{
    public float animationDuration = 1.5f; // Duration of the alpha animation
    private Image panelImage;

    void Start()
    {
        // Get the Image component
        panelImage = GetComponent<Image>();
        // Deactivate the GameObject initially
        gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        // Activate the GameObject
        gameObject.SetActive(true);

        panelImage.DOFade(0f, animationDuration).SetDelay(0f).SetEase(Ease.InQuad).OnComplete(HidePanel);
    }

    private void HidePanel()
    {
        // Show the panel with alpha tweening
        panelImage.DOFade(1f, animationDuration).SetEase(Ease.OutQuad).OnComplete(DeactivatePanel);
    }

    private void DeactivatePanel()
    {
        // Deactivate the GameObject
        gameObject.SetActive(false);
    }
}
