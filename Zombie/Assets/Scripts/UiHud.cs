using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UiHud: MonoBehaviour
{
    public Button restartButton;
    public GameObject GameOverUi; 

    public bool isGameOver;

    private void Start()
    {
        GameOverUi.SetActive(false);
    }
    private void Update()
    {
        if(isGameOver)
        {
            GameOverUi.SetActive(true);
            isGameOver = false;
        }
        restartButton.Invoke("ReStart",0f);
    }
}
