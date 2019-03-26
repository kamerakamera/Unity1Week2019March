using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManeger : MonoBehaviour
{
    [SerializeField]
    GameObject wallPrefab, burrowPrefab, maguroPrefab, ikaPrefab;
    float wallHeight = 0, createPos = 0, wallInterval = 45, maguroCreateOffset = 60, ikaCreateOffsetX = 60, createOffsetY = 6.5f, maguroCreateInterval = 1.5f, ikaCreateInterval = 1.2f;//wallのy軸の位置
    [SerializeField]
    private GameObject player;
    Vector3 enemyCreatePos;
    bool stopCor;
    [SerializeField]
    private GameManeger gameManeger;
    [SerializeField]
    private ScoreManeger scoreManeger;
    private List<GameObject> wallList = new List<GameObject>(), ikaList = new List<GameObject>(), maguroList = new List<GameObject>(), burrowList = new List<GameObject>();
    // Start is called before the first frame update
    public void Start()
    {
        SetEnemyCreateTime();
        StartCoroutine(CreateMaguroCor());
        StartCoroutine(CreateIkaCor());
        stopCor = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManeger.GetState() != State.playing)
        {
            if (stopCor == false)
            {
                StopCoroutine(CreateIkaCor());
                StopCoroutine(CreateMaguroCor());
                stopCor = true;
            }
            return;
        }
        SetEnemyCreateTime();
        if (player.transform.position.x + wallInterval >= createPos)//ここでStage生成
        {
            CreateWall();
        }
    }

    public void ResetStageList()
    {
        for (int i = 0; i < wallList.Count; i++)
        {
            Destroy(wallList[i]);
        }
        createPos = 0;
        for (int i = 0; i < ikaList.Count; i++)
        {
            Destroy(ikaList[i]);
        }
        for (int i = 0; i < maguroList.Count; i++)
        {
            Destroy(maguroList[i]);
        }
        for (int i = 0; i < burrowList.Count; i++)
        {
            Destroy(burrowList[i]);
        }
    }

    void CreateWall()
    {
        wallList.Add(Instantiate(wallPrefab, new Vector3(createPos, 0, 0), Quaternion.identity));
        if (wallList.Count >= 4)
        {
            Destroy(wallList[0]);
            wallList.RemoveAt(0);
        }
        if (Random.Range(0.0f, (10.0f + scoreManeger.SwimDistance) / wallInterval) <= 5)
        {
            CreateBurrow();
        }
        createPos += wallInterval;
    }

    void CreateBurrow()
    {
        burrowList.Add(Instantiate(burrowPrefab, new Vector3(createPos + Random.Range(-20.0f, 20.0f), (Random.Range(0.0f, 1.0f) < 0.5f) ? -6.7f : 6.7f, 0), Quaternion.identity));
        if (burrowList[burrowList.Count - 1].transform.position.y > 0)
        {
            burrowList[burrowList.Count - 1].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        if (burrowList.Count >= 4)
        {
            Destroy(burrowList[0]);
            burrowList.RemoveAt(0);
        }
    }

    void CreateMaguro()
    {
        if (gameManeger.GetState() != State.playing)
        {
            return;
        }
        enemyCreatePos = new Vector3(maguroCreateOffset + player.transform.position.x, Random.Range(-createOffsetY, createOffsetY), 0);
        maguroList.Add(Instantiate(maguroPrefab, enemyCreatePos, Quaternion.identity));
        if (maguroList.Count >= 9)
        {
            Destroy(maguroList[0]);
            maguroList.RemoveAt(0);
        }
    }

    void CreateIka()
    {
        if (gameManeger.GetState() != State.playing)
        {
            return;
        }
        enemyCreatePos = new Vector3(ikaCreateOffsetX + player.transform.position.x, Random.Range(-createOffsetY, createOffsetY), 0);
        ikaList.Add(Instantiate(ikaPrefab, enemyCreatePos, Quaternion.identity));
        ikaList[ikaList.Count - 1].transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector3.Angle(ikaList[ikaList.Count - 1].transform.up, player.transform.position - ikaList[ikaList.Count - 1].transform.position)));
        if (ikaList.Count >= 9)
        {
            Destroy(ikaList[0]);
            ikaList.RemoveAt(0);
        }
    }

    void SetEnemyCreateTime()
    {

        if (scoreManeger.FlockScore <= 20)
        {
            maguroCreateInterval = 1.5f;
            ikaCreateInterval = 1.5f;
            if (scoreManeger.FlockScore <= 10)
            {
                maguroCreateInterval = 2.0f;
                ikaCreateInterval = 2.5f;
            }
        }
    }
    private IEnumerator CreateMaguroCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(maguroCreateInterval);
            CreateMaguro();
        }
    }

    private IEnumerator CreateIkaCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(ikaCreateInterval);
            CreateIka();
        }
    }
}
