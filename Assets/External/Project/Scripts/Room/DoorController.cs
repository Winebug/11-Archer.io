using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    public Animator animator;

    private RoomManager roomManager;

    private void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
        Debug.Log("문 열림");

        if (animator != null)
            animator.SetBool("Open", true);

        GetComponent<Collider2D>().isTrigger = true;
    }
}
