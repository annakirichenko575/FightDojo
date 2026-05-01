using UnityEngine;

namespace FightDojo.UI.Database.Combo
{
    public class DeleteComboButton : MonoBehaviour
    {
        private ComboDataProvider _comboDataProvider;

        private void Awake()
        {
            _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
        }

        public void DeleteCombo()
        {
            _comboDataProvider.DeleteCombo();
        }
    }
}