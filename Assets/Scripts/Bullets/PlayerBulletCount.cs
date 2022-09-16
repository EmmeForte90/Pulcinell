using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletCount : MonoBehaviour
{
    public List<GameObject> bulletNumber;
    //Lista dei proiettili

        public static PlayerBulletCount instance;
        //Istanza per far interagire con altri script

        [HideInInspector]
        public int bulletRemain;
        //Calcolo dei proiettili che restano

        private void Awake()
        {
            bulletRemain = bulletNumber.Count; 
            //Numero totale di proiettili       
            instance = this;
        }

#region  Proiettili al completo
        public bool bulletIsFull()
        {
            if (bulletRemain == bulletNumber.Count)
                return true;
            else
                return false;
        }
#endregion

#region  Recupera un proiettile

        public void restoreOneBullet()
        {
            if (!bulletIsFull())
            {
                bulletNumber[bulletRemain].GetComponent<Bullet_Animation>().restoreBullet();
                bulletRemain += 1;
            }
        }

#endregion

#region  Rimuove un proiettile

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

#endregion

#region  Proiettili finiti

        public void EndBullet()
        {
            while( bulletRemain !=  bulletNumber.Count)
            {
                restoreOneBullet();
            }
            
            
        }

#endregion

}
