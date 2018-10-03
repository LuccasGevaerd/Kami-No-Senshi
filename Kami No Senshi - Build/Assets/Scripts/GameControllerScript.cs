using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour {
    public static GameObject player1, player2;
    public static float lifeP1, lifeP2;
    public float maxLifeP1, maxLifeP2;
    public float atualLifeP1, atualLifeP2;
    public Image lifebar1P1, lifebar2P1, lifebar1P2, lifebar2P2;
    public Image mpbar1P1, mpbar2P1, mpbar3P1, mpbar1P2, mpbar2P2, mpbar3P2;
    public float mpAtualP1, mpAtualP2;
    public Text mpCounterP1, mpCounterP2;
    public GameObject[] DebugObjects;
    public float cloack;

    private void Start()
    {
        lifeP1 = maxLifeP1;
        lifeP2 = maxLifeP2;
    }
    void PlayerReference1(GameObject pl)
    {
        player1 = pl;
    }
    void PlayerReference2(GameObject pl)
    {
        player2 = pl;
    }
    private void Update()
    {
        Debug();


        atualLifeP1 = lifeP1;
        atualLifeP2 = lifeP2;

        lifebar1P1.fillAmount = lifeP1 / maxLifeP1;
        lifebar1P2.fillAmount = lifeP2 / maxLifeP2;

        lifebar2P1.fillAmount = Mathf.Lerp(lifebar1P1.fillAmount, lifebar2P1.fillAmount, 0.7f);
        lifebar2P2.fillAmount = Mathf.Lerp(lifebar1P2.fillAmount, lifebar2P2.fillAmount, 0.7f);

        player1.GetComponent<Animator>().SetFloat("Special", mpAtualP1);
        player2.GetComponent<Animator>().SetFloat("Special", mpAtualP2);

        if (mpAtualP1 < 3)
        {
            mpAtualP1 += Time.deltaTime / 100;
        }
        if (mpAtualP2 < 3)
        {
            mpAtualP2 += Time.deltaTime / 100;
        }

        mpCounterP1.text = "" + (int)mpAtualP1;
        mpCounterP2.text = "" + (int)mpAtualP2;

        mpbar1P1.fillAmount = mpAtualP1; 
        if (mpAtualP1 > 1) mpbar2P1.fillAmount = (mpAtualP1 -1); else mpbar2P1.fillAmount = 0;
        if (mpAtualP1 > 2) mpbar3P1.fillAmount = (mpAtualP1 -2); else mpbar3P1.fillAmount = 0;
        mpbar1P2.fillAmount = mpAtualP2;
        if (mpAtualP2 > 1) mpbar2P2.fillAmount = (mpAtualP2 -1); else mpbar2P2.fillAmount = 0;
        if (mpAtualP2 > 2) mpbar3P2.fillAmount = (mpAtualP2 -2); else mpbar3P2.fillAmount = 0;

        if (lifeP1 <= 0) { player1.SendMessage("Death"); player2.SendMessage("Win");}
        if (lifeP2 <= 0) { player2.SendMessage("Death");player1.SendMessage("Win"); }

    }
    private void LateUpdate()
    {
        TimeImpact();
    }
    void HitP1(float dmg)
    {
        lifeP1 -= dmg;
    }
    void HitP2(float dmg)
    {
        lifeP2 -= dmg;
    }
    void MpP1(float mp)
    {
        if (mpAtualP1 < 3)
            mpAtualP1 += mp;
    }
    void MpP2(float mp)
    {
        if (mpAtualP2 < 3)
            mpAtualP2 += mp;
    }
    void SP1(int id)
    {
        if (id == 1) mpAtualP1 -= 1;
        else mpAtualP2 -= 1;
    }
    void SP2(int id)
    {
        if (id == 1) mpAtualP1 -= 2;
        else mpAtualP2 -= 2;
    }
    void SP3(int id)
    {
        if (id == 1) mpAtualP1 = 0;
        else mpAtualP2 = 0;
    }
    void TimeImpact()
    {
        if (Time.timeScale != 1)
        {
            cloack += Time.unscaledDeltaTime;
            if (cloack >= 0.05)
            {
                Time.timeScale = 1;
                cloack = 0;
            }
        }
    }
    void Debug()
    {
        GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (DebugObjects[0].activeInHierarchy)
            {
                DebugObjects[0].SetActive(false);
                DebugObjects[1].SetActive(true);
                DebugObjects[1].transform.position = DebugObjects[0].transform.position;
                Cam.SendMessage("GetTarget1", DebugObjects[1].transform);
            } else
            {
                DebugObjects[1].SetActive(false);
                DebugObjects[0].SetActive(true);
                DebugObjects[0].transform.position = DebugObjects[1].transform.position;
                Cam.SendMessage("GetTarget1", DebugObjects[0].transform);
            }
            player1.SendMessage("PlayerAgainst");
            player2.SendMessage("PlayerAgainst");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (DebugObjects[2].activeInHierarchy)
            {
                DebugObjects[2].SetActive(false);
                DebugObjects[3].SetActive(true);;
                DebugObjects[3].transform.position = DebugObjects[2].transform.position;
                Cam.SendMessage("GetTarget2", DebugObjects[3].transform);
            }
            else
            {
                DebugObjects[3].SetActive(false);
                DebugObjects[2].SetActive(true);
                DebugObjects[2].transform.position = DebugObjects[3].transform.position;
                Cam.SendMessage("GetTarget2", DebugObjects[2].transform);
            }
            player1.SendMessage("PlayerAgainst");
            player2.SendMessage("PlayerAgainst");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            mpAtualP1 = 3;
            mpAtualP2 = 3;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
