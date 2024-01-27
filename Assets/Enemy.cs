using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
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

    [Header("PROPERTIES")]
    public float StartFollowingRadius;
    public LayerMask playerMask;
    public int regenerationAmount;

    private bool isFollowing = false;
    private bool isDead = false;

    private float currentHp;

    private Vector3 cachedScaleOfGraphChild;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        canvas = GetComponentInChildren<Canvas>();
        graph = transform.GetChild(0);
        graphChild = transform.GetChild(0).GetChild(0);
        cachedScaleOfGraphChild = graphChild.localScale;
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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

            Vector3 newScale;
            newScale = Vector3.LerpUnclamped(new Vector3(cachedScaleOfGraphChild.x + 0.5f, cachedScaleOfGraphChild.y, cachedScaleOfGraphChild.z + 0.7f), cachedScaleOfGraphChild, (currentHp / EnemyData.MaxHp));                     
            graphChild.localScale = newScale;

            Color newColor;
            newColor = Color.Lerp(Color.red, cachedMaterialColor, currentHp / EnemyData.MaxHp);
            meshRenderer.material.color = newColor;

        }
    }

    private void StartFollowing()
    {
        isFollowing = true;
        anim.SetTrigger("Walk");
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

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, StartFollowingRadius);
    }
}
