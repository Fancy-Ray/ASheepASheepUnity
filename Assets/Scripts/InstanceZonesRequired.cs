using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Assets.Scripts 
{
    class InstanceZonesRequired 
    {
        int cardNumber { get; set; }
        Vector3[] vector3s { get; set; }
        GameObject[] cards;
        Dictionary<GameObject, Vector3> correspond = new Dictionary<GameObject, Vector3>();
        public InstanceZonesRequired(int cardNumber, Vector3[] vector3s) 
        {
            this.cardNumber = cardNumber;
            this.vector3s = vector3s;
            this.cards=new GameObject[cardNumber];
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
        public void CreateCards(Sprite[] sprites, GameObject card, Transform parent ) 
        {
            //Create number of cardNumbers cards
            Vector3 vector3 = new Vector3();
            for (int index = 0; index < cardNumber; index++)
            {
                vector3 = vector3s[Random.Range(0, vector3s.Length)];
                cards[index] = Object.Instantiate(card, new Vector3(vector3.x+ GameObject.Find("trough").transform.position.x, 10+GameObject.Find("trough").transform.position.y, GameObject.Find("trough").transform.position.z) , Quaternion.identity, parent);
                correspond.Add(cards[index], vector3);
            }
            Debug.Log(vector3s.Length);
            //Randomly give cards sprites
            for (int index = 0; index < cardNumber; index++)
                cards[index].GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0,sprites.Length)];
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
                        vector3Index.y += 0.5f;
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
