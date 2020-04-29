using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

using Zenject;
using CFLFramework.Data;

namespace CFLFramework.Collectibles
{
    public class CollectiblesManager : MonoBehaviour
    {
        #region FIELDS

        private const string CollectiblesResourceFolder = "Collectibles";
        internal const string CollectiblesDataKey = "collectibles";

        [Inject] private DataManager dataManager = null;

        private List<Collectible> collectibles = new List<Collectible>();

        #endregion

        #region EVENTS

        public event UnityAction<Collectible, int> onCollectibleIncreased;
        public event UnityAction<Collectible, int> onCollectibleSpent;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            LoadCollectibles();
        }

        private void LoadCollectibles()
        {
            Object[] collectibleObjects = Resources.LoadAll(CollectiblesResourceFolder, typeof(Collectible));
            foreach (Collectible collectible in collectibleObjects)
            {
                collectibles.Add(collectible);
                collectible.Load(dataManager);
            }
        }

        internal void ForceCollectiblesUpdate()
        {
            foreach (Collectible collectible in collectibles)
                collectible.ForceUpdate();
        }

        public Collectible GetCollectible(string name)
        {
            return GetCollectible(null, name);
        }

        public Collectible GetCollectible(string tag, string name)
        {
            foreach (Collectible collectible in collectibles)
                if (collectible.Name == name && (tag == null || collectible.Tag == tag))
                    return collectible;

            return null;
        }

        public Collectible[] GetCollectibles(string tag)
        {
            return collectibles.Where(collectible => collectible.Tag == tag).ToArray();
        }

        public void IncreaseCollectible(string tag, string name, int amount)
        {
            IncreaseCollectible(GetCollectible(tag, name), amount);
        }

        public void IncreaseCollectible(string name, int amount)
        {
            IncreaseCollectible(GetCollectible(name), amount);
        }

        public void IncreaseCollectible(Collectible collectible, int amount)
        {
            collectible.IncreaseCollectible(amount);
            onCollectibleIncreased?.Invoke(collectible, amount);
        }

        public bool SpendCollectible(string tag, string name, int amount)
        {
            return SpendCollectible(GetCollectible(tag, name), amount);
        }

        public bool SpendCollectible(string name, int amount)
        {
            return SpendCollectible(GetCollectible(name), amount);
        }

        public bool SpendCollectible(Collectible collectible, int amount)
        {
            bool spent = collectible.SpendCollectible(amount);
            if (spent)
                onCollectibleSpent?.Invoke(collectible, amount);

            return spent;
        }

        #endregion
    }
}
