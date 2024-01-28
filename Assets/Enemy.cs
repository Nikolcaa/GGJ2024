using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class EnemyData
{
    public int MaxHp;
    public float MovementSpeed;
}

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData;
    private Animator anim;
    [HideInInspector] public Transform graphChild;
    public Slider HpSlider;
    private Canvas canvas;
    private Transform graph;
    private SkinnedMeshRenderer meshRenderer;
    private Color cachedMaterialColor;
    private CharacterController characterController;
    public GameObject deathPS;

    [Header("PROPERTIES")]
    public float StartFollowingRadius;
    public LayerMask playerMask;
    public int regenerationAmount;

    private bool isFollowing = false;
    private bool isDead = false;

    private float currentHp;

    private Vector3 cachedScaleOfGraphChild;
    private AudioSource audioSource;

    [Header("AudioClips")]
    public AudioClip[] startFollowSounds;

    public AudioClip[] laughSounds;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        canvas = GetComponentInChildren<Canvas>();
        graph = transform.GetChild(0);
        graphChild = transform.GetChild(0).GetChild(0);
        cachedScaleOfGraphChild = graphChild.localScale;
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        characterController = graphChild.GetComponent<CharacterController>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        canvas.transform.SetParent(graphChild);
        UpdateHp(EnemyData.MaxHp);
        anim.SetFloat("MovementSpeed", EnemyData.MovementSpeed);
        cachedMaterialColor = meshRenderer.material.color;
    }

    float t = 0;
    private void Update()
    {
        if (isDead || PlayerMovement.isDead)
            return;

        if (!isFollowing && Physics.OverlapSphere(transform.position, StartFollowingRadius, playerMask, QueryTriggerInteraction.Collide).Count() > 0)
        {
            StartFollowing();
            PlayRandomStartSound();
        }
        else if (isFollowing)
        {
            Vector3 DIR = PlayerMovement.Transform.position - graphChild.position;
            Vector3 dir = new Vector3(DIR.x, 0, DIR.z).normalized;
            graphChild.forward = dir;

            
            if(currentHp < EnemyData.MaxHp)
            {
                t = 0;
                UpdateHp(Time.deltaTime * regenerationAmount);
            }
        }

        // Scale with hp
        Vector3 newScale;
        newScale = Vector3.LerpUnclamped(new Vector3(cachedScaleOfGraphChild.x + 0.5f, cachedScaleOfGraphChild.y, cachedScaleOfGraphChild.z + 0.7f), cachedScaleOfGraphChild, (currentHp / EnemyData.MaxHp));
        graphChild.localScale = newScale;

        // Color with hp
        Color newColor;
        newColor = Color.Lerp(Color.red, cachedMaterialColor, currentHp / EnemyData.MaxHp);
        meshRenderer.material.color = newColor;

        // Gravity
        Vector3 _moveDirection = Vector3.zero;
        _moveDirection.y -= 5 * Time.deltaTime;
        characterController.Move(_moveDirection);
    }

    private void StartFollowing()
    {
        isFollowing = true;
        anim.SetTrigger("Walk");
    }

    public void Damaged(float Amount)
    {
        UpdateHp(Amount);
        PlayRandomLaughSound();

        if (!isFollowing)
        {
            StartFollowing();
        }
    }

    public void UpdateHp(float Amount)
    {
        currentHp += Amount;

        if(currentHp > EnemyData.MaxHp)
            currentHp = EnemyData.MaxHp;

        HpSlider.maxValue = EnemyData.MaxHp;
        HpSlider.value = currentHp;

        if (currentHp <= 0)
            Die();
    }

    public void Die()
    {
        isDead = true;
        Instantiate(deathPS, graphChild.position, Quaternion.identity);
        Destroy(gameObject);
    }

    int amountCalled = 0;
    int previousLaughSoundIndex = -1;
    private void PlayRandomLaughSound()
    {
        amountCalled++;

        int current = Random.Range(0, laughSounds.Length);
        while (current == previousLaughSoundIndex)
            current = Random.Range(0, laughSounds.Length);

        if (!audioSource.isPlaying || amountCalled >= 5)
        {
            amountCalled = 0;
            audioSource.PlayOneShot(laughSounds[current]);
            previousLaughSoundIndex = current;
        }

    }

    int previousStartSoundIndex;
    private void PlayRandomStartSound()
    {
        amountCalled++;

        int current = Random.Range(0, startFollowSounds.Length);
        while (current == previousStartSoundIndex)
            current = Random.Range(0, startFollowSounds.Length);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(startFollowSounds[current]);
            previousStartSoundIndex = current;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, StartFollowingRadius);
    }
}
