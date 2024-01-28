using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform graph;
    public SpriteRenderer sprite;
    public Collider coll;
    private Rigidbody rb;

    public int Damage;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        graph.transform.DORotate(new Vector3(0, 0, Random.Range(-90f, 90f)), 0.5f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);

        graph.transform.DOLocalMove(new Vector3(Random.Range(-3f, 3f), 0, 0), 0.3f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);

        //sprite.color = Random.ColorHSV();
        sprite.DOColor(Random.ColorHSV(0, 1, 0.8f, 1, 0.8f, 1, 1, 1), Random.Range(0.5f, 1f))
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
            enemy.UpdateHp(-Damage);
        }

        Destroy(gameObject);
    }
}
