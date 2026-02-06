using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float damage;
    [SerializeField] private GameObject bulletHolePrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("[Bullet] bullet hit enemy");
            other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }

        else if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
        else
        {
            var contact = other.GetContact(0);
            var normalOffset = 0.05f;
            var spawnPoint = contact.point + contact.normal * normalOffset;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.back, contact.normal);
            var bulletHole = Instantiate(bulletHolePrefab, spawnPoint, rotation, other.transform);

            var aliveTime = 5f;
            Destroy(bulletHole, aliveTime);
        }

        Destroy(gameObject);
    }

    // video testing

    private void VideoTest()
    {
        var videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Play();
        videoPlayer.Pause();
        videoPlayer.Stop();
        var isPlaying = videoPlayer.isPlaying;
        var isPaused = videoPlayer.isPaused;
        var length = videoPlayer.length;
        var clip = videoPlayer.clip; // also is setter
    }

    private void TimelineTest()
    {
        var director = GetComponent<PlayableDirector>();
        director.Play();
        director.Pause();
        director.Stop();
        var isPlaying = director.state == PlayState.Playing;
        var isPaused = director.state ==  PlayState.Paused;
    }
}