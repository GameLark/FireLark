using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private HashSet<Fire> logs = new HashSet<Fire>();
    private SunController sun;
    private bool isGameStarted = false;
    private bool isGameOver = false;
    private AudioClip monsterSound;
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        monsterSound = Resources.Load<AudioClip>("monster");
        sun = GameObject.Find("Sun").GetComponent<SunController>();
    }

    public void RegisterNewLog(Fire fire) {
        logs.Add(fire);
    }
    
    public void StartGame() {
        isGameStarted = true;
    }
    void Update()
    {
        if (isGameStarted && !isGameOver && sun.IsNightTime() && !logs.Select(log => log.isLit).Any(isLit => isLit)) {
            // if no logs are lit
            BeginGameOver();
        }
    }

    async void BeginGameOver() {
        Debug.Log("Game Over");
        isGameOver = true;
        
        // extinguish all the logs?
        foreach (var log in logs)
        {
            log.Init(293.15f);
        }
        // extinguish fire flies
        GameObject.Find("Fireflies").SetActive(false);

        audioSource.PlayOneShot(monsterSound);
        await Task.Delay((int)(monsterSound.length * 1000));
        var blackScreen = GameObject.Find("End Black").GetComponent<Image>();
        blackScreen.color = Color.black;
        await Task.Delay(2000);
        audioSource.PlayOneShot(Resources.Load<AudioClip>("timpani_1"));
        blackScreen.transform.Find("DARKNESS").GetComponent<TMPro.TMP_Text>().color = Color.white;
        await Task.Delay(2500);
        audioSource.PlayOneShot(Resources.Load<AudioClip>("timpani_2"));
        blackScreen.transform.Find("IS").GetComponent<TMPro.TMP_Text>().color = Color.white;
        await Task.Delay(2500);
        audioSource.PlayOneShot(Resources.Load<AudioClip>("timpani_3"));
        blackScreen.transform.Find("DEATH").GetComponent<TMPro.TMP_Text>().color = Color.white;
        await Task.Delay(7000);
        // Restart the game but without the intro music
        GameManager.quickStart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
