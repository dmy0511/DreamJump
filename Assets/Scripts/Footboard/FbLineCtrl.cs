using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbLineCtrl : MonoBehaviour
{
    GameObject player;
    float destroyDistance = 10.0f;

    public GameObject[] Footboards;
    [System.Serializable]
    public struct ItemWithProbability
    {
        public GameObject item;
        public float probability;
    }
    public ItemWithProbability[] ItemProbabilities; //아이템 확률 조정

    void Start()
    {
        player = GameObject.Find("cat");
    }

    void Update()
    {
        Vector3 playerPos = player.transform.position;

        if (transform.position.y < playerPos.y - destroyDistance)
            Destroy(gameObject);
    }

    public void SetHideFootboards(int a_Count)
    {
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

    public void SetItemProbabilitiesToZero()    //아이템 생성확률 0으로 변경
    {
        for (int i = 0; i < ItemProbabilities.Length; i++)
        {
            ItemProbabilities[i] = new ItemWithProbability
            {
                item = ItemProbabilities[i].item,
                probability = 0f
            };
        }
    }
}
