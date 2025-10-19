using UnityEngine;

public class UI : MonoBehaviour
{

    [Header("Pop Up details")]
    public int Pop_Up_Steps;
    public PopUpButtonUI[] PopUpButtons;
    
    
 
void Start()
    {
        PopUpButtons = GetComponentsInChildren<PopUpButtonUI>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

   

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
            OpenPopUpUI();

    }

    public void OpenPopUpUI()
    {
        foreach(PopUpButtonUI button in  PopUpButtons)
        {
            button.gameObject.SetActive(true);

            float randomX = Random.Range(300, 1700);
            float randomY = Random.Range(200, 900);

            button.transform.position = new Vector2(randomX, randomY);

        }

    }
}
