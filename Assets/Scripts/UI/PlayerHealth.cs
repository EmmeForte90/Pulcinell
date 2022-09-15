using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


    public class PlayerHealth : MonoBehaviour
    {
        public List<GameObject> heartsNumber;

        public static PlayerHealth instance;


        [HideInInspector]
        public int heartsRemain;

        private void Awake()
        {
            heartsRemain = heartsNumber.Count;
            instance = this;
        }

        public bool healthIsFull()
        {
            if (heartsRemain == heartsNumber.Count)
                return true;
            else
                return false;
        }

        public void restoreOneHeart()
        {
            if (!healthIsFull())
            {
                heartsNumber[heartsRemain].GetComponent<HP_Animation>().restoreHP();
                heartsRemain += 1;
            }
        }

        public void removeOneHeart()
        {
            if (heartsRemain == 1)
                playerDeath();
            else
            {
                heartsNumber[heartsRemain - 1].GetComponent<HP_Animation>().removeHP();
                heartsRemain -= 1;
            }
        }

        public void playerDeath()
        {
            while(heartsRemain != heartsNumber.Count)
            {
                restoreOneHeart();
            }
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
            //transform.position = Data.checkpointPos;
            
        }
    }