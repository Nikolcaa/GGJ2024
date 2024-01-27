using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform graph;
    public SpriteRenderer sprite;
    private void Start()
    {
        //graph.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        graph.transform.DORotate(new Vector3(0, 0, Random.Range(-90f, 90f)), 0.7f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);

        graph.transform.DOLocalMove(new Vector3(Random.Range(-1.5f, 1.5f), 0, 0), 0.5f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);

        //sprite.color = Random.ColorHSV();
        sprite.DOColor(Random.ColorHSV(0, 1, 0.8f, 1, 0.8f, 1, 1, 1), Random.Range(0.5f, 1f))
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
