using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using DG.Tweening;


//------------------------------------------
// クラス
//------------------------------------------
[Serializable]
public class AnyDictionary<Tkey, Tvalue>
{
	public Tkey key;
	public Tvalue value;

	public AnyDictionary(Tkey key, Tvalue value)
	{
		this.key = key;
		this.value = value;
	}
	public AnyDictionary(KeyValuePair<Tkey, Tvalue> pair)
	{
		this.key = pair.Key;
		this.value = pair.Value;
	}
}


namespace Controller
{
	[Serializable]
	public class VRMap
	{
		public Transform vrTarget = default;
		public Transform rigTarget = default;
		public Vector3 pos = default;
		public Vector3 rot = default;
		public void Map()
		{
			rigTarget.position = vrTarget.TransformPoint(pos);
			rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(rot);
		}
	}
	public enum StaffHoldingHand { LeftHand, RightHand }
}


//------------------------------------------
// インターフェイス
//------------------------------------------
public interface IApplyDamage
{
	void ApplyDamage(Magic self, CharacterType character, float damage);
}


//------------------------------------------
// 列挙
//------------------------------------------
public enum CharacterType
{
	Player, Enemy, 
}

namespace MagicContext
{
	public enum MagicGrade 
	{
		[InspectorName("初級生成魔法")] Low,
		[InspectorName("中級生成魔法")] Middle,
		[InspectorName("上級生成魔法")] High 
	}
	public enum MagicAttribute 
	{ 
		[InspectorName("炎属性")] Flame, 
		[InspectorName("水属性")] Water, 
		[InspectorName("氷属性")] Ice, 
		[InspectorName("風属性")] Wind, 
		[InspectorName("光属性")] Light,
		[InspectorName("無属性")] None, 
	}

	public static class ContextName
    {
		public static string GradeName(int level)
		{
			if (level == ((int)MagicGrade.Low)) return "初級生成魔法";
			if (level == ((int)MagicGrade.Middle)) return "中級生成魔法";
			if (level == ((int)MagicGrade.High)) return "上級生成魔法";
			return "";
		}
		public static string AtributeName(int attribute)
		{
			if (attribute == ((int)MagicAttribute.Flame)) return "炎属性";
			if (attribute == ((int)MagicAttribute.Water)) return "水属性";
			if (attribute == ((int)MagicAttribute.Ice)) return "氷属性";
			if (attribute == ((int)MagicAttribute.Wind)) return "風属性";
			if (attribute == ((int)MagicAttribute.Light)) return "光属性";
			if (attribute == ((int)MagicAttribute.None)) return "無属性";
			return "";
		}
    }

	[System.Serializable]
	public class GradiantSet
    {
		[System.Serializable]
		class GradWithKey
		{
			[SerializeField] Gradient gradient = default;
			[SerializeField] MagicAttribute attribute = default;

			public Gradient Gradient => gradient;
			public MagicAttribute Attribute => attribute;
		}
		[SerializeField] List<GradWithKey> gradsList = default;


		public Gradient GetGradient(MagicAttribute attribute)
		{
			foreach (var el in gradsList)
			{
				if (el.Attribute == attribute)
				{
					return el.Gradient;
				}
			}
			return null;
		}
	}
}
public enum HandType
{
	LeftHand, RightHand,
}


//------------------------------------------
// ユーティリティ
//------------------------------------------
namespace CMN
{
	public static class Utility
	{
		public static TValue GetDICVal<TValue, TKey>(TKey component, List<AnyDictionary<TKey, TValue>> dics)
		{
			foreach (var dic in dics)
			{
				if (dic.key.Equals(component))
				{
					return dic.value;
				}
			}
			return default;
		}
		public static T GetNextEnum<T>(int currentEnum)
		{
			int nextIndex = currentEnum + 1;
			T nextEnum = (T)Enum.ToObject(typeof(T), nextIndex);
			int length = Enum.GetValues(typeof(T)).Length;
			if (nextIndex >= length)
			{
				nextEnum = (T)Enum.ToObject(typeof(T), 0);
			}
			return nextEnum;
		}
		public static T GetIntToEnum<T>(int targetInt)
		{
			T targetEnum = (T)Enum.ToObject(typeof(T), targetInt);
			return targetEnum;
		}
		public static bool Probability(float fPercent)
		{
			float fProbabilityRate = UnityEngine.Random.value * 100.0f;

			if (fPercent == 100.0f && fProbabilityRate == fPercent) return true;
			else if (fProbabilityRate < fPercent) return true;
			else return false;
		}
		public static OVRInput.Controller GetEnableOVRController(StaffHoldingHand holdingHand, bool inverse = false)
		{
			OVRInput.Controller controller;
			if (!inverse)
			{
				controller = (holdingHand == StaffHoldingHand.LeftHand) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
			}
			else
			{
				controller = (holdingHand == StaffHoldingHand.LeftHand) ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch;
			}
			return controller;
		}
	}
}