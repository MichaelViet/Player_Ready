using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    // Масив анімацій зброї
    public WeaponAnimations[] weaponAnimations;

    // Словник для зберігання анімацій зброї залежно від екіпірованої зброї
    Dictionary<Equipment, AnimationClip[]> weaponAnimationsDict;

    protected override void Start()
    {
        base.Start();
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;

        // Ініціалізуємо словник та заповнюємо його анімаціями зброї
        weaponAnimationsDict = new Dictionary<Equipment, AnimationClip[]>();
        foreach (WeaponAnimations animation in weaponAnimations)
        {
            weaponAnimationsDict.Add(animation.weapon, animation.clips);
        }
    }

    // Функція зворотнього виклику для зміни екіпунку
    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null && newItem.equipSlot == EquipmentSlot.Weapon)
        {
            // Вмикаємо шар анімацій зброї
            animator.SetLayerWeight(1, 1);

            // Встановлюємо поточний набір анімацій атаки залежно від екіпірованої зброї
            if (weaponAnimationsDict.ContainsKey(newItem))
            {
                currentAttackAnimSet = weaponAnimationsDict[newItem];
            }
        }
        else if (newItem == null && oldItem != null && oldItem.equipSlot == EquipmentSlot.Weapon)
        {
            // Вимикаємо шар анімацій зброї
            animator.SetLayerWeight(1, 0);

            // Повертаємося до стандартного набору анімацій атаки
            currentAttackAnimSet = defaultAttackAnimSet;
        }

        if (newItem != null && newItem.equipSlot == EquipmentSlot.Shield)
        {
            // Вмикаємо шар анімацій щита
            animator.SetLayerWeight(2, 1);
        }
        else if (newItem == null && oldItem != null && oldItem.equipSlot == EquipmentSlot.Shield)
        {
            // Вимикаємо шар анімацій щита
            animator.SetLayerWeight(2, 0);
        }
    }

    // Структура для зберігання анімацій зброї
    [System.Serializable]
    public struct WeaponAnimations
    {
        public Equipment weapon;
        public AnimationClip[] clips;
    }
}
