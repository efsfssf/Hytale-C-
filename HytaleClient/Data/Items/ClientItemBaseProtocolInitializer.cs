using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Audio;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.FX;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame.Modules.BuilderTools.Tools;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AFE RID: 2814
	internal class ClientItemBaseProtocolInitializer
	{
		// Token: 0x06005856 RID: 22614 RVA: 0x001ACD6C File Offset: 0x001AAF6C
		public static void Parse(ItemBase networkItemBase, NodeNameManager nodeNameManager, ref ClientItemBase itemBase)
		{
			itemBase.Id = networkItemBase.Id;
			itemBase.Categories = networkItemBase.Categories;
			itemBase.Set = networkItemBase.Set;
			itemBase.ItemLevel = networkItemBase.ItemLevel;
			itemBase.QualityIndex = networkItemBase.QualityIndex;
			itemBase.SoundEventIndex = ResourceManager.GetNetworkWwiseId(networkItemBase.SoundEventIndex);
			bool flag = networkItemBase.Particles != null;
			if (flag)
			{
				itemBase.Particles = new ModelParticleSettings[networkItemBase.Particles.Length];
				for (int i = 0; i < networkItemBase.Particles.Length; i++)
				{
					itemBase.Particles[i] = new ModelParticleSettings("");
					ParticleProtocolInitializer.Initialize(networkItemBase.Particles[i], ref itemBase.Particles[i], nodeNameManager);
				}
			}
			bool flag2 = networkItemBase.FirstPersonParticles != null;
			if (flag2)
			{
				itemBase.FirstPersonParticles = new ModelParticleSettings[networkItemBase.FirstPersonParticles.Length];
				for (int j = 0; j < networkItemBase.FirstPersonParticles.Length; j++)
				{
					itemBase.FirstPersonParticles[j] = new ModelParticleSettings("");
					ParticleProtocolInitializer.Initialize(networkItemBase.FirstPersonParticles[j], ref itemBase.FirstPersonParticles[j], nodeNameManager);
				}
			}
			itemBase.Trails = networkItemBase.Trails;
			bool flag3 = networkItemBase.Light != null;
			if (flag3)
			{
				ClientItemBaseProtocolInitializer.ParseLightColor(networkItemBase.Light, ref itemBase.LightEmitted);
			}
			itemBase.Scale = networkItemBase.Scale;
			itemBase.Texture = networkItemBase.Texture;
			itemBase.MaxStack = networkItemBase.MaxStack;
			itemBase.Icon = networkItemBase.Icon;
			bool flag4 = networkItemBase.IconProperties != null;
			if (flag4)
			{
				itemBase.IconProperties = new ClientItemIconProperties(networkItemBase.IconProperties);
			}
			bool flag5 = networkItemBase.Recipe != null;
			if (flag5)
			{
				itemBase.Recipe = new ClientItemCraftingRecipe(networkItemBase.Recipe);
			}
			ClientItemBase clientItemBase = itemBase;
			ItemBase.ItemResourceType[] resourceTypes = networkItemBase.ResourceTypes;
			ClientItemResourceType[] resourceTypes2;
			if (resourceTypes == null)
			{
				resourceTypes2 = null;
			}
			else
			{
				resourceTypes2 = Enumerable.ToArray<ClientItemResourceType>(Enumerable.Select<ItemBase.ItemResourceType, ClientItemResourceType>(resourceTypes, (ItemBase.ItemResourceType resource) => new ClientItemResourceType(resource)));
			}
			clientItemBase.ResourceTypes = resourceTypes2;
			itemBase.Consumable = networkItemBase.Consumable;
			itemBase.PlayerAnimationsId = networkItemBase.PlayerAnimationsId;
			itemBase.UsePlayerAnimations = networkItemBase.UsePlayerAnimations;
			itemBase.ReticleIndex = networkItemBase.ReticleIndex;
			itemBase.BlockId = networkItemBase.BlockId;
			itemBase.Tool = networkItemBase.Tool;
			itemBase.BuilderTool = ((networkItemBase.BuilderToolData != null) ? new BuilderTool(networkItemBase.BuilderToolData) : null);
			itemBase.Armor = ((networkItemBase.Armor != null) ? new ClientItemArmor(networkItemBase.Armor) : null);
			itemBase.Weapon = networkItemBase.Weapon;
			itemBase.Utility = (networkItemBase.Utility ?? new ItemBase.ItemUtility());
			itemBase.Durability = networkItemBase.Durability;
			itemBase.ItemEntity = networkItemBase.ItemEntity;
			itemBase.Interactions = networkItemBase.Interactions;
			itemBase.InteractionVars = networkItemBase.InteractionVars;
			itemBase.InteractionConfiguration = networkItemBase.InteractionConfig;
			itemBase.TagIndexes = networkItemBase.TagIndexes;
			bool flag6 = networkItemBase.ItemAppearanceConditions != null;
			if (flag6)
			{
				itemBase.ItemAppearanceConditions = new Dictionary<int, ClientItemAppearanceCondition[]>();
				foreach (KeyValuePair<int, ItemAppearanceCondition[]> keyValuePair in networkItemBase.ItemAppearanceConditions)
				{
					ClientItemAppearanceCondition[] array = new ClientItemAppearanceCondition[keyValuePair.Value.Length];
					for (int k = 0; k < keyValuePair.Value.Length; k++)
					{
						array[k] = ClientItemBaseProtocolInitializer.ParseItemAppearanceCondition(keyValuePair.Value[k], nodeNameManager);
					}
					itemBase.ItemAppearanceConditions.Add(keyValuePair.Key, array);
				}
			}
			itemBase.DisplayEntityStatsHUD = networkItemBase.DisplayEntityStatsHUD;
			itemBase.PullbackConfig = ((networkItemBase.PullbackConfig != null) ? new ClientItemPullbackConfig(networkItemBase.PullbackConfig) : null);
			itemBase.ClipsGeometry = networkItemBase.ClipsGeometry;
		}

		// Token: 0x06005857 RID: 22615 RVA: 0x001AD180 File Offset: 0x001AB380
		public static void ParseLightColor(ColorLight colorLight, ref ColorRgb light)
		{
			light.R = Math.Max((byte)colorLight.Red, (byte)colorLight.Radius);
			light.G = Math.Max((byte)colorLight.Green, (byte)colorLight.Radius);
			light.B = Math.Max((byte)colorLight.Blue, (byte)colorLight.Radius);
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x001AD1DC File Offset: 0x001AB3DC
		public static ClientItemAppearanceCondition ParseItemAppearanceCondition(ItemAppearanceCondition itemAppearanceCondition, NodeNameManager nodeNameManager)
		{
			ClientItemAppearanceCondition clientItemAppearanceCondition = new ClientItemAppearanceCondition();
			bool flag = itemAppearanceCondition.Particles != null;
			if (flag)
			{
				clientItemAppearanceCondition.Particles = new ModelParticleSettings[itemAppearanceCondition.Particles.Length];
				for (int i = 0; i < itemAppearanceCondition.Particles.Length; i++)
				{
					clientItemAppearanceCondition.Particles[i] = new ModelParticleSettings("");
					ParticleProtocolInitializer.Initialize(itemAppearanceCondition.Particles[i], ref clientItemAppearanceCondition.Particles[i], nodeNameManager);
				}
			}
			bool flag2 = itemAppearanceCondition.FirstPersonParticles != null;
			if (flag2)
			{
				clientItemAppearanceCondition.FirstPersonParticles = new ModelParticleSettings[itemAppearanceCondition.FirstPersonParticles.Length];
				for (int j = 0; j < itemAppearanceCondition.FirstPersonParticles.Length; j++)
				{
					clientItemAppearanceCondition.FirstPersonParticles[j] = new ModelParticleSettings("");
					ParticleProtocolInitializer.Initialize(itemAppearanceCondition.FirstPersonParticles[j], ref clientItemAppearanceCondition.FirstPersonParticles[j], nodeNameManager);
				}
			}
			clientItemAppearanceCondition.ModelId = itemAppearanceCondition.Model;
			clientItemAppearanceCondition.Texture = itemAppearanceCondition.Texture;
			clientItemAppearanceCondition.Condition = new FloatRange(itemAppearanceCondition.Condition.InclusiveMin, itemAppearanceCondition.Condition.InclusiveMax);
			bool flag3 = itemAppearanceCondition.ModelVFXId != null;
			if (flag3)
			{
				clientItemAppearanceCondition.ModelVFXId = itemAppearanceCondition.ModelVFXId;
			}
			ValueType conditionValueType = itemAppearanceCondition.ConditionValueType;
			ValueType valueType = conditionValueType;
			if (valueType == null || valueType != 1)
			{
				clientItemAppearanceCondition.Type = 0;
			}
			else
			{
				clientItemAppearanceCondition.Type = 1;
			}
			return clientItemAppearanceCondition;
		}
	}
}
