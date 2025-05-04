using UnityEngine;
using extOSC;

public class OSCTesting : MonoBehaviour
{
    [Header("OSC Settings")]
    public string Address = "/ADD";
    public OSCReceiver Receiver;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 if (Receiver == null)
        {
            Debug.LogError("OSC Receiver is not assigned!");
            return;
        }


        Receiver.Bind(Address, ReceivedMessage);
    }
    private void ReceivedMessage(OSCMessage message)
    {

        // videoPlayer.Stop();

        Debug.Log(message);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
