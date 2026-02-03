using System;
using System.Collections.Generic;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Data
{
	// Token: 0x02000AC6 RID: 2758
	internal class ClientAnimationSet
	{
		// Token: 0x060056FC RID: 22268 RVA: 0x001A56EA File Offset: 0x001A38EA
		public ClientAnimationSet(string id)
		{
			this.Id = id;
		}

		// Token: 0x060056FD RID: 22269 RVA: 0x001A5706 File Offset: 0x001A3906
		public ClientAnimationSet(string id, ClientAnimationSet clientAnimationSet)
		{
			this.Id = id;
			this.Animations = clientAnimationSet.Animations;
			this.PassiveNextDelay = clientAnimationSet.PassiveNextDelay;
			this.WeightSum = clientAnimationSet.WeightSum;
		}

		// Token: 0x060056FE RID: 22270 RVA: 0x001A5748 File Offset: 0x001A3948
		public EntityAnimation GetWeightedAnimation(int seed)
		{
			bool flag = this.Animations.Count == 0;
			EntityAnimation result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = this.Animations.Count == 1;
				if (flag2)
				{
					result = this.Animations[0];
				}
				else
				{
					bool flag3 = this.WeightSum == 0f;
					if (flag3)
					{
						result = this.Animations[seed % this.Animations.Count];
					}
					else
					{
						float num = StaticRandom.NextFloat((long)seed, 0f, this.WeightSum);
						EntityAnimation entityAnimation = null;
						foreach (EntityAnimation entityAnimation2 in this.Animations)
						{
							entityAnimation = entityAnimation2;
							num -= entityAnimation2.Weight;
							bool flag4 = num <= 0f;
							if (flag4)
							{
								break;
							}
						}
						result = entityAnimation;
					}
				}
			}
			return result;
		}

		// Token: 0x04003482 RID: 13442
		public readonly string Id;

		// Token: 0x04003483 RID: 13443
		public readonly List<EntityAnimation> Animations = new List<EntityAnimation>();

		// Token: 0x04003484 RID: 13444
		public Rangef PassiveNextDelay;

		// Token: 0x04003485 RID: 13445
		public float WeightSum;
	}
}
