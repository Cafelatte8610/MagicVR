using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject StartUIs;
    public GameObject EndUIs;
    public Transform EnemysParent;
    public PlayerController pc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartUIs.SetActive(true);
        EndUIs.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EndGame()
    {
        EndUIs.SetActive(true);
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
}
