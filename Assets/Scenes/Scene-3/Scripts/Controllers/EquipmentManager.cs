using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Следит за оборудованием. Имеет функции добавления и удаления элементов. */

public class EquipmentManager : MonoBehaviour {

	#region Singleton

    public enum MeshBlendShape {Torso, Arms, Legs };
    public Equipment[] defaultEquipment;

	public static EquipmentManager instance;
	public SkinnedMeshRenderer targetMesh;

    SkinnedMeshRenderer[] currentMeshes;

	void Awake ()
	{
		instance = this;
	}

	#endregion

	Equipment[] currentEquipment;   // Предметы, которые мы в экипировали

	// Обратный вызов, когда предмет экипирован/не экипирован
	public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
	public OnEquipmentChanged onEquipmentChanged;
   

	Inventory inventory;    // Ссылка на наш инвентарь

	void Start ()
	{
		inventory = Inventory.instance;     // Получить ссылку на наш инвентарь

		// Инициализировать currentEquipment на основе количества слотов оборудования
		int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
		currentEquipment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        EquipDefaults();
	}

	// Экипировать новый предмет
	public void Equip (Equipment newItem)
	{
		// Узнайте, в какой слот подходит предмет
		int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = Unequip(slotIndex);

		// Предмет был экипирован, поэтому мы запускаем обратный вызов
		if (onEquipmentChanged != null)
		{
			onEquipmentChanged.Invoke(newItem, oldItem);
		}

		// Вставить предмет в слот
		currentEquipment[slotIndex] = newItem;
        AttachToMesh(newItem, slotIndex);
	}

	// Снять предмет с определенным индексом
	public Equipment Unequip (int slotIndex)
	{
        Equipment oldItem = null;
		// Проверка, есть ли предмет
		if (currentEquipment[slotIndex] != null)
		{
			// Добавить предмет в инвентарь
			oldItem = currentEquipment[slotIndex];
			inventory.Add(oldItem);

            SetBlendShapeWeight(oldItem, 0);
            // Уничтожаем MESH
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }

			// Удалить предмет из массива снаряжения
			currentEquipment[slotIndex] = null;

			// Оборудование было удалено, поэтому мы запускаем обратный вызов
			if (onEquipmentChanged != null)
			{
				onEquipmentChanged.Invoke(null, oldItem);
			}
		}
        return oldItem;
	}

	// Снять все предметы
	public void UnequipAll ()
	{
		for (int i = 0; i < currentEquipment.Length; i++)
		{
			Unequip(i);
		}

        EquipDefaults();
	}

    void AttachToMesh(Equipment item, int slotIndex)
	{

        SkinnedMeshRenderer newMesh = Instantiate(item.mesh) as SkinnedMeshRenderer;
        newMesh.transform.parent = targetMesh.transform.parent;

        newMesh.rootBone = targetMesh.rootBone;
		newMesh.bones = targetMesh.bones;
		
		currentMeshes[slotIndex] = newMesh;


        SetBlendShapeWeight(item, 100);
       
	}

    void SetBlendShapeWeight(Equipment item, int weight)
    {
		foreach (MeshBlendShape blendshape in item.coveredMeshRegions)
		{
			int shapeIndex = (int)blendshape;
            targetMesh.SetBlendShapeWeight(shapeIndex, weight);
		}
    }

    void EquipDefaults()
    {
		foreach (Equipment e in defaultEquipment)
		{
			Equip(e);
		}
    }

	void Update ()
	{
		// Снимите все предметы, если мы нажмем U
		if (Input.GetKeyDown(KeyCode.U))
			UnequipAll();
	}

}
