using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using extOSC;

public class Dialogue : MonoBehaviour
{

    [Header("OSC Settings")]
    public string Address = "/OurCity";
    public OSCReceiver Receiver;

    // the text objects for the bubble and the name
    public TMP_Text bubbleText;
    public TMP_Text nameText;

    private AddBuilding addBuilding;
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
    public Sprite miguelMicaelaSprite;
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

        StartCoroutine(PopupCharacter());

        // set the character image to the character's image
        // characterImage.sprite = Resources.Load<Sprite>(characterImage);
    }

    string messageFromOSC = "";


    private void ReceivedMessage(OSCMessage message)
    {
        Debug.Log($"Received OSC message: {message}");
        messageFromOSC = message.ToString();

        if (message.ToString().Contains("NEXTPROMPT"))
        {
            bubbleText.text = "Great choice! Thank you!";
        
            
            UpThePromptIndex();
        }
    }


    public void UpThePromptIndex()
    {
        // wait three seconds
        StartCoroutine(WaitThreeSecondsAndHandle());
        
    }

    private IEnumerator WaitThreeSecondsAndHandle()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);
        // Dismiss the character
        StartCoroutine(HandlePromptIndex());
    }


    // using osc button



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
        // addBuilding = FindObjectOfType<AddBuilding>();

        if (Receiver == null)
        {
            Debug.LogError("OSC Receiver is not assigned!");
            return;
        }

        Receiver.Bind(Address, ReceivedMessage);


        characterPosition = new Vector2(330, -460);


        dialogueLines = new Dictionary<int, string>
        {
            { 0, "The Repair Crew needs to fuel up! Determine where power stations go" },
            { 1, "I need to get to work, but there's no hyperrail! Please rebuild the hyperrail station" },
            { 2, "Mg: The ice cream shop is better near the cafe!<br> Mc: NOOooOO I hink it's better near the hyperrail!<br> Mg: Hey you! Where do YOU think the ice cream shop should go?" },
            { 3, "I lost a leg while hoverboarding away from Godzilla. Can you help rebuild the hospital so I can get it fixed? I hope to not walk too far from the hyperrail" },
            { 4, "Godzilla stomped on all the senior housing units. Let's fix them so we have somewhere to live! We really like to be near the library and hospital and repair shops" },
            { 5, "It's hard for me to find places to study history! I need a library! I'm always there late, and I'm *totally* not scared of the dark, so being near other buildings is a bonus!" }
        };

        characterNames = new Dictionary<int, string>
        {
            { 0, "Kelly" },
            { 1, "Reece" },
            { 2, "Twins Miguel & Micaela" },
            { 3, "Zeek" },
            { 4, "Eddie" },
             { 5, "Olya" }
        };

        characterImages = new Dictionary<int, Sprite>
        {
            { 0, kellySprite },
            { 1, reeceSprite },
            { 2, miguelMicaelaSprite},
            { 3, zeekSprite},
            { 4, eddieSprite },
             { 5, olyaSprite }
        };

        characterScores = new Dictionary<int, float>
        {
            { 0, 0.5f },
            { 1, 0.5f },
            { 2, 0.5f },
            { 3, 0.5f },
            { 4, 0.5f }
        };


    }

    // Update is called once per frame
    void Update()
    {

        // if (addBuilding == null || !addBuilding.isTutorialStarted)
        // {
        //     return;
        // }
        characterPopupAndChildren.transform.position = characterPosition;


    }

    // move character up and set position of characterPopupAndChildren to characterPosition
    public IEnumerator PopupCharacter()
    {
        Vector2 targetPosition = new Vector2(characterPosition.x, 266);
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

    // private void ReceivedMessage(OSCMessage message)
    // {
    //     Debug.Log($"Received OSC message: {message}");
    //     if (message.ToString().Contains("ENDTUTORIAL"))
    //     {
    //         StartDialogue();
    //     }
    // }


}

