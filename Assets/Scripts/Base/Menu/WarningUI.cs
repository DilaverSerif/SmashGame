using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    private Image warningPanel;
    private TextMeshPro warningText;

    private string content;
    private Button closeButton;

    private void Awake()
    {
        warningPanel = transform.Find("WarningPanel").GetComponent<Image>();
        warningText = warningPanel.transform.Find("Text").GetComponent<TextMeshPro>();
        closeButton = warningPanel.GetComponentInChildren<Button>();
        
        closeButton.onClick.AddListener(CloseButton);
    }

    private void OnEnable()
    {
        MenuSystem.OpenWarning.AddListener(OpenWarning);
    }

    private void OnDisable()
    {
        MenuSystem.OpenWarning.RemoveListener(OpenWarning);
    }

    private void OpenWarning(string context)
    {
        warningText.text = context;
        warningPanel.gameObject.SetActive(true);
        warningPanel.color = new Color(1, 1, 1, 0);
        warningPanel.DOFade(1, 0.2F).SetUpdate(true);
        Time.timeScale = 0;
    }

    private void CloseButton()
    {
        warningPanel.DOFade(0, 0.2F).OnComplete(
            () =>
            {
                warningPanel.gameObject.SetActive(false);
                Time.timeScale = 1;
            });
    }
}



