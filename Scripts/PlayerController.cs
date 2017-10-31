using UnityEngine;

//To make network aware
using UnityEngine.Networking;


//For network aware change mono to networkbehaviour
public class PlayerController : NetworkBehaviour
{

    public GameObject bulletPrefab; //ref for the bullet object
    public Transform bulletSpawn;   //reference to the location of the bullet spawn

    void Update()
    {
        //For networking - Will exit out the update before executing anything important if it's not the local player and connection
        if (!isLocalPlayer)
        {
            return;
        }
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    [Command]
    void CmdFire()
    {
        //Create a bullet from the bullet prefab
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        //Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        //Spawn the bullet on the clients
        NetworkServer.Spawn(bullet);

        //Destroy bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}