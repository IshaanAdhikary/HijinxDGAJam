using UnityEngine;
using UnityEngine.SceneManagement;

public class Stress : MonoBehaviour
{
    private  int baseStress = 0;
    public int stress;
    public Animator animator;
    void Start()
    {
        stress = baseStress;
    }

    public void raiseStress()
    {
        stress++;
        if (stress == 3)
        {
            SceneManager.LoadScene("Introduction");
        }
    }
    
    void Update()
    {
        animator.SetInteger("StressForNerv", stress);
    }
}
