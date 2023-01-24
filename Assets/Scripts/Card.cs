using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Assets.Scripts;
using UnityEngine;
using Assets.Scripts.Main;
using System;
using System.Reflection;

public class Card : MonoBehaviour
{
    public GameObject parent;
    private void OnMouseDown()
    {
        if (Main.index == false)
        {   
            for (int index = 0; index < 7; index++)
            {
                bool coordinatesAreRepeated = false;
                for (int index_ = 0; index_ < Main.zone1.cardNumber; index_++)
                {
                    try
                    {
                        if (Main.zone1.correspond[Main.zone1.cards[index_]].x == Main.zone1.vector3sForTrough[index].x && Main.zone1.correspond[Main.zone1.cards[index_]].y == Main.zone1.vector3sForTrough[index].y && Main.zone1.correspond[Main.zone1.cards[index_]].z == Main.zone1.vector3sForTrough[index].z)
                            coordinatesAreRepeated = true;
                        //if (Main.zone1.cards[index_] == null)
                        //    coordinatesAreRepeated = false;
                    }
                    catch { }
                }
                if (coordinatesAreRepeated == false)
                {
                    Main.index = true;
                    try
                    {
                        Main.zone1.correspond[parent] = Main.zone1.vector3sForTrough[index];
                    }
                    catch { }
                    break;
                }
            }
        }

    }
    private void OnMouseOver()
    {
        this.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    }
    private void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0f);
    }
}
