using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    public UiHud uiHud;
    private GameObject player;
    private GameObject enemy;

    int enemyCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("enemy");
        player = GameObject.FindGameObjectWithTag("player");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ReStart()
    {
        uiHud.GameOverUi.SetActive(false);
        //player.healthSlider.gameObject.SetActive(false);
        //player.animator.SetTrigger(IdleHash);

        //movement.enabled = true;
        //shooter.enabled = true;
    }
}
