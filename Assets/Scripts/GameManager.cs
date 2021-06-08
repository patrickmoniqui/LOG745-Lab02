using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ball;
    public Rigidbody ballRB;
    public GameObject ballSpeed;
    public static float ballForceMultiplier = 1f;
    public string messageWB;
    public AudioClip collisionAudio;
    public AudioSource audioSource;
    public GameObject timer;
    public float timerLimit;
    WebSocket websocket;

    // Start is called before the first frame update
    async void Start()
    {
        ballRB = ball.GetComponent<Rigidbody>();
        websocket = new WebSocket("ws://localhost:8080");
        audioSource = GetComponent<AudioSource>();

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            messageWB = message;
            Debug.Log("OnMessage! " + message);
        };

        // Keep sending messages at every 0.3s
        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif

        if (!string.IsNullOrEmpty(messageWB))
        {
            Debug.Log(messageWB);

            switch(messageWB)
            {
                case "Top": ballRB.AddForce(Vector3.forward * 2 * ballForceMultiplier);
                    break;
                case "Down": ballRB.AddForce(Vector3.back * 2 * ballForceMultiplier);
                    break;
                case "Left": ballRB.AddForce(Vector3.left * 2 * ballForceMultiplier);
                    break;
                case "Right": ballRB.AddForce(Vector3.right * 2 * ballForceMultiplier);
                    break;
                case "First": 
                    break;
                case "Stop": ballRB.velocity = Vector3.zero;
                    break;
                case "Peace": ballRB.AddForce(Vector3.up * 5);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.W)) ballRB.AddForce(Vector3.forward * 100);
        if (Input.GetKeyDown(KeyCode.S)) ballRB.AddForce(Vector3.back * 100);
        if (Input.GetKeyDown(KeyCode.A)) ballRB.AddForce(Vector3.left * 100);
        if (Input.GetKeyDown(KeyCode.D)) ballRB.AddForce(Vector3.right * 100);
        if (Input.GetKeyDown(KeyCode.Space)) ballRB.AddForce(Vector3.up * 300);

        // Ball Speed
        ballSpeed.GetComponent<Text>().text = "Speed: " + ballRB.velocity.magnitude.ToString();

        // Timer
        timer.GetComponent<Text>().text = "Timer: " + ((int)(timerLimit -= Time.deltaTime)).ToString();
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            await websocket.SendText("plain text message");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}
