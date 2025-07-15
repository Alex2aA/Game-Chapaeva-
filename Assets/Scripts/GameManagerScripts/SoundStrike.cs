using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundStrike : MonoBehaviour
{
    [Header("Настройки звуков")]
    [Tooltip("Добавьте 5 аудио клипов")]
    public List<AudioClip> collisionSounds = new List<AudioClip>(5);

    public static SoundStrike Instance { get; private set; }
    
    [Header("Настройки воспроизведения")]

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayRandomCollisionSound()
    {
        if (collisionSounds == null || collisionSounds.Count == 0)
        {
            Debug.LogWarning("No collision sounds assigned!");
            return;
        }

        // Выбираем случайный звук
        int randomIndex = Random.Range(0, collisionSounds.Count);
        AudioClip randomClip = collisionSounds[randomIndex];

        // Настраиваем pitch для вариативности
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        
        // Воспроизводим звук
        audioSource.PlayOneShot(randomClip);
    }
}