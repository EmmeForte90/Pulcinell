using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletCount : MonoBehaviour
{
    public List<GameObject> bulletNumber;

        public static PlayerBulletCount instance;

        [HideInInspector]
        public int bulletRemain;

        private void Awake()
        {
            bulletRemain = bulletNumber.Count;        
            instance = this;

        }

        public bool bulletIsFull()
        {
            if (bulletRemain == bulletNumber.Count)
                return true;
            else
                return false;
        }

        public void restoreOneBullet()
        {
            if (!bulletIsFull())
            {
                bulletNumber[bulletRemain].GetComponent<Bullet_Animation>().restoreBullet();
                bulletRemain += 1;
            }
        }

        public void removeOneBullet()
        {
            if (bulletRemain == 1)
                EndBullet();
            else
            {
                bulletNumber[bulletRemain - 1].GetComponent<Bullet_Animation>().removeBullet();
                 bulletRemain -= 1;
            }
        }

        public void EndBullet()
        {
            while( bulletRemain !=  bulletNumber.Count)
            {
                restoreOneBullet();
            }
            
            
        }
}
