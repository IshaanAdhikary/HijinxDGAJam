using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class CatchaSystem : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Sprite[] catSprites; // Assign at least 40 cat images
    [SerializeField] private Sprite[] dogSprites; // Assign at least 5 dog images
    
    [Header("UI References")]
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject imagePrefab; // Button with Image component
    [SerializeField] private TextMeshProUGUI instructionText;
    
    private List<Button> gridButtons = new List<Button>();
    private int currentRound = 0;
    private const int TOTAL_ROUNDS = 5;
    private const int GRID_SIZE = 9;
    private int correctImageIndex = -1;
    private HashSet<Sprite> usedSprites = new HashSet<Sprite>();

    void Start()
    {
        if (catSprites.Length < 40 || dogSprites.Length < 5)
        {
            Debug.LogError("Not enough cat or dog sprites in \"Catcha\" logic.");
        }

        SetupGrid();
        StartNewRound();
    }
    
    void SetupGrid()
    {
        // Create 3x3 grid of buttons
        for (int i = 0; i < GRID_SIZE; i++)
        {
            GameObject btnObj = Instantiate(imagePrefab, gridLayout.transform);
            Button btn = btnObj.GetComponent<Button>();
            
            int index = i; // Capture for closure
            btn.onClick.AddListener(() => OnImageClicked(index));
            
            gridButtons.Add(btn);
        }
        
        UpdateInstructionText();
    }
    
    void StartNewRound()
    {
        if (currentRound >= TOTAL_ROUNDS)
        {
            OnPassCatcha();
            return;
        }
        
        // Create list of images: 1 dog and 8 cats
        List<Sprite> roundImages = new List<Sprite>();
        
        // Add one random dog that hasn't been used
        Sprite dogSprite = GetUnusedSprite(dogSprites);
        roundImages.Add(dogSprite);
        usedSprites.Add(dogSprite);
        
        // Add 8 random cats that haven't been used
        for (int i = 0; i < GRID_SIZE - 1; i++)
        {
            Sprite catSprite = GetUnusedSprite(catSprites);
            roundImages.Add(catSprite);
            usedSprites.Add(catSprite);
        }
        
        // Shuffle the images
        roundImages = roundImages.OrderBy(x => Random.value).ToList();
        
        // Find where the dog ended up after shuffle
        correctImageIndex = roundImages.IndexOf(dogSprite);
        
        // Assign images to buttons
        for (int i = 0; i < GRID_SIZE; i++)
        {
            Image img = gridButtons[i].GetComponent<Image>();
            img.sprite = roundImages[i];
            img.preserveAspect = true;
        }
        
        UpdateInstructionText();
    }
    
    Sprite GetUnusedSprite(Sprite[] spriteArray)
    {
        // Get available sprites
        List<Sprite> availableSprites = spriteArray.Where(s => !usedSprites.Contains(s)).ToList();
        
        // If we've used all sprites, reset the used set
        if (availableSprites.Count == 0)
        {
            usedSprites.Clear();
            availableSprites = spriteArray.ToList();
        }
        
        // Return random unused sprite
        return availableSprites[Random.Range(0, availableSprites.Count)];
    }
    
    void OnImageClicked(int index)
    {
        if (index == correctImageIndex)
        {
            currentRound++;
            StartNewRound();
        }
        else
        {
            OnMistake();
        }
    }
    
    private void ResetCatcha()
    {
        currentRound = 0;
        usedSprites.Clear();
        StartNewRound();
    }
    
    private void UpdateInstructionText()
    {
        if (instructionText != null)
        {
            instructionText.text = $"Click the dog! (Round {currentRound + 1}/{TOTAL_ROUNDS})";
        }
    }
    
    private void OnPassCatcha()
    {
        Debug.Log("Catcha Passed! All 5 rounds completed successfully.");
        // TODO: Implement success logic here
    }
    
    private void OnMistake()
    {
        Debug.Log("Mistake made! Resetting Catcha.");
        ResetCatcha();
        // TODO: Implement failure logic here
    }
}