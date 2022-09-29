using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//Usa l'input system di Unity scaricato dal packet manager


public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager instance;
   [Header("Proiettili")]
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject rapid;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject bomb;

    [SerializeField] public float tapCount;

    PlayerMovement playerShootScript;

    private void Awake()
    {
        playerShootScript = GetComponent<PlayerMovement>();
        instance = this;
        tapCount = 1;
    }


#region ChangeWeapon
void OnChangeWeapon(InputValue value)
{
    if(value.isPressed)
    {
        if(tapCount == 1)
        {
        //normal
        SetWeapon(1);
        tapCount++;
        FindObjectOfType<ChangeWeaponAnimation>().ChangeWeapon(1);
        FindObjectOfType<PlayerMovement>().ChangeWeaponSkin(1);

        }
        else if(tapCount == 2)
        {
        //rapid
        SetWeapon(2);
        tapCount++;
        FindObjectOfType<ChangeWeaponAnimation>().ChangeWeapon(2);
        FindObjectOfType<PlayerMovement>().ChangeWeaponSkin(2);
        
        }
        else if(tapCount == 3)
        {
        //shotgun
        SetWeapon(3);
        tapCount++;
        FindObjectOfType<ChangeWeaponAnimation>().ChangeWeapon(3);
        FindObjectOfType<PlayerMovement>().ChangeWeaponSkin(3);
        
        }
        else if(tapCount == 4)
        {
        //bomb
        SetWeapon(4);
        tapCount = 1;
        FindObjectOfType<ChangeWeaponAnimation>().ChangeWeapon(4);
        FindObjectOfType<PlayerMovement>().ChangeWeaponSkin(4);
        
        }
    } 
    

}
#endregion

public void SetWeapon(int WeaponID)
{
    switch (WeaponID)
    {
        case 1:
    PlayerMovement.instance.SetBulletPrefab(normal);
    break;

    case 2:
    PlayerMovement.instance.SetBulletPrefab(rapid);
    break;

    case 3:
    PlayerMovement.instance.SetBulletPrefab(shotgun);
    break;

    case 4:
    PlayerMovement.instance.SetBulletPrefab(bomb);
    break;
        
    }
    

 }
}
