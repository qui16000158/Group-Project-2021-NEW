using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideJump : MonoBehaviour
{
    [SerializeField]
    GameObject slidePrefab; // The prefab for the slide
    [SerializeField]
    Vector3 rotationOffset; // The offset for the player's rotation when sliding

    public void StartSlide(Transform player)
    {
        Transform slide = Instantiate(slidePrefab, player.position, Quaternion.identity).transform;

        Transform agent = slide.Find("Slide Anchor");

        player.parent = agent;

        Debug.Log(player.parent);

        Animator anim = slide.GetComponent<Animator>();

        anim.SetTrigger("Start Slide");
    }
}
