using System;
using HytaleClient.Data.Items;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x02000884 RID: 2180
	internal static class IconHelper
	{
		// Token: 0x06003DBF RID: 15807 RVA: 0x000A0C84 File Offset: 0x0009EE84
		public static ClientItemIconProperties GetIconProperties(ClientItemBase item)
		{
			ClientItemArmor armor = item.Armor;
			ClientItemIconProperties defaultIconProperties = IconHelper.GetDefaultIconProperties(item, (armor != null) ? new ItemBase.ItemArmor.ItemArmorSlot?(armor.ArmorSlot) : null);
			bool flag = item.IconProperties != null;
			if (flag)
			{
				defaultIconProperties.Scale = item.IconProperties.Scale;
				bool flag2 = item.IconProperties.Translation != null;
				if (flag2)
				{
					defaultIconProperties.Translation = item.IconProperties.Translation;
				}
				bool flag3 = item.IconProperties.Rotation != null;
				if (flag3)
				{
					defaultIconProperties.Translation = item.IconProperties.Translation;
				}
			}
			return defaultIconProperties;
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x000A0D2C File Offset: 0x0009EF2C
		private static ClientItemIconProperties GetDefaultIconProperties(ClientItemBase item, ItemBase.ItemArmor.ItemArmorSlot? armorSlot)
		{
			return IconHelper.GetDefaultIconProperties(item.Weapon != null, item.Tool != null, item.Armor != null, armorSlot);
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x000A0D60 File Offset: 0x0009EF60
		public static ClientItemIconProperties GetDefaultIconProperties(bool isWeapon, bool isTool, bool isArmor, ItemBase.ItemArmor.ItemArmorSlot? armorSlot)
		{
			ClientItemIconProperties result;
			if (isWeapon)
			{
				result = new ClientItemIconProperties
				{
					Scale = 0.37f,
					Translation = new Vector2?(new Vector2(-24.6f, -24.6f)),
					Rotation = new Vector3?(new Vector3(45f, 90f, 0f))
				};
			}
			else if (isTool)
			{
				result = new ClientItemIconProperties
				{
					Scale = 0.5f,
					Translation = new Vector2?(new Vector2(-17.4f, -12f)),
					Rotation = new Vector3?(new Vector3(45f, 270f, 0f))
				};
			}
			else
			{
				if (isArmor)
				{
					ItemBase.ItemArmor.ItemArmorSlot? itemArmorSlot = armorSlot;
					if (itemArmorSlot != null)
					{
						switch (itemArmorSlot.GetValueOrDefault())
						{
						case 0:
							return new ClientItemIconProperties
							{
								Scale = 0.5f,
								Translation = new Vector2?(new Vector2(--0f, -3f)),
								Rotation = new Vector3?(new Vector3(22.5f, 45f, 22.5f))
							};
						case 1:
							return new ClientItemIconProperties
							{
								Scale = 0.5f,
								Translation = new Vector2?(new Vector2(--0f, -5f)),
								Rotation = new Vector3?(new Vector3(22.5f, 45f, 22.5f))
							};
						case 2:
							return new ClientItemIconProperties
							{
								Scale = 0.92f,
								Translation = new Vector2?(new Vector2(--0f, -10.8f)),
								Rotation = new Vector3?(new Vector3(22.5f, 45f, 22.5f))
							};
						case 3:
							return new ClientItemIconProperties
							{
								Scale = 0.5f,
								Translation = new Vector2?(new Vector2(--0f, -25.8f)),
								Rotation = new Vector3?(new Vector3(22.5f, 45f, 22.5f))
							};
						}
					}
				}
				result = new ClientItemIconProperties
				{
					Scale = 0.58823f,
					Translation = new Vector2?(new Vector2(0f, -13.5f)),
					Rotation = new Vector3?(new Vector3(22.5f, 45f, 22.5f))
				};
			}
			return result;
		}
	}
}
