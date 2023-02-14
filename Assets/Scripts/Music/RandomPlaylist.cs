using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlaylist : MonoBehaviour
{

    public AudioClip[] clips;
    public AudioSource source;
    public float newClip;
    public float timer;
    
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = 0.03f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= newClip + 1)
        {
            newClips();
            timer = 0;
        }
    }

    void newClips()
    {
        int clipNum = Random.Range(0, clips.Length);
        if (!source.isPlaying)
        {
            source.loop = true;
            source.PlayOneShot(clips[clipNum]);
        }

        newClip = clips[clipNum].length;
    }
}
