using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    public UiHud uiHud;
    private GameObject player;
    private GameObject enemy;
    private int score;
    public ZombieSpawner zombieSpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("enemy");
        player = GameObject.FindGameObjectWithTag("player");
    }
    void Start()
    {
        var findGo = GameObject.FindWithTag("Player");
        var playerHealth = findGo.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int add)
    {
        score += add;
        uiHud.SetUpdateScore(score);
    }

    private void ReStart()
    {
        uiHud.GameOverUi.SetActive(false);
        //player.healthSlider.gameObject.SetActive(false);
        //player.animator.SetTrigger(IdleHash);

        //movement.enabled = true;
        //shooter.enabled = true;
    }

    private void EndGame()
    {
        zombieSpawner.enabled = false;
    }
}
