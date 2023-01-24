using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

namespace Assets.Scripts
{
    public class InstanceZonesRequired
    {
        public int cardNumber { get; set; }
        Vector3[] vector3s { get; set; }
        public Vector3[] vector3sForTrough { get; set; } = { new Vector3(-3, 0, 0), new Vector3(-2, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0), new Vector3(3, 0, 0) };
        Sprite[] sprites;
        int[] quantitiesPerCoordinate { get; set; }
        // vector3&quantitiesPerCoordinate use the same index.
        public GameObject[] cards { get; set; }
        public Dictionary<GameObject, Vector3> correspond = new Dictionary<GameObject, Vector3>();
        //Parameter:
        //  sprites:
        //      Mark cards with different kinds of sprites.
        public InstanceZonesRequired(int cardNumber, Sprite[] sprites, Vector3[] vector3s, int[] quantitiesPerCoordinate)
        {
            this.cardNumber = cardNumber;
            this.cards = new GameObject[cardNumber];
            this.vector3s = vector3s;
            this.quantitiesPerCoordinate = quantitiesPerCoordinate;
            this.sprites = sprites;
        }
        //
        //Brief:
        //  Create the cards that we need.
        //Parameter:
        //  card:
        //      An existing object that you want to make a copy of.
        //  parent:
        //      The parent of cards.
        //
        public void CreateCards(GameObject card, Transform parent)
        {
            //Check for errors
            if (vector3s.Length != quantitiesPerCoordinate.Length)
                throw new System.Exception("The lengh of " + nameof(vector3s) + " is not equal to " + nameof(quantitiesPerCoordinate));
            int quantitiesOfCoordinate = 0;
            for (int index = 0; index < quantitiesPerCoordinate.Length; index++)
                quantitiesOfCoordinate += quantitiesPerCoordinate[index];
            if (quantitiesOfCoordinate != this.cardNumber)
                throw new System.Exception(nameof(cardNumber) + " not equal to quantities of coordinates");
            if (this.cardNumber % 3 != 0)
                throw new System.Exception(nameof(cardNumber) + "is not a multiple of 3");
            //Create number of cardNumbers cards
            Vector3 vector3 = new Vector3();
            int[] quantitiesPerCoordinateCheckUp = new int[quantitiesPerCoordinate.Length];
            int randomNumber;
            for (int index = 0; index < cardNumber; index++)
            {
                while (true)
                {
                    randomNumber = Random.Range(0, vector3s.Length);
                    if (quantitiesPerCoordinate[randomNumber] > quantitiesPerCoordinateCheckUp[randomNumber])
                    {
                        quantitiesPerCoordinateCheckUp[randomNumber] += 1;
                        break;
                    }
                }
                vector3 = vector3s[randomNumber];
                cards[index] = Object.Instantiate(card, new Vector3(vector3.x + GameObject.Find("trough").transform.position.x, 10 + GameObject.Find("trough").transform.position.y, GameObject.Find("trough").transform.position.z), Quaternion.identity, parent);
                correspond.Add(cards[index], vector3);
            }
            Debug.Log(vector3s.Length);
            //Randomly give cards sprites
            for (int index = 0; index < cardNumber; index += 3)
            {
                randomNumber = Random.Range(0, sprites.Length);
                cards[index].GetComponentInChildren<SpriteRenderer>().sprite = sprites[randomNumber];
                cards[index + 1].GetComponentInChildren<SpriteRenderer>().sprite = sprites[randomNumber];
                cards[index + 2].GetComponentInChildren<SpriteRenderer>().sprite = sprites[randomNumber];

            }
            Debug.Log(sprites.Length);
            //Randomly determine the order
            Vector3 vector3Index;
            Vector3 vector3Index_;
            for (int index = 0; index < cardNumber; index++)
            {
                for (int index_ = 0; index_ < index; index_++)
                {
                    correspond.TryGetValue(cards[index], out vector3Index);
                    correspond.TryGetValue(cards[index_], out vector3Index_);
                    if (vector3Index_.x == vector3Index.x && vector3Index_.y == vector3Index.y && vector3Index_.z == vector3Index.z)
                    {
                        if (vector3Index.y % 1 == 0)
                            vector3Index.y += 0.6f;
                        vector3Index.z += 1;
                        correspond[cards[index]] = vector3Index;
                        index--;
                        break;
                    }
                }
            }
        }

        //
        //Brief:
        //  Method to realize card dropping,used in the update() method
        //Return:
        //  Return true means the process is still going on.
        //  Return false means the process has ended.
        public bool DropCards()
        {
            bool returnIndex = false;
            float step = 5 * Time.deltaTime;
            Vector3 vector3 = new Vector3();
            for (int index = 0; index < cardNumber; index++)
            {
                try
                {
                    correspond.TryGetValue(cards[index], out vector3);
                    cards[index].transform.localPosition = Vector3.MoveTowards(cards[index].transform.localPosition, vector3, step);
                    if (cards[index].transform.localPosition != vector3)
                        returnIndex = true;
                }
                catch { }
            }
            return returnIndex;
        }

        //
        //Brief:
        //  Check whether there are 3 duplicate cards in the trough,and destroy them if there are any.
        public void DestroyCards()
        {
            //use a array to storage the gameobject who in the trough.
            GameObject[] gameObjectsInTrough = new GameObject[vector3sForTrough.Length];
            for (int index = 0; index < vector3sForTrough.Length; index++)
            {
                for (int index_ = 0; index_ < cards.Length; index_++)
                {
                    try
                    {
                        Vector3 vector3 = cards[index_].GetComponent<Transform>().position;
                        vector3.x -= GameObject.Find("trough").GetComponent<Transform>().position.x;
                        vector3.y -= GameObject.Find("trough").GetComponent<Transform>().position.y;
                        vector3.z -= GameObject.Find("trough").GetComponent<Transform>().position.z;
                        //
                        if (vector3.x == vector3sForTrough[index].x && vector3.y == vector3sForTrough[index].y && vector3.z == vector3sForTrough[index].z)
                        {
                        gameObjectsInTrough[index] = cards[index_];
                        }
                    }
                    catch { }
                }
            }
            //
            for (int index_ = 0; index_ < sprites.Length; index_++)
            {
                GameObject[] threeDuplicateObject = new GameObject[3];
                int objectNumber = 0;//Up to 3
                for (int index = 0; index < vector3sForTrough.Length; index++)
                {
                    try
                    {
                        if (gameObjectsInTrough[index].GetComponentInChildren<SpriteRenderer>().sprite == sprites[index_]) {
                            threeDuplicateObject[objectNumber]= gameObjectsInTrough[index];
                            objectNumber++;
                        }

                        //GameObject.Find("trough").GetComponent<SpriteRenderer>().sprite = gameObjectsInTrough[index].GetComponentInChildren<SpriteRenderer>().sprite;
                    }
                    catch { }
                }
                if (objectNumber == 3)
                {
                    correspond[threeDuplicateObject[0]] = new Vector3(100,100,100);
                    correspond[threeDuplicateObject[1]] = new Vector3(100, 100, 100);
                    correspond[threeDuplicateObject[2]] = new Vector3(100, 100, 100);
                    GameObject.Destroy(threeDuplicateObject[0]);
                    GameObject.Destroy(threeDuplicateObject[1]);
                    GameObject.Destroy(threeDuplicateObject[2]);
                    break;
                }
            }
        }
    }
}
