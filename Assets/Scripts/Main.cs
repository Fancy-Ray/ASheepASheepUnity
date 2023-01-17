using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;

public class Main : MonoBehaviour
{
    public GameObject card;
    static bool index=true;
    public Sprite[] sprites;
    private Vector3[] coordinates=new Vector3[6];//存贮最开始创建cards选择的坐标
    private InstanceZonesRequired a = new InstanceZonesRequired(10, new Vector3[4] { new Vector3(0, 4, 0), new Vector3(-4, 4, 0), new Vector3(-2, 4, 0), new Vector3(2,4,0)});
    GameObject card0;
    // Start is called before the first frame update
    void Start()
    {
        //card0 =Instantiate(card, gameObject.transform);
        //card0.GetComponentInChildren<SpriteRenderer>().sprite=sprites[1];
        a.CreateCards(sprites, card, GameObject.Find("trough").transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(index)
            index=a.DropCards();
    }
}
