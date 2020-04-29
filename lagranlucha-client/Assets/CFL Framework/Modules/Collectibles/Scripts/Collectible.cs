using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

using CFLFramework.Data;
using CFLFramework.Utilities.Inspector;

namespace CFLFramework.Collectibles
{
    [CreateAssetMenu(menuName = ScriptableCreationRoute)]
    public class Collectible : ScriptableObject
    {
        #region FIELDS

        private const string ResetToolName = "Reset Data";
        private const string ScriptableCreationRoute = "CFL Framework/Collectibles/Create Collectible";

        [Header("DATA")]
        [SerializeField] private string tag = null;
        [SerializeField] private new string name = null;
        [ReadOnly] [SerializeField] private float quantity = default(float);

        [Header("EXTRA DATA")]
        [SerializeField] private Sprite sprite = null;
        [SerializeField] private string displayName = "";
        [SerializeField] private float defaultValue = default(float);

        private DataManager dataManager = null;
        private string[] keys = null;

        #endregion

        #region EVENTS

        public event UnityAction updated;

        #endregion

        #region PROPERTIES

        public string Tag { get => tag; }
        public string Name { get => name; }
        public float Quantity { get => quantity; }
        public Sprite Sprite { get => sprite; }
        public string DisplayName { get => displayName; }
        public float DefaultValue { get => defaultValue; }

        #endregion

        #region BEHAVIORS

        internal void Load(DataManager dataManager)
        {
            GenerateKeys();
            this.dataManager = dataManager;
            quantity = dataManager.GetData<float>(keys, defaultValue);
        }

        internal void ForceUpdate()
        {
            var newQuantity = dataManager.GetData<float>(keys, defaultValue);

            if (quantity != newQuantity)
            {
                quantity = newQuantity;
                updated?.Invoke();
            }
        }

        internal void IncreaseCollectible(float amount)
        {
            quantity = Mathf.Clamp(quantity + amount, 0, Mathf.Infinity);
            Save();
        }

        internal bool SpendCollectible(float amount)
        {
            if (amount > quantity)
                return false;

            quantity -= amount;
            Save();
            return true;
        }

        private void Save()
        {
            dataManager.SetData(keys, quantity);
            updated?.Invoke();
        }

        private void GenerateKeys()
        {
            List<string> keyList = new List<string>();
            keyList.Add(CollectiblesManager.CollectiblesDataKey);

            if (!string.IsNullOrEmpty(tag))
                keyList.Add(tag);

            if (!string.IsNullOrEmpty(name))
                keyList.Add(name);

            keys = keyList.ToArray();
        }

        [ContextMenu(ResetToolName)]
        private void ResetData()
        {
            DataManager.ResetKey(keys, defaultValue);
        }

        #endregion
    }
}
