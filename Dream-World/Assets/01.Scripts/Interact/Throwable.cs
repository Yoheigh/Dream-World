using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Grabable
{
    public override void OnRelease()
    {
        base.OnRelease();
        GetComponent<Collider>().isTrigger = true;
        StartCoroutine(RotateObject());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false && other.CompareTag("GamePoint") == false)
        {
            if(other.TryGetComponent<MonsterV2>(out var obj) == true) other.GetComponent<MonsterV2>().Hit();
            GameObject vfx = Instantiate(Managers.Instance.Build.BuildVFX, transform.position, Quaternion.identity, null);
            Destroy(vfx, 4f);
            Destroy(gameObject);
            Managers.Sound.PlaySFX(6);
        }
    }

    IEnumerator RotateObject()
    {
        while(true)
        {
            transform.rotation *= Quaternion.Euler(new Vector3(3f, 0f, 0.2f));
            yield return null;
        }
    }
}
