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
    public string messageWB;
    public AudioClip collisionAudio;
    public AudioSource audioSource;
    WebSocket websocket;

    // Start is called before the first frame update
    async void Start()
    {
        ball = GameObject.Find("Sphere");
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
                case "Top": ballRB.AddForce(Vector3.forward * 3);
                    break;
                case "Down": ballRB.AddForce(Vector3.back * 3);
                    break;
                case "Left": ballRB.AddForce(Vector3.left * 3);
                    break;
                case "Right": ballRB.AddForce(Vector3.right * 3);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) ballRB.AddForce(Vector3.forward * 100);
        if (Input.GetKeyDown(KeyCode.DownArrow)) ballRB.AddForce(Vector3.back * 100);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ballRB.AddForce(Vector3.left * 100);
        if (Input.GetKeyDown(KeyCode.RightArrow)) ballRB.AddForce(Vector3.right * 100);
        if (Input.GetKeyDown(KeyCode.Space)) ballRB.AddForce(Vector3.up * 300);

        ballSpeed.GetComponent<Text>().text = ballRB.velocity.magnitude.ToString();
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

    void OnCollisionEnter(Collision collision)
    {
        audioSource.PlayOneShot(collisionAudio, 0.7F);
    }

}
