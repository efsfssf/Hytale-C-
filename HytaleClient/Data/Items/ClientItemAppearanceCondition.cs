using System;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.EntityStats;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AF0 RID: 2800
	internal class ClientItemAppearanceCondition
	{
		// Token: 0x06005835 RID: 22581 RVA: 0x001AC1B4 File Offset: 0x001AA3B4
		public bool CanApplyCondition(ClientEntityStatValue entityStat)
		{
			ValueType type = this.Type;
			ValueType valueType = type;
			float value;
			if (valueType != null)
			{
				if (valueType != 1)
				{
				}
				value = entityStat.Value;
			}
			else
			{
				value = entityStat.Value / entityStat.Max * 100f;
			}
			return this.Condition.Includes(value);
		}

		// Token: 0x0400367B RID: 13947
		public ModelParticleSettings[] Particles;

		// Token: 0x0400367C RID: 13948
		public ModelParticleSettings[] FirstPersonParticles;

		// Token: 0x0400367D RID: 13949
		public string ModelId;

		// Token: 0x0400367E RID: 13950
		public BlockyModel Model;

		// Token: 0x0400367F RID: 13951
		public string Texture;

		// Token: 0x04003680 RID: 13952
		public FloatRange Condition;

		// Token: 0x04003681 RID: 13953
		public ValueType Type;

		// Token: 0x04003682 RID: 13954
		public string ModelVFXId;

		// Token: 0x02000F1F RID: 3871
		public class Data
		{
			// Token: 0x06006838 RID: 26680 RVA: 0x0021A684 File Offset: 0x00218884
			public Data(int entityStatIndex, int conditionIndex)
			{
				this.EntityStatIndex = entityStatIndex;
				this.ConditionIndex = conditionIndex;
			}

			// Token: 0x04004A2C RID: 18988
			public int EntityStatIndex;

			// Token: 0x04004A2D RID: 18989
			public int ConditionIndex;

			// Token: 0x04004A2E RID: 18990
			public Entity.EntityParticle[] EntityParticles;
		}
	}
}
