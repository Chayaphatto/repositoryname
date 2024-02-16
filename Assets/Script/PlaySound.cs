using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public List<AudioClip> playerWalking;
    public AudioClip playerJumping;
    public AudioSource playerSource;

    public int pos;

    public static PlaySound instace;
    private void Awake()
    {
        instace = this;
        playerSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playWalking()
    {
        pos = (int)Mathf.Floor(Random.Range(0,playerWalking.Count));
        playerSource.PlayOneShot(playerWalking[pos]);
    }
    public void playPlayerJumping()
    {
        playerSource.PlayOneShot(playerJumping);
    }
}
