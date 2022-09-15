using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//Usa l'input system di Unity scaricato dal packet manager

public class PlayerWeaponManager : MonoBehaviour
{
   [Header("Proiettili")]
    [SerializeField] private GameObject Weapon_1;
    [SerializeField] private GameObject Weapon_2;
    [SerializeField] private GameObject Weapon_3;
    [SerializeField] private GameObject Weapon_4;

    [SerializeField] private float tapCount = 1;


    PlayerMovement playerShootScript;

    private void Awake()
    {
        playerShootScript = GetComponent<PlayerMovement>();
    }

void Update()
{
    


}

#region ChangeWeapon
void OnChangeWeapon(InputValue value)
{
    if(value.isPressed)
    {
        if(tapCount == 1)
        {
        SetWeapon(1);
        tapCount++;
        }
        else if(tapCount == 2)
        {
        SetWeapon(2);
        tapCount++;
        }
        else if(tapCount == 3)
        {
        SetWeapon(3);
        tapCount++;
        }
        else if(tapCount == 4)
        {
        SetWeapon(4);
        tapCount = 1;
        }
    } 
    

}
#endregion

void SetWeapon(int WeaponID)
{
    switch (WeaponID)
    {
        case 1:
    PlayerMovement.instance.SetBulletPrefab(Weapon_1);
    break;

    case 2:
    PlayerMovement.instance.SetBulletPrefab(Weapon_2);
    break;

    case 3:
    PlayerMovement.instance.SetBulletPrefab(Weapon_3);
    break;

    case 4:
    PlayerMovement.instance.SetBulletPrefab(Weapon_4);
    break;
        
    }
    
    

}
 


}
