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

        #region PROPERTIES

        public float TotalParticleDuration { get => InitialImpulseDuration + GoToRecieverDuration; }

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
                TweenCallback onEnd = () => ParticleReachedReciever(particle, amountToIncrease, reciever);
                SetAnimation(particle, reciever.transform.position, onEnd);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        private void ParticleReachedReciever(GameObject particle, int amountToIncrease, NumberCounter reciever)
        {
            Destroy(particle);
            reciever.Increase(amountToIncrease);
            audioManager.PlaySound(particlePopSound);
        }

        private Vector2 RandomImpulse()
        {
            return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * InitialImpulse;
        }

        private void SetAnimation(GameObject gameObject, Vector3 recieverPosition, TweenCallback onEnd)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(InitialImpulseTween(gameObject));
            sequence.Append(GoToRecieverTween(gameObject, recieverPosition));
            sequence.Insert(0, ScaleTween(gameObject));
            sequence.OnComplete(onEnd);
        }

        private Tween InitialImpulseTween(GameObject gameObject)
        {
            return gameObject.transform.DOMove(RandomImpulse(), InitialImpulseDuration).SetRelative().SetEase(Ease.OutCubic);
        }

        private Tween GoToRecieverTween(GameObject gameObject, Vector3 recieverPosition)
        {
            return gameObject.transform.DOMove(recieverPosition, GoToRecieverDuration).SetEase(Ease.InCubic);
        }

        private Tween ScaleTween(GameObject gameObject)
        {
            return gameObject.transform.DOScale(gameObject.transform.localScale, TotalParticleDuration).From(Vector3.zero).SetEase(tweenScaleEase);
        }

        #endregion
    }
}
