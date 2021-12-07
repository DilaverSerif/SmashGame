using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailMenu : MonoBehaviour
{
    [SerializeField] private Button failButton;

    private void Start()
    {
        failButton.onClick.AddListener(Fail);
    }

    private void Fail()
    {
        var a = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(a);
    }
}
