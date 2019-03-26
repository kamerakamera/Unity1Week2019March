using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    wait, playing, end, desc
}

public class GameManeger : MonoBehaviour
{
    private State state;
    [SerializeField]
    private GameObject player, mainCamera, descPanel;
    [SerializeField]
    private StageManeger stageManeger;
    [SerializeField]
    private ScoreManeger scoreManeger;
    [SerializeField]
    private GameObject[] stateWindows = new GameObject[3], descText = new GameObject[2];
    int descpage;
    string url;
    [SerializeField]
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        SetState(State.wait);
        player.gameObject.SetActive(false);
        descPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.wait)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            if (Input.GetKeyDown(KeyCode.Space))//ゲーム開始
            {
                player.gameObject.SetActive(true);
                player.GetComponent<Player>().SetStart();
                mainCamera.GetComponent<MainCamera>().SetStartPos();
                SetState(State.playing);
                scoreManeger.ResetScore();
            }
            if (Input.GetKeyDown("d"))
            {
                descpage = 0;
                descPanel.SetActive(true);
                SetState(State.desc);
                descText[descpage].SetActive(true);
                for (int i = 0; i < descText.Length; i++)
                {
                    if (i == descpage)
                    {
                        continue;
                    }
                    descText[i].SetActive(false);
                }
            }
        }
        if (state == State.end)
        {
            audioSource.Stop();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetState(State.wait);
                stageManeger.ResetStageList();
                state = State.wait;
            }
            if (Input.GetKeyDown("t"))
            {
                url = "https://twitter.com/intent/tweet?text=" + scoreManeger.SwimDistance.ToString() + "泳いで、" + scoreManeger.FlockScore.ToString() + "匹の群れを救いました！" + "&hashtags=" + "FlockConnect,unity1week";
                Application.OpenURL(url);
            }
        }
        if (state == State.desc)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                descpage++;
                if (descpage > descText.Length - 1)
                {
                    descPanel.SetActive(false);
                    SetState(State.wait);
                    return;
                }
                descText[descpage].SetActive(true);
                for (int i = 0; i < descText.Length; i++)
                {
                    if (i == descpage)
                    {
                        continue;
                    }
                    descText[i].SetActive(false);
                }
            }
        }
    }

    void SwichWindow()
    {
        if (state == State.wait)
        {
            stateWindows[0].SetActive(true);
            stateWindows[1].SetActive(false);
            stateWindows[2].SetActive(false);
        }
        if (state == State.playing)
        {
            stateWindows[0].SetActive(false);
            stateWindows[1].SetActive(true);
            stateWindows[2].SetActive(false);
        }
        if (state == State.end)
        {
            stateWindows[0].SetActive(false);
            stateWindows[1].SetActive(false);
            stateWindows[2].SetActive(true);
        }

    }

    public void SetState(State st)
    {
        state = st;
        SwichWindow();
        Debug.Log(state);
    }

    public State GetState()
    {
        return state;
    }
}
