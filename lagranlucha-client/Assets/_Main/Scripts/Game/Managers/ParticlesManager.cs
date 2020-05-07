using UnityEngine;
using System.Collections;

using DG.Tweening;
using Zenject;
using CFLFramework.Audio;

namespace LaGranLucha.Game
{
    public class ParticlesManager : MonoBehaviour
    {
        #region FIELDS

        private const int MaximumParticles = 20;
        private const float MaximumSpawnDuration = 0.5f;
        private const float InitialImpulse = 1.5f;
        private const float InitialImpulseDuration = 0.3f;
        private const float GoToRecieverDuration = 0.5f;

        [Inject] private AudioManager audioManager = null;

        [SerializeField] private AudioClip particlePopSound = null;
        [SerializeField] private AnimationCurve tweenScaleEase = null;

        #endregion

        #region BEHAVIORS

        public void SpawnParticles(GameObject particlePrefab, NumberCounter reciever, Vector2 position, int amount = 1)
        {
            StartCoroutine(SpawnCoinsParticlesCoroutine(particlePrefab, reciever, position, amount));
        }

        private IEnumerator SpawnCoinsParticlesCoroutine(GameObject particlePrefab, NumberCounter reciever, Vector2 position, int amount)
        {
            int particlesToSpawn = amount < MaximumParticles ? amount : MaximumParticles;
            int amountPerParticle = Mathf.RoundToInt((float)amount / (float)particlesToSpawn);
            int coinsRemaining = amount;
            float spawnDelay = MaximumSpawnDuration / particlesToSpawn;

            for (int i = 0; i < particlesToSpawn; i++)
            {
                int amountToIncrease = (i == particlesToSpawn - 1) ? coinsRemaining : amountPerParticle;
                coinsRemaining -= amountToIncrease;
                GameObject particle = Instantiate<GameObject>(particlePrefab, position, Quaternion.identity, transform);
                TweenCallback onEnd = () => { Destroy(particle.gameObject); reciever.Increase(amountToIncrease); audioManager.PlaySound(particlePopSound); };
                SetAnimation(particle, reciever.transform.position, InitialImpulseDuration, onEnd);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        private Vector2 RandomVector2()
        {
            return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        }

        private void SetAnimation(GameObject gameObject, Vector3 position, float initialImpluseDuration, TweenCallback onEnd)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(gameObject.transform.DOMove(RandomVector2() * InitialImpulse, initialImpluseDuration).SetRelative().SetEase(Ease.OutCubic));
            sequence.Append(gameObject.transform.DOMove(position, GoToRecieverDuration).SetEase(Ease.InCubic));
            sequence.Insert(0, gameObject.transform.DOScale(gameObject.transform.localScale, initialImpluseDuration + GoToRecieverDuration).From(Vector3.zero).SetEase(tweenScaleEase));
            sequence.OnComplete(onEnd);
        }

        #endregion
    }
}
