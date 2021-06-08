using UnityEngine;
using UnityEngine.UI;

public class SphereCollision : MonoBehaviour
{
    public AudioSource audioSource;
    private int CurrentScore = 0;
    public GameObject score;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        // Scorwe
        score.GetComponent<Text>().text = "Score: " + CurrentScore.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Sphere collision");
        audioSource.PlayOneShot(audioSource.clip, Music.effectVolume);

        if (collision.transform.gameObject.name == "Hit")
        {
            CurrentScore += 1;
            Destroy(collision.transform.gameObject);
            GameManager.ballForceMultiplier += 0.25f;
        }
    }
}
