using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UiHud: MonoBehaviour
{
    public Text ammoText;
    public Text scoreText;
    public Text waveText;
    public GameObject GameOverUi;
    public Button restartButton;
    public Gun gun;
    public PlayerHealth playerHealth;

    public int leftEnemy;
    public int score;

    public bool isGameOver;

    public void OnEnable()
    {
        SetAmmoText(0, 0);
        SetUpdateScore(0);
        SetWaveInfo(0, 0);
        SetActiveGameOverUi(false);
    }
    //private void Start()
    //{
    //    GameOverUi.SetActive(false);
    //}
    private void Update()
    {
        if (isGameOver)
        {
            GameOverUi.SetActive(true);
            isGameOver = false;
        }
        restartButton.Invoke("ReStart", 0f);

        if (ammoText != null) ammoText.text = gun.ammoRemain + "/" + gun.magAmmo;
        if (playerHealth != null && playerHealth.IsDead && GameOverUi != null) GameOverUi.SetActive(true);
        //if (WaveText != null) WaveText.text = "Wave : " + waveNumber + "\r\nEnemy Left : " + leftEnemy;
        if (scoreText != null) scoreText.text = "SCORE : " + score;
    }

    public void SetAmmoText(int magAmmo, int remainAmmo)
    {
        if (ammoText != null)
            ammoText.text = $"{magAmmo} / {remainAmmo}";
    }

    public void SetUpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE : " + score;
        }
    }

    public void SetWaveInfo(int wave, int count)
    {
        if (waveText != null)
            waveText.text = $"Wave: {wave}\n Enemy Lef:{count}";
    }

    public void SetActiveGameOverUi(bool active)
    {
        if (GameOverUi != null)
        {
            GameOverUi.SetActive(active);
        }
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
