using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToEnding : MonoBehaviour
{
    public void StartEnding()
    {
        SceneManager.LoadScene("EndCredits");
    }
}
