using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private HashSet<Combustible> logs = new HashSet<Combustible>();
    private SunController sun;
    private bool isGameStarted = false;
    private bool isGameOver = false;
    private AudioClip monsterSound;
    private AudioSource audioSource;
    private bool isGameEnded = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        monsterSound = Resources.Load<AudioClip>("monster");
        sun = GameObject.Find("Sun").GetComponent<SunController>();
    }

    public void RegisterNewLog(Combustible fire) {
        logs.Add(fire);
    }
    
    public void StartGame() {
        isGameStarted = true;
    }
    public void EndGame() {
        isGameEnded = true;
    }
    void Update()
    {
        if (isGameStarted && !isGameOver && !isGameEnded && sun.IsNightTime() && !logs.Select(log => log.isLit).Any(isLit => isLit)) {
            // if no logs are lit
            StartCoroutine(BeginGameOver());
        }
    }

    IEnumerator BeginGameOver() {
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
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        var blackScreen = GameObject.Find("End Black").GetComponent<Image>();
        blackScreen.color = Color.black;
        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(Resources.Load<AudioClip>("timpani_1"));
        blackScreen.transform.Find("DARKNESS").GetComponent<TMPro.TMP_Text>().color = Color.white;
        yield return new WaitForSeconds(2.5f);
        audioSource.PlayOneShot(Resources.Load<AudioClip>("timpani_2"));
        blackScreen.transform.Find("IS").GetComponent<TMPro.TMP_Text>().color = Color.white;
        yield return new WaitForSeconds(2.5f);
        audioSource.PlayOneShot(Resources.Load<AudioClip>("timpani_3"));
        blackScreen.transform.Find("DEATH").GetComponent<TMPro.TMP_Text>().color = Color.white;
        yield return new WaitForSeconds(7);
        // Restart the game but without the intro music
        GameManager.quickStart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
