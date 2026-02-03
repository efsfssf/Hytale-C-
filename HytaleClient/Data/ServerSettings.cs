using System;
using System.Collections.Generic;
using HytaleClient.Data.Entities;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.EntityUI;
using HytaleClient.Data.Items;
using HytaleClient.Data.Weather;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data
{
	// Token: 0x02000AC9 RID: 2761
	internal class ServerSettings
	{
		// Token: 0x0600570B RID: 22283 RVA: 0x001A6210 File Offset: 0x001A4410
		public int GetServerTag(string tag)
		{
			ServerTags serverTags = this.ServerTags;
			Dictionary<string, int> dictionary = (serverTags != null) ? serverTags.Tags : null;
			bool flag = dictionary == null;
			int result;
			if (flag)
			{
				result = int.MinValue;
			}
			else
			{
				int num;
				result = (dictionary.TryGetValue(tag, out num) ? num : int.MinValue);
			}
			return result;
		}

		// Token: 0x0600570C RID: 22284 RVA: 0x001A6258 File Offset: 0x001A4458
		public ServerSettings Clone()
		{
			ServerSettings serverSettings = new ServerSettings();
			serverSettings.ServerTags = ((this.ServerTags != null) ? new ServerTags(this.ServerTags) : null);
			bool flag = this.Weathers != null;
			if (flag)
			{
				serverSettings.Weathers = new ClientWeather[this.Weathers.Length];
				serverSettings.WeatherIndicesByIds = new Dictionary<string, int>();
				for (int i = 0; i < this.Weathers.Length; i++)
				{
					ClientWeather clientWeather = this.Weathers[i].Clone();
					serverSettings.Weathers[i] = clientWeather;
					serverSettings.WeatherIndicesByIds[clientWeather.Id] = i;
				}
			}
			bool flag2 = this.Environments != null;
			if (flag2)
			{
				serverSettings.Environments = new ClientWorldEnvironment[this.Environments.Length];
				for (int j = 0; j < this.Environments.Length; j++)
				{
					serverSettings.Environments[j] = this.Environments[j].Clone();
				}
			}
			bool flag3 = this.BlockHitboxes != null;
			if (flag3)
			{
				serverSettings.BlockHitboxes = new BlockHitbox[this.BlockHitboxes.Length];
				for (int k = 0; k < this.BlockHitboxes.Length; k++)
				{
					serverSettings.BlockHitboxes[k] = this.BlockHitboxes[k].Clone();
				}
			}
			bool flag4 = this.BlockSoundSets != null;
			if (flag4)
			{
				serverSettings.BlockSoundSets = new BlockSoundSet[this.BlockSoundSets.Length];
				for (int l = 0; l < this.BlockSoundSets.Length; l++)
				{
					serverSettings.BlockSoundSets[l] = new BlockSoundSet(this.BlockSoundSets[l]);
				}
			}
			bool flag5 = this.BlockParticleSets != null;
			if (flag5)
			{
				serverSettings.BlockParticleSets = new Dictionary<string, ClientBlockParticleSet>();
				foreach (KeyValuePair<string, ClientBlockParticleSet> keyValuePair in this.BlockParticleSets)
				{
					serverSettings.BlockParticleSets[keyValuePair.Key] = keyValuePair.Value.Clone();
				}
			}
			bool flag6 = this.FluidFXs != null;
			if (flag6)
			{
				serverSettings.FluidFXs = new FluidFX[this.FluidFXs.Length];
				for (int m = 0; m < this.FluidFXs.Length; m++)
				{
					serverSettings.FluidFXs[m] = new FluidFX(this.FluidFXs[m]);
				}
			}
			bool flag7 = this.UnarmedInteractions != null;
			if (flag7)
			{
				serverSettings.UnarmedInteractions = new Dictionary<InteractionType, int>();
				foreach (KeyValuePair<InteractionType, int> keyValuePair2 in this.UnarmedInteractions)
				{
					serverSettings.UnarmedInteractions[keyValuePair2.Key] = keyValuePair2.Value;
				}
			}
			bool flag8 = this.EntityStatTypes != null;
			if (flag8)
			{
				serverSettings.EntityStatTypes = new ClientEntityStatType[this.EntityStatTypes.Length];
				for (int n = 0; n < this.EntityStatTypes.Length; n++)
				{
					ClientEntityStatType clientEntityStatType = this.EntityStatTypes[n];
					serverSettings.EntityStatTypes[n] = clientEntityStatType;
				}
			}
			bool flag9 = this.ItemQualities != null;
			if (flag9)
			{
				serverSettings.ItemQualities = new ClientItemQuality[this.ItemQualities.Length];
				for (int num = 0; num < this.ItemQualities.Length; num++)
				{
					serverSettings.ItemQualities[num] = this.ItemQualities[num].Clone();
				}
			}
			bool flag10 = this.ItemReticleConfigs != null;
			if (flag10)
			{
				serverSettings.ItemReticleConfigs = new ClientItemReticleConfig[this.ItemReticleConfigs.Length];
				for (int num2 = 0; num2 < this.ItemReticleConfigs.Length; num2++)
				{
					serverSettings.ItemReticleConfigs[num2] = this.ItemReticleConfigs[num2].Clone();
				}
			}
			bool flag11 = this.HitboxCollisionConfigs != null;
			if (flag11)
			{
				serverSettings.HitboxCollisionConfigs = new ClientHitboxCollisionConfig[this.HitboxCollisionConfigs.Length];
				for (int num3 = 0; num3 < this.HitboxCollisionConfigs.Length; num3++)
				{
					serverSettings.HitboxCollisionConfigs[num3] = this.HitboxCollisionConfigs[num3].Clone();
				}
			}
			bool flag12 = this.EntityUIComponents != null;
			if (flag12)
			{
				serverSettings.EntityUIComponents = new ClientEntityUIComponent[this.EntityUIComponents.Length];
				for (int num4 = 0; num4 < this.EntityUIComponents.Length; num4++)
				{
					serverSettings.EntityUIComponents[num4] = this.EntityUIComponents[num4].Clone();
				}
			}
			bool flag13 = this.RepulsionConfigs != null;
			if (flag13)
			{
				serverSettings.RepulsionConfigs = new ClientRepulsionConfig[this.RepulsionConfigs.Length];
				for (int num5 = 0; num5 < this.RepulsionConfigs.Length; num5++)
				{
					serverSettings.RepulsionConfigs[num5] = this.RepulsionConfigs[num5].Clone();
				}
			}
			bool flag14 = this.BlockGroups != null;
			if (flag14)
			{
				serverSettings.BlockGroups = new Dictionary<string, BlockGroup>(this.BlockGroups);
			}
			return serverSettings;
		}

		// Token: 0x040034C1 RID: 13505
		public const int EmptyBlockSoundId = 0;

		// Token: 0x040034C2 RID: 13506
		public const int AirFluidFXId = 0;

		// Token: 0x040034C3 RID: 13507
		public const int EmptyFluidFXId = 0;

		// Token: 0x040034C4 RID: 13508
		public const int UnknownEnvironmentId = 0;

		// Token: 0x040034C5 RID: 13509
		public const int UnknownWeatherId = 0;

		// Token: 0x040034C6 RID: 13510
		public const int UnknownTag = -2147483648;

		// Token: 0x040034C7 RID: 13511
		public Dictionary<string, int> WeatherIndicesByIds;

		// Token: 0x040034C8 RID: 13512
		public ClientWeather[] Weathers;

		// Token: 0x040034C9 RID: 13513
		public ClientWorldEnvironment[] Environments;

		// Token: 0x040034CA RID: 13514
		public BlockHitbox[] BlockHitboxes;

		// Token: 0x040034CB RID: 13515
		public BlockSoundSet[] BlockSoundSets;

		// Token: 0x040034CC RID: 13516
		public Dictionary<string, ClientBlockParticleSet> BlockParticleSets;

		// Token: 0x040034CD RID: 13517
		public FluidFX[] FluidFXs;

		// Token: 0x040034CE RID: 13518
		public Dictionary<InteractionType, int> UnarmedInteractions;

		// Token: 0x040034CF RID: 13519
		public ClientEntityStatType[] EntityStatTypes;

		// Token: 0x040034D0 RID: 13520
		public ClientItemQuality[] ItemQualities;

		// Token: 0x040034D1 RID: 13521
		public ClientItemReticleConfig[] ItemReticleConfigs;

		// Token: 0x040034D2 RID: 13522
		public ClientHitboxCollisionConfig[] HitboxCollisionConfigs;

		// Token: 0x040034D3 RID: 13523
		public ClientRepulsionConfig[] RepulsionConfigs;

		// Token: 0x040034D4 RID: 13524
		public ClientEntityUIComponent[] EntityUIComponents;

		// Token: 0x040034D5 RID: 13525
		public Dictionary<string, BlockGroup> BlockGroups;

		// Token: 0x040034D6 RID: 13526
		public ServerTags ServerTags;
	}
}
