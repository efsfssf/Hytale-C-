using System;
using System.Collections.Generic;
using HytaleClient.Audio;
using HytaleClient.InGame.Modules.AmbienceFX;
using HytaleClient.Protocol;

namespace HytaleClient.Data.AmbienceFX
{
	// Token: 0x02000B77 RID: 2935
	internal class AmbienceFXProtocolInitializer
	{
		// Token: 0x06005A14 RID: 23060 RVA: 0x001BF9F8 File Offset: 0x001BDBF8
		public static void Initialize(AmbienceFX networkAmbienceFX, ref AmbienceFXSettings clientAmbienceFX, List<AmbienceFXSoundSettings> validSoundSettings)
		{
			clientAmbienceFX.Id = networkAmbienceFX.Id;
			bool flag = networkAmbienceFX.Conditions != null;
			if (flag)
			{
				clientAmbienceFX.Conditions = new AmbienceFXConditionSettings();
				AmbienceFXProtocolInitializer.Initialize(networkAmbienceFX.Conditions, ref clientAmbienceFX.Conditions);
			}
			bool flag2 = networkAmbienceFX.Sounds != null;
			if (flag2)
			{
				for (int i = 0; i < networkAmbienceFX.Sounds.Length; i++)
				{
					validSoundSettings.Clear();
					AmbienceFXSoundSettings ambienceFXSoundSettings = new AmbienceFXSoundSettings();
					AmbienceFXProtocolInitializer.Initialize(networkAmbienceFX.Sounds[i], ref ambienceFXSoundSettings);
					bool flag3 = ambienceFXSoundSettings.SoundEventIndex > 0U;
					if (flag3)
					{
						validSoundSettings.Add(ambienceFXSoundSettings);
					}
				}
				bool flag4 = validSoundSettings.Count != 0;
				if (flag4)
				{
					clientAmbienceFX.Sounds = validSoundSettings.ToArray();
				}
			}
			clientAmbienceFX.MusicSoundEventIndex = ResourceManager.GetNetworkWwiseId(networkAmbienceFX.MusicSoundEventIndex);
			clientAmbienceFX.AmbientBedSoundEventIndex = ResourceManager.GetNetworkWwiseId(networkAmbienceFX.AmbientBedSoundEventIndex);
			clientAmbienceFX.EffectSoundEventIndex = ResourceManager.GetNetworkWwiseId(networkAmbienceFX.EffectSoundEventIndex);
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x001BFAF4 File Offset: 0x001BDCF4
		public static void Initialize(AmbienceFXConditions networkAmbienceFXConditions, ref AmbienceFXConditionSettings clientAmbienceFXConditions)
		{
			clientAmbienceFXConditions.EnvironmentIndices = networkAmbienceFXConditions.EnvironmentIndices;
			clientAmbienceFXConditions.WeatherIndices = networkAmbienceFXConditions.WeatherIndices;
			clientAmbienceFXConditions.FluidFXIndices = networkAmbienceFXConditions.FluidFXIndices;
			bool flag = networkAmbienceFXConditions.SurroundingBlockSoundSets != null;
			if (flag)
			{
				clientAmbienceFXConditions.SurroundingBlockSoundSets = new AmbienceFXConditionSettings.AmbienceFXBlockSoundSet[networkAmbienceFXConditions.SurroundingBlockSoundSets.Length];
				for (int i = 0; i < networkAmbienceFXConditions.SurroundingBlockSoundSets.Length; i++)
				{
					clientAmbienceFXConditions.SurroundingBlockSoundSets[i] = default(AmbienceFXConditionSettings.AmbienceFXBlockSoundSet);
					AmbienceFXProtocolInitializer.Initialize(networkAmbienceFXConditions.SurroundingBlockSoundSets[i], ref clientAmbienceFXConditions.SurroundingBlockSoundSets[i]);
				}
			}
			clientAmbienceFXConditions.Altitude = new Range(networkAmbienceFXConditions.Altitude.Min, networkAmbienceFXConditions.Altitude.Max);
			clientAmbienceFXConditions.Walls = new Range((int)networkAmbienceFXConditions.Walls.Min, (int)networkAmbienceFXConditions.Walls.Max);
			clientAmbienceFXConditions.Roof = networkAmbienceFXConditions.Roof;
			clientAmbienceFXConditions.Floor = networkAmbienceFXConditions.Floor;
			clientAmbienceFXConditions.SunLightLevel = new Range((int)networkAmbienceFXConditions.SunLightLevel.Min, (int)networkAmbienceFXConditions.SunLightLevel.Max);
			clientAmbienceFXConditions.TorchLightLevel = new Range((int)networkAmbienceFXConditions.TorchLightLevel.Min, (int)networkAmbienceFXConditions.TorchLightLevel.Max);
			clientAmbienceFXConditions.GlobalLightLevel = new Range((int)networkAmbienceFXConditions.GlobalLightLevel.Min, (int)networkAmbienceFXConditions.GlobalLightLevel.Max);
			clientAmbienceFXConditions.DayTime = new Rangef(networkAmbienceFXConditions.DayTime.Min, networkAmbienceFXConditions.DayTime.Max);
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x001BFC79 File Offset: 0x001BDE79
		public static void Initialize(AmbienceFXBlockSoundSet networkAmbienceFXBlockSoundSet, ref AmbienceFXConditionSettings.AmbienceFXBlockSoundSet clientAmbienceFXBlockSoundSet)
		{
			clientAmbienceFXBlockSoundSet.BlockSoundSetIndex = networkAmbienceFXBlockSoundSet.BlockSoundSetIndex;
			clientAmbienceFXBlockSoundSet.Percent = new Rangef(networkAmbienceFXBlockSoundSet.Percent.Min, networkAmbienceFXBlockSoundSet.Percent.Max);
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x001BFCAC File Offset: 0x001BDEAC
		public static void Initialize(AmbienceFXSound networkAmbienceFXSound, ref AmbienceFXSoundSettings clientAmbienceFXSound)
		{
			clientAmbienceFXSound.SoundEventIndex = ResourceManager.GetNetworkWwiseId(networkAmbienceFXSound.SoundEventIndex);
			clientAmbienceFXSound.Play3D = networkAmbienceFXSound.Play3D;
			clientAmbienceFXSound.BlockSoundSetIndex = networkAmbienceFXSound.BlockSoundSetIndex;
			clientAmbienceFXSound.Altitude = networkAmbienceFXSound.Altitude;
			clientAmbienceFXSound.Frequency = new Rangef(networkAmbienceFXSound.Frequency.Min, networkAmbienceFXSound.Frequency.Max);
			clientAmbienceFXSound.Radius = new Range(networkAmbienceFXSound.Radius.Min, networkAmbienceFXSound.Radius.Max);
		}
	}
}
