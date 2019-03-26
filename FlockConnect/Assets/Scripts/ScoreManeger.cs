using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManeger : MonoBehaviour
{
    [SerializeField]
    private GameObject player, mainCamera;
    float startCameraPos;
    public float SwimDistance { get; private set; }
    public float FlockScore { get; private set; }
    [SerializeField]
    Text scoretext;
    // Start is called before the first frame update
    void Start()
    {
        ResetScore();
        startCameraPos = mainCamera.GetComponent<MainCamera>().posOffset;
    }

    // Update is called once per frame
    void Update()
    {
        SwimDistance = mainCamera.transform.position.x - startCameraPos;
        SetScoretext();
    }

    public void SetScoretext()
    {
        scoretext.text = "泳いだ距離: " + ((int)SwimDistance).ToString() + "m\n" + "救った群れの数: " + ((int)FlockScore).ToString() + "\n\ntキーでツイート";
    }

    public void AddFlockScore()
    {
        FlockScore++;
    }

    public void ResetScore()
    {
        SwimDistance = 0;
        FlockScore = 0;
    }
}
