using UnityEngine;

public class StressMeter : MonoBehaviour
{
    [SerializeField] int maxStress = 5;
    RectTransform rectTransform;
    int currentStress; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        currentStress = 1;
        UpdateStressMeter();
    }

    public void IncreaseStress(int amount)
    {
        currentStress += amount;
        UpdateStressMeter();
    }
    
    private void UpdateStressMeter()
    {
        rectTransform.localScale = new Vector3(50 * currentStress, 50, 1);
        rectTransform.localPosition = new Vector3(50 + 25 * currentStress, 45, 0);
    }
}
