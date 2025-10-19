using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    [Header("Pop Up details")]
    public int Pop_Up_Steps;
    public PopUpButtonUI[] PopUpButtons;




    float timer;
    float minDelay = 30f;
    float maxDelay = 60f;
    void Start()
    {
        PopUpButtons = GetComponentsInChildren<PopUpButtonUI>();
        timer = Random.Range(minDelay, maxDelay);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created



    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            OpenPopUpUI();
            timer = Random.Range(minDelay, maxDelay);
        }

    }

    public void OpenPopUpUI()
    {
        foreach (PopUpButtonUI button in PopUpButtons)
        {
            button.gameObject.SetActive(true);

            float randomX = Random.Range(400, 1500);
            float randomY = Random.Range(400, 700);

            button.transform.position = new Vector2(randomX, randomY);

        }

    }
}
