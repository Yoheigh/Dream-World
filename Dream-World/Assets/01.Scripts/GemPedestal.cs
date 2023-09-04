using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GemPedestal : InteractionObject
{
    public GameObject Gem;

    public bool isHaveGem = false;

    public bool isActivated = false;

    public Light gemLight;

    public override ObjectType ObjectType => ObjectType.Grabable;

    public override void InteractWithPlayer(PlayerController _player)
    {
        if (isHaveGem == true) return;

        _player.interaction.InteractionObj = Gem.GetComponent<InteractionObject>() as Grabable;
        Gem.GetComponent<Rigidbody>().useGravity = true;
        Gem.GetComponent<Rigidbody>().isKinematic = false;
        Gem = null;
        gemLight.enabled = false;
        isHaveGem = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHaveGem == true) return;

        if (other.name == "Gem")
        {
            Gem = other.gameObject;
            Gem.GetComponent<Collider>().enabled = false;
            Gem.GetComponent<Rigidbody>().useGravity = false;
            Gem.GetComponent<Rigidbody>().isKinematic = true;
            Gem.transform.position = transform.position + Vector3.up;
            gemLight.enabled = true;
            isHaveGem = true;
            Debug.Log("Áª ÀåÂø ¿Ï·á");
        }
    }

    public void DoorBehaviour()
    {
        if (isHaveGem == true) return;


        isActivated = true;

        // StartCoroutine(Manager.Instance.Flag.DoorAction001());
    }
}
