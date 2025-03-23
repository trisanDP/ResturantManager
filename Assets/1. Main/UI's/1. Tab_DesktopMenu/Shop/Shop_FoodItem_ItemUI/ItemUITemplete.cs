using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RestaurantManagement {
    public class ItemUITemplete : MonoBehaviour {
        #region Fields

        public TextMeshProUGUI foodNameText;
        public TextMeshProUGUI costText;
        public Button addToCartButton;
        public TextMeshProUGUI amount;

        private FoodItemData itemData;
        private ShopLogicManager shopLogicManager;

        #endregion

        #region Initialization

        public void Initialize(FoodItemData itemData, ShopLogicManager shopLogicManager) {
            this.itemData = itemData;
            this.shopLogicManager = shopLogicManager;

            foodNameText.text = itemData.FoodName;
            costText.text = $"${itemData.costPrice}";

            addToCartButton.onClick.AddListener(() => {
                shopLogicManager.AddToCart(itemData);
            });
        }

        #endregion
    }
}
