using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;
using System.Linq.Expressions;

public class Main : MonoBehaviour
{
    public GameObject card;
    static bool index { set; get; } = true;
    public Sprite[] sprites;
    private Vector3[] coordinates=new Vector3[6];//存贮最开始创建cards选择的坐标
    public InstanceZonesRequired zone1 = new InstanceZonesRequired(9, new Vector3[4] { new Vector3(0, 4, 0), new Vector3(-4, 4, 0), new Vector3(-2, 4, 0), new Vector3(2,4,0)},new int[4] { 3,2,2,2});
    GameObject card0;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            zone1.CreateCards(sprites, card, GameObject.Find("trough").transform);
        }
        catch (Exception e) { 
        Debug.LogException(e);
            index= false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(index)
            index= zone1.DropCards();
    }
}
