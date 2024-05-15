using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator anim;
    public string playerTag;
    public string playerNoWeaponTag;
    public Transform door;
    public float distanceToOpen;

    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);
        GameObject[] playerNoWeapon = GameObject.FindGameObjectsWithTag(playerNoWeaponTag);

        bool isPlayerNear = false;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, door.position);
            if (distance <= distanceToOpen)
            {
                isPlayerNear = true;
                break;
            }
        }
        foreach (GameObject player in playerNoWeapon)
        {
            float distance = Vector3.Distance(player.transform.position, door.position);
            if (distance <= distanceToOpen)
            {
                isPlayerNear = true;
                break;
            }
        }

        anim.SetBool("Near", isPlayerNear);
    }
}
