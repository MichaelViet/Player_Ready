using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Класс, используемый для всех характеристик, где мы хотим иметь возможность добавлять/удалять модификаторы */

[System.Serializable]
public class Stat {

	[SerializeField]
	private int baseValue;  // Начальное значение

	// Список модификаторов, изменяющих baseValue
	private List<int> modifiers = new List<int>();

	// Получить окончательное значение после применения модификаторов
	public int GetValue ()
	{
		int finalValue = baseValue;
		modifiers.ForEach(x => finalValue += x);
		return finalValue;
	}

	// Добавить новый модификатор
	public void AddModifier (int modifier)
	{
		if (modifier != 0)
			modifiers.Add(modifier);
	}

	// Удалить модификатор
	public void RemoveModifier (int modifier)
	{
		if (modifier != 0)
			modifiers.Remove(modifier);
	}

}
