using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject StartUIs;
    public GameObject TutoUI;
    public GameObject EndUIs;
    public Transform EnemysParent;
    public PlayerController pc;
    public AudioClip normalBGM;
    public AudioClip battleBGM;
    private bool tutoFlag = false;
    public BGMManager bgmManager;
    public EnemySponer enemyspawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartUIs.SetActive(true);
        EndUIs.SetActive(false);
        tutoFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GameStart()
    {
        StartUIs.SetActive(false);
        enemyspawner.startSpawn();
        bgmManager.ChangeBGM(battleBGM);
    }
    
    public void EndGame()
    {
        EndUIs.SetActive(true);
        bgmManager.ChangeBGM(normalBGM);
        foreach (Transform enemyTfm in EnemysParent)
        {
            Destroy(enemyTfm.gameObject);
        }
    }

    public void BackStart()
    {
        pc.Initialize();
        EndUIs.SetActive(false);
        StartUIs.SetActive(true);
    }

    public void FlipTuto()
    {
        tutoFlag ^= true;
        TutoUI.SetActive(tutoFlag);
    }
}
