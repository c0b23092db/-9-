using UnityEngine;
public class commonParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem cursorParticle;

    private void Update()
    {
        // TimeScale に関係なくパーティクルを更新
        float delta = Time.unscaledDeltaTime;
        cursorParticle.Simulate(delta, withChildren: true, restart: false);
    }
}