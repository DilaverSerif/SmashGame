using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelMeu : MonoBehaviour
{
    [SerializeField] private Button nextLevel;

    private void Start()
    {
        nextLevel.onClick.AddListener(Next);
    }

    private void Next()
    {
        var a = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(a + 1);
    }
}
