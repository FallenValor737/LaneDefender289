using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public GameManager Manager;
    public PlayerInput Actions;
    public InputAction Shoot;
    public InputAction Move;
    public int SelectedLine;
    public GameObject[] Lines;
    public bool NoManualShoot;
    private InputAction.CallbackContext CachedContext;
    public AudioSource ShootAudio;
    public GameObject Barrel;
    public GameObject BarrelSmoke;



    private void Start()
    {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, Lines[0].transform.position.y, this.gameObject.transform.position.z);
        Shoot = Actions.currentActionMap.FindAction("Shoot");
        Shoot.started += Handle_ShootActionStart;
        Shoot.canceled += Handle_ShootActionEnds;
        Move = Actions.currentActionMap.FindAction("Move");
        Move.started += Handle_MoveAction;
    }



    private void Handle_MoveAction(InputAction.CallbackContext context)
    {
        if (SelectedLine + Move.ReadValue<float>() * 1 >= 0 && SelectedLine + Move.ReadValue<float>() * 1 <= 4)
        {
            SelectedLine += (int)Move.ReadValue<float>() * 1;
        }
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, Lines[SelectedLine].transform.position.y, this.gameObject.transform.position.z);
    }

    private void Handle_ShootActionStart(InputAction.CallbackContext context)
    {

        if (!NoManualShoot)
        {
            CancelInvoke("checkManual");
            StopAllCoroutines();
            NoManualShoot = true;
            InvokeRepeating("ShootBullet", 0f, 0.5f);
            StartCoroutine(NoMoeShoot());
        }
        else
        {
            CachedContext = context;
            InvokeRepeating("checkManual", 0f, 0.1f);
        }


    }
    private void Handle_ShootActionEnds(InputAction.CallbackContext context)
    {
        CancelInvoke("ShootBullet");
        CancelInvoke("checkManual");
    }

    public void ShootBullet()
    {
        ShootAudio.Play();
        BarrelSmoke.SetActive(true);
        StartCoroutine(Smoke());
        Instantiate(Manager.BulletPrefab, Barrel.transform.position, Quaternion.identity);
    }

    public IEnumerator NoMoeShoot()
    {
        yield return new WaitForSeconds(0.5f);
        NoManualShoot = false;
    }

    public void checkManual()
    {
        if (!NoManualShoot)
        {
            Handle_ShootActionStart(CachedContext);
        }
    }

    public IEnumerator Smoke()
    {
        yield return new WaitForSeconds(0.35f);
        BarrelSmoke.SetActive(false);
    }


}
