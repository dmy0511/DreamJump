using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbLineCtrl : MonoBehaviour
{
    GameObject player;
    float destroyDistance = 10.0f;  // player 아래쪽으로 10m

    public GameObject[] Footboards;
    [System.Serializable]
    public struct ItemWithProbability
    {
        public GameObject item;
        public float probability;
    }
    public ItemWithProbability[] ItemProbabilities; // 아이템과 확률을 담은 배열

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("cat");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;

        // 일정 거리 아래 파괴
        if (transform.position.y < playerPos.y - destroyDistance)
            Destroy(gameObject);
    }

    public void SetHideFootboards(int a_Count)
    {
        // a_Count 몇개를 보이지 않게 할 건지 개수
        List<int> active = new List<int>();
        for (int ii = 0; ii < Footboards.Length; ii++)
        {
            active.Add(ii);
        }

        for (int ii = 0; ii < a_Count; ii++)
        {
            int ran = Random.Range(0, active.Count);
            Footboards[active[ran]].SetActive(false);

            active.RemoveAt(ran);
        }

        active.Clear();

        // 아이템 스폰
        SpriteRenderer[] a_FbObj = GetComponentsInChildren<SpriteRenderer>();

        foreach (var fbObj in a_FbObj)
        {
            SpawnItem(fbObj.transform.position);
        }
    }

    void SpawnItem(Vector3 a_Pos)
    {
        GameObject selectedItem = GetRandomItem();
        if (selectedItem != null)
        {
            GameObject go = Instantiate(selectedItem);
            go.SetActive(true);
            go.transform.position = a_Pos + Vector3.up * 0.5f;
        }
    }

    GameObject GetRandomItem()
    {
        float totalProbability = 2f;
        foreach (var itemProbability in ItemProbabilities)
        {
            totalProbability += itemProbability.probability;
        }

        float randomPoint = Random.value * totalProbability;

        foreach (var itemProbability in ItemProbabilities)
        {
            if (randomPoint < itemProbability.probability)
            {
                return itemProbability.item;
            }
            else
            {
                randomPoint -= itemProbability.probability;
            }
        }
        return null;
    }
}
