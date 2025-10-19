using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int BaseStress=0;
    public int stress;
    public Animator animator;
    void Start()
    {
        stress = BaseStress;
    }

    public void raiseStress()
    {
        stress++;
        if(stress==3)
        {
             SceneManager.LoadScene("Introduction");
        }
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("StressForNerv",stress);
    }
}
