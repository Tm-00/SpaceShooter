using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public static int scoreCount;

    public Text hiScoreText;
    public static int hiScoreCount;

    public Text ammoText;
    public static int ammoCount;

    public Text maxAmmoText;
    public static int maxAmmoCount;

    public Text healthText;
    public static int healthCount;


    // Start is called before the first frame update
    void Awake()
    {
        scoreText = transform.Find("Score").GetComponent<Text>();
        if (scoreText == null)
        {
            Debug.LogError("Text component not found on the object or its children.");
        }
        scoreCount = 0;

        hiScoreText = transform.Find("HiScore").GetComponent<Text>();
        if (hiScoreText == null)
        {
            Debug.LogError("Text component not found on the object or its children.");
        }

        ammoText = transform.Find("Ammo").GetComponent<Text>();
        if (ammoText == null)
        {
            Debug.LogError("Text component not found on the object or its children.");
        }

        maxAmmoText = transform.Find("MaxAmmo").GetComponent<Text>();
        if (hiScoreText == null)
        {
            Debug.LogError("Text component not found on the object or its children.");
        }

        healthText = transform.Find("Health").GetComponent<Text>();
        if (hiScoreText == null)
        {
            Debug.LogError("Text component not found on the object or its children.");
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            hiScoreCount = PlayerPrefs.GetInt("HighScore");
        }
    }

    // Update is called once per frame
    void Update()
     {
        if(scoreCount > hiScoreCount)
        {
            hiScoreCount = scoreCount;
            PlayerPrefs.SetInt("HighScore", hiScoreCount);
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + scoreCount;
            hiScoreText.text = "Hi-Score: " + hiScoreCount;
        }

        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + ammoCount.ToString();
        }

        if (maxAmmoText != null)
        {
            maxAmmoText.text = "/ " + maxAmmoCount.ToString();
        }

        if (healthText != null)
        {
            healthText.text = "health: " + healthCount.ToString();
        }

        gameFinished();
    }

    private void gameFinished()
    {
        if(scoreCount == 50)
        {
            FindObjectOfType<Manager>().CompleteLevel(); 
        }
    }
}
