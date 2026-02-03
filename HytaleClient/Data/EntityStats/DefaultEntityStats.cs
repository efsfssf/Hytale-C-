using System;

namespace HytaleClient.Data.EntityStats
{
	// Token: 0x02000B0A RID: 2826
	public static class DefaultEntityStats
	{
		// Token: 0x06005888 RID: 22664 RVA: 0x001B0364 File Offset: 0x001AE564
		public static void Update(ClientEntityStatType[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				string id = types[i].Id;
				string a = id;
				if (!(a == "Health"))
				{
					if (!(a == "Oxygen"))
					{
						if (!(a == "Stamina"))
						{
							if (!(a == "Mana"))
							{
								if (!(a == "SignatureEnergy"))
								{
									if (a == "Ammo")
									{
										DefaultEntityStats.Ammo = i;
									}
								}
								else
								{
									DefaultEntityStats.SignatureEnergy = i;
								}
							}
							else
							{
								DefaultEntityStats.Mana = i;
							}
						}
						else
						{
							DefaultEntityStats.Stamina = i;
						}
					}
					else
					{
						DefaultEntityStats.Oxygen = i;
					}
				}
				else
				{
					DefaultEntityStats.Health = i;
				}
			}
		}

		// Token: 0x06005889 RID: 22665 RVA: 0x001B041C File Offset: 0x001AE61C
		public static bool IsDefault(int type)
		{
			return type == DefaultEntityStats.Health || type == DefaultEntityStats.Oxygen || type == DefaultEntityStats.Stamina || type == DefaultEntityStats.Mana || type == DefaultEntityStats.SignatureEnergy || type == DefaultEntityStats.Ammo;
		}

		// Token: 0x04003700 RID: 14080
		public static int Health = -1;

		// Token: 0x04003701 RID: 14081
		public static int Oxygen = -1;

		// Token: 0x04003702 RID: 14082
		public static int Stamina = -1;

		// Token: 0x04003703 RID: 14083
		public static int Mana = -1;

		// Token: 0x04003704 RID: 14084
		public static int SignatureEnergy = -1;

		// Token: 0x04003705 RID: 14085
		public static int Ammo = -1;
	}
}
