using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{

    // the text objects for the bubble and the name
    public TMP_Text bubbleText;
    public TMP_Text nameText;

    // public GameObject bubbleBack;
    // public float bubbleHeight;

    // the body text
    public string lines;

    public GameObject characterImageObject;

    // character's name
    public string characterName;


    // the actual ui element for the character and the bubble and the name
    public GameObject characterPopupAndChildren;

    // the position of the character and bubbles
    public Vector2 characterPosition;

    private int index = 0;

    public int indexOfCurrentPrompt = 0;

    // which illustration to use
    public Sprite characterImage;

    // store how well the player followed the prompt
    public float playerScore;

    // sprites for each character
    public Sprite kellySprite;
    public Sprite reeceSprite;
    public Sprite olyaSprite;
    public Sprite zeekSprite;
    public Sprite eddieSprite;


    // public DialogueData(string charName, string charImage, string[] dialogueLines, float score)
    // {
    //     characterName = charName;
    //     characterImage = charImage;
    //     lines = dialogueLines;
    //     playerScore = score;
    // }

    // dict storing all the dialogue lines in order
    public Dictionary<int, string> dialogueLines;



    // dict storing all the names in order
    public Dictionary<int, string> characterNames;

    // dict storing all the character sprites in order
    public Dictionary<int, Sprite> characterImages;

    // dict storing all the character scores in order
    public Dictionary<int, float> characterScores;

    // dict storing all the bubble heights in order
    public Dictionary<int, float> bubbleHeights;



    // function to start a dialogue/round
    public void StartDialogue()
    {
        // set the name text to the character's name
        nameText.text = characterNames[indexOfCurrentPrompt];

        // set the bubble text to the first line of dialogue
        bubbleText.text = dialogueLines[indexOfCurrentPrompt];

        characterImageObject.GetComponent<Image>().sprite = characterImages[indexOfCurrentPrompt];

        // set the character image to the character's image
        // characterImage.sprite = Resources.Load<Sprite>(characterImage);
    }


    public void UpThePromptIndex()
    {
        StartCoroutine(HandlePromptIndex());
    }

    private IEnumerator HandlePromptIndex()
    {
        // Wait for the character to dismiss
        yield return StartCoroutine(DismissCharacter());

        // Increment the prompt index
        indexOfCurrentPrompt += 1;

        yield return new WaitForSeconds(2f);

        // Start the next dialogue
        StartDialogue();

        // Wait for the character to pop up
        yield return StartCoroutine(PopupCharacter());
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterPosition = new Vector2(300, -460);


        dialogueLines = new Dictionary<int, string>
        {
            { 0, "The Repair Crew needs to fuel up! Determine where power stations go" },
            { 1, "I need to get to work, but there's no hyperrail! Please rebuild the hyperrail" },
            { 2, "My robodog ran away from home. It loves to hang around trees! Place a park so we can see where it ran off to" },
            { 3, "I lost a leg while hoverboarding away from Godzilla. Can you help rebuild the clinics so I can get it fixed? I hope to not walk too far from the hyperrail" },
            { 4, "Godzilla stomped on all the senior housing units. Let's fix them so we have somewhere to live! We really like to be near the library and hospital and repair shops)" }
        };

        characterNames = new Dictionary<int, string>
        {
            { 0, "Kelly" },
            { 1, "Reece" },
            { 2, "Olya" },
            { 3, "Zeek" },
            { 4, "Eddie" }
        };

        characterImages = new Dictionary<int, Sprite>
        {
            { 0, kellySprite },
            { 1, reeceSprite },
            { 2, olyaSprite},
            { 3, zeekSprite},
            { 4, eddieSprite }
        };

        characterScores = new Dictionary<int, float>
        {
            { 0, 0.5f },
            { 1, 0.5f },
            { 2, 0.5f },
            { 3, 0.5f },
            { 4, 0.5f }
        };

        StartDialogue();
        StartCoroutine(PopupCharacter());
    }

    // Update is called once per frame
    void Update()
    {

        characterPopupAndChildren.transform.position = characterPosition;

    }

    // move character up and set position of characterPopupAndChildren to characterPosition
    public IEnumerator PopupCharacter()
    {
        Vector2 targetPosition = new Vector2(characterPosition.x, 330);
        float speed = 1600f; // Adjust the speed as needed

        while (characterPosition != targetPosition)
        {
            characterPosition = Vector2.MoveTowards(characterPosition, targetPosition, speed * Time.deltaTime);
            speed = Mathf.Max(speed - 40f, 50f);
            yield return null;
        }
        characterPosition = targetPosition;
    }

    // dismiss the character
    public IEnumerator DismissCharacter()
    {
        Vector2 targetPosition = new Vector2(characterPosition.x, -460);
        float speed = 600f; // Adjust the speed as needed

        while (characterPosition != targetPosition)
        {
            characterPosition = Vector2.MoveTowards(characterPosition, targetPosition, speed * Time.deltaTime);
            speed = speed * 1.1f;
            yield return null;
        }
        characterPosition = targetPosition;
    }


}

