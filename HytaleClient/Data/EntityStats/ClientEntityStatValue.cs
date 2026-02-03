using System;
using System.Collections.Generic;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.EntityStats
{
	// Token: 0x02000B09 RID: 2825
	public class ClientEntityStatValue
	{
		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x06005882 RID: 22658 RVA: 0x001B014E File Offset: 0x001AE34E
		// (set) Token: 0x06005883 RID: 22659 RVA: 0x001B0156 File Offset: 0x001AE356
		public float Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = MathHelper.Clamp(value, this.Min, this.Max);
			}
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x001B0170 File Offset: 0x001AE370
		public float AsPercentage()
		{
			return (this.Value - this.Min) / (this.Max - this.Min);
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x001B01A0 File Offset: 0x001AE3A0
		public void CalculateModifiers(ClientEntityStatType statType)
		{
			this.Min = statType.Min;
			this.Max = statType.Max;
			bool flag = this.Modifiers != null;
			if (flag)
			{
				for (int i = 0; i < ClientEntityStatValue.TARGETS.Length; i++)
				{
					Modifier.ModifierTarget modifierTarget = ClientEntityStatValue.TARGETS[i];
					bool flag2 = false;
					float num = 0f;
					bool flag3 = false;
					float num2 = 0f;
					foreach (Modifier modifier in this.Modifiers.Values)
					{
						bool flag4 = modifier.Target != modifierTarget;
						if (!flag4)
						{
							Modifier.CalculationType calculationType_ = modifier.CalculationType_;
							Modifier.CalculationType calculationType = calculationType_;
							if (calculationType != null)
							{
								if (calculationType != 1)
								{
									throw new ArgumentOutOfRangeException();
								}
								flag3 = true;
								num2 += modifier.Amount;
							}
							else
							{
								flag2 = true;
								num += modifier.Amount;
							}
						}
					}
					Modifier.ModifierTarget modifierTarget2 = modifierTarget;
					Modifier.ModifierTarget modifierTarget3 = modifierTarget2;
					if (modifierTarget3 != null)
					{
						if (modifierTarget3 != 1)
						{
							throw new ArgumentOutOfRangeException();
						}
						bool flag5 = flag2;
						if (flag5)
						{
							this.Max += num;
						}
						bool flag6 = flag3;
						if (flag6)
						{
							this.Max *= num2;
						}
					}
					else
					{
						bool flag7 = flag2;
						if (flag7)
						{
							this.Min += num;
						}
						bool flag8 = flag3;
						if (flag8)
						{
							this.Min *= num2;
						}
					}
				}
			}
			this._value = MathHelper.Clamp(this._value, this.Min, this.Max);
		}

		// Token: 0x040036FB RID: 14075
		private static Modifier.ModifierTarget[] TARGETS = new Modifier.ModifierTarget[]
		{
			default(Modifier.ModifierTarget),
			1
		};

		// Token: 0x040036FC RID: 14076
		private float _value;

		// Token: 0x040036FD RID: 14077
		public float Min;

		// Token: 0x040036FE RID: 14078
		public float Max;

		// Token: 0x040036FF RID: 14079
		public Dictionary<string, Modifier> Modifiers;
	}
}
