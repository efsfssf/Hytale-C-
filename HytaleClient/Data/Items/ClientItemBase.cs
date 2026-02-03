using System;
using System.Collections.Generic;
using Coherent.UI.Binding;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame.Modules.BuilderTools.Tools;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AF2 RID: 2802
	[CoherentType]
	internal class ClientItemBase
	{
		// Token: 0x06005838 RID: 22584 RVA: 0x001AC24C File Offset: 0x001AA44C
		public EntityAnimation GetAnimation(string id)
		{
			EntityAnimation entityAnimation;
			bool flag = !this.PlayerAnimations.Animations.TryGetValue(id, out entityAnimation);
			EntityAnimation result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = entityAnimation;
			}
			return result;
		}

		// Token: 0x06005839 RID: 22585 RVA: 0x001AC280 File Offset: 0x001AA480
		public bool ShouldDisplayHudForEntityStat(int entityStatIndex)
		{
			bool flag = this.DisplayEntityStatsHUD == null;
			return !flag && Array.IndexOf<int>(this.DisplayEntityStatsHUD, entityStatIndex) >= 0;
		}

		// Token: 0x04003685 RID: 13957
		public const string EmptyItemName = "Empty";

		// Token: 0x04003686 RID: 13958
		public const string UnknownItemName = "Unknown";

		// Token: 0x04003687 RID: 13959
		[CoherentProperty("id")]
		public string Id;

		// Token: 0x04003688 RID: 13960
		[CoherentProperty("categories")]
		public string[] Categories;

		// Token: 0x04003689 RID: 13961
		[CoherentProperty("set")]
		public string Set;

		// Token: 0x0400368A RID: 13962
		[CoherentProperty("qualityIndex")]
		public int QualityIndex;

		// Token: 0x0400368B RID: 13963
		[CoherentProperty("itemLevel")]
		public int ItemLevel;

		// Token: 0x0400368C RID: 13964
		[CoherentProperty("recipe")]
		public ClientItemCraftingRecipe Recipe;

		// Token: 0x0400368D RID: 13965
		[CoherentProperty("resourceTypes")]
		public ClientItemResourceType[] ResourceTypes;

		// Token: 0x0400368E RID: 13966
		public uint SoundEventIndex;

		// Token: 0x0400368F RID: 13967
		public ModelParticleSettings[] Particles;

		// Token: 0x04003690 RID: 13968
		public ModelParticleSettings[] FirstPersonParticles;

		// Token: 0x04003691 RID: 13969
		public ModelTrail[] Trails;

		// Token: 0x04003692 RID: 13970
		public ColorRgb LightEmitted;

		// Token: 0x04003693 RID: 13971
		public BlockyModel Model;

		// Token: 0x04003694 RID: 13972
		public float Scale;

		// Token: 0x04003695 RID: 13973
		public string Texture;

		// Token: 0x04003696 RID: 13974
		public BlockyAnimation Animation;

		// Token: 0x04003697 RID: 13975
		public string PlayerAnimationsId;

		// Token: 0x04003698 RID: 13976
		public ClientItemPlayerAnimations PlayerAnimations;

		// Token: 0x04003699 RID: 13977
		public bool UsePlayerAnimations;

		// Token: 0x0400369A RID: 13978
		public bool Consumable;

		// Token: 0x0400369B RID: 13979
		public int BlockId;

		// Token: 0x0400369C RID: 13980
		public ItemBase.ItemUtility Utility;

		// Token: 0x0400369D RID: 13981
		[CoherentProperty("tool")]
		public ItemBase.ItemTool Tool;

		// Token: 0x0400369E RID: 13982
		[CoherentProperty("weapon")]
		public ItemBase.ItemWeapon Weapon;

		// Token: 0x0400369F RID: 13983
		public BuilderTool BuilderTool;

		// Token: 0x040036A0 RID: 13984
		[CoherentProperty("armor")]
		public ClientItemArmor Armor;

		// Token: 0x040036A1 RID: 13985
		[CoherentProperty("iconProperties")]
		public ClientItemIconProperties IconProperties;

		// Token: 0x040036A2 RID: 13986
		[CoherentProperty("maxStack")]
		public int MaxStack;

		// Token: 0x040036A3 RID: 13987
		[CoherentProperty("icon")]
		public string Icon;

		// Token: 0x040036A4 RID: 13988
		public int ReticleIndex;

		// Token: 0x040036A5 RID: 13989
		[CoherentProperty("durability")]
		public double Durability;

		// Token: 0x040036A6 RID: 13990
		public ItemBase.ItemEntityConfig ItemEntity;

		// Token: 0x040036A7 RID: 13991
		public Dictionary<InteractionType, int> Interactions;

		// Token: 0x040036A8 RID: 13992
		public Dictionary<string, int> InteractionVars;

		// Token: 0x040036A9 RID: 13993
		public ItemBase.InteractionConfiguration InteractionConfiguration;

		// Token: 0x040036AA RID: 13994
		public Dictionary<int, ClientItemAppearanceCondition[]> ItemAppearanceConditions;

		// Token: 0x040036AB RID: 13995
		public BlockyAnimation DroppedItemAnimation;

		// Token: 0x040036AC RID: 13996
		public int[] TagIndexes;

		// Token: 0x040036AD RID: 13997
		public int[] DisplayEntityStatsHUD;

		// Token: 0x040036AE RID: 13998
		public ClientItemPullbackConfig PullbackConfig;

		// Token: 0x040036AF RID: 13999
		public bool ClipsGeometry;
	}
}
