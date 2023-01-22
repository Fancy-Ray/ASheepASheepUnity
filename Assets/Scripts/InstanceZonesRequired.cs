using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Assets.Scripts 
{
    public class InstanceZonesRequired 
    {
        int cardNumber { get; set; }
        Vector3[] vector3s { get; set; }
        int[] quantitiesPerCoordinate { get; set; }
        // vector3&quantitiesPerCoordinate use the same index.
        GameObject[] cards { get; set; }
        Dictionary<GameObject, Vector3> correspond = new Dictionary<GameObject, Vector3>();
        public InstanceZonesRequired(int cardNumber, Vector3[] vector3s, int[] quantitiesPerCoordinate) 
        {
            this.cardNumber = cardNumber;
            this.cards = new GameObject[cardNumber];
            this.vector3s = vector3s;
            this.quantitiesPerCoordinate = quantitiesPerCoordinate;
        }
        //
        //Brief:
        //  Create the cards that we need.
        //Parameter:
        //  sprites:
        //      Mark cards with different kinds of sprites.
        //  card:
        //      An existing object that you want to make a copy of.
        //  parent:
        //      The parent of cards.
        //
        public void CreateCards(Sprite[] sprites, GameObject card, Transform parent)
        {
            //Check for errors
            if (vector3s.Length != quantitiesPerCoordinate.Length)
                throw new System.Exception("The lengh of "+ nameof(vector3s)+" is not equal to "+ nameof(quantitiesPerCoordinate));
            int quantitiesOfCoordinate = 0;
            for (int index = 0; index < quantitiesPerCoordinate.Length; index++)
                quantitiesOfCoordinate += quantitiesPerCoordinate[index];
            if (quantitiesOfCoordinate != this.cardNumber)
                throw new System.Exception(nameof(cardNumber)+" not equal to quantities of coordinates");
            if (this.cardNumber % 3 != 0)
                throw new System.Exception(nameof(cardNumber)+"is not a multiple of 3");
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
                cards[index] = Object.Instantiate(card, new Vector3(vector3.x+ GameObject.Find("trough").transform.position.x, 10+GameObject.Find("trough").transform.position.y, GameObject.Find("trough").transform.position.z) , Quaternion.identity, parent);
                correspond.Add(cards[index], vector3);
            }
            Debug.Log(vector3s.Length);
            //Randomly give cards sprites
            for (int index = 0; index < cardNumber; index += 3)
            {
                randomNumber = Random.Range(0, sprites.Length);
                cards[index].GetComponentInChildren<SpriteRenderer>().sprite = sprites[randomNumber];
                cards[index+1].GetComponentInChildren<SpriteRenderer>().sprite = sprites[randomNumber];
                cards[index+2].GetComponentInChildren<SpriteRenderer>().sprite = sprites[randomNumber];

            }
            Debug.Log(sprites.Length);
            //Randomly determine the order
            Vector3 vector3Index;
            Vector3 vector3Index_;
            for (int index = 0; index < cardNumber; index++) {
                for (int index_ = 0; index_ < index; index_++)
                {
                    correspond.TryGetValue(cards[index], out vector3Index);
                    correspond.TryGetValue(cards[index_], out vector3Index_);
                    if (vector3Index_ .x== vector3Index.x&& vector3Index_ .y== vector3Index.y&& vector3Index_.z == vector3Index.z)
                    {
                        if(vector3Index.y%1==0)
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
        public bool DropCards() {
            bool returnIndex = false;
            float step = 5 * Time.deltaTime;
            Vector3 vector3 = new Vector3();
            for (int index = 0; index < cardNumber; index++)
            {
                correspond.TryGetValue(cards[index], out vector3);
                cards[index].transform.localPosition = Vector3.MoveTowards(cards[index].transform.localPosition, vector3 , step);
                if (cards[index].transform.localPosition != vector3)
                    returnIndex = true;
            }
            return returnIndex;
        }
    }
}
