using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine.XR;

namespace Assets.Scripts.Main
{
    public class Main : MonoBehaviour
    {
        public GameObject card;
        public static bool index { set; get; } = true;
        public Sprite[] sprites;
        private Vector3[] coordinates = new Vector3[6];//存贮最开始创建cards选择的坐标
        public static InstanceZonesRequired zone1;
        GameObject card0;
        // Start is called before the first frame update
        void Start()
        {
            zone1 = new InstanceZonesRequired(54, sprites,
                new Vector3[6] {
                    //new Vector3(0, 2, 0), new Vector3(-4, 2, 0), new Vector3(-2, 2, 0), new Vector3(2, 2, 0),new Vector3(4, 2, 0),
                    //new Vector3(0, 4, 0), new Vector3(-4, 4, 0), new Vector3(-2, 4, 0), new Vector3(2, 4, 0),new Vector3(4, 4, 0),
                    //new Vector3(0, 6, 0), new Vector3(-4, 6, 0), new Vector3(-2, 6, 0), new Vector3(2, 6, 0),new Vector3(4, 6, 0),
                   new Vector3(0, 4, 0) , new Vector3(-2, 4, 0), new Vector3(2, 4, 0),
                   new Vector3(0, 6, 0) , new Vector3(-2, 6, 0), new Vector3(2, 6, 0),
                    //new Vector3(0, 8, 0), new Vector3(-4, 8, 0), new Vector3(-2, 8, 0), new Vector3(2, 8, 0),new Vector3(4, 8, 0),

                }, new int[6] {
                    //3, 2, 2, 3, 2,
                    //4, 4, 6, 4, 4,
                    //6, 5, 4, 4, 4,
                    8, 12, 8,
                    10, 8, 8,
                    //2, 2, 2, 2, 3
                })
            {

            };
            try
            {
                zone1.CreateCards(card, GameObject.Find("trough").transform);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                index = false;
            }


        }
        // Update is called once per frame
        void Update()
        {
            if (index)
            {
                index = zone1.DropCards();
                zone1.DestroyCards();
            }
        }
    }
}