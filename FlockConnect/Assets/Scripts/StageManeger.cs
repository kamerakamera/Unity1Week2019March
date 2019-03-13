using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManeger : MonoBehaviour
{
    [SerializeField]
    GameObject wallPrefab, burrowPrefab, maguroPrefab, ikaPrefab;
    float wallHeight = 0, createPos = 0, wallInterval = 45;//wallのy軸の位置
    [SerializeField]
    private GameObject player;
    private List<GameObject> wallList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x + wallInterval >= createPos)//ここでStage生成
        {
            CreateWall();
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
        createPos += wallInterval;
    }
}
