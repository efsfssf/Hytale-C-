using System;
using System.Collections.Generic;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000ACF RID: 2767
	internal class AudioSettings
	{
		// Token: 0x0600575D RID: 22365 RVA: 0x001A791C File Offset: 0x001A5B1C
		public void Initialize()
		{
			foreach (string text in Enum.GetNames(typeof(AudioSettings.SoundCategory)))
			{
				bool flag = !this.CategoryVolumes.ContainsKey(text);
				if (flag)
				{
					AudioSettings.SoundCategory soundCategory;
					float value = (Enum.TryParse<AudioSettings.SoundCategory>(text, out soundCategory) && soundCategory == AudioSettings.SoundCategory.MusicVolume) ? 70f : 100f;
					this.CategoryVolumes[text] = value;
				}
			}
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x001A7990 File Offset: 0x001A5B90
		public AudioSettings Clone()
		{
			return new AudioSettings
			{
				OutputDeviceId = this.OutputDeviceId,
				MasterVolume = this.MasterVolume,
				CategoryVolumes = new Dictionary<string, float>(this.CategoryVolumes)
			};
		}

		// Token: 0x0600575F RID: 22367 RVA: 0x001A79D4 File Offset: 0x001A5BD4
		internal string[] GetCategoryRTPCsArray()
		{
			string[] names = Enum.GetNames(typeof(AudioSettings.SoundCategory));
			string[] array = new string[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				array[i] = names[i].ToUpper();
			}
			return array;
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x001A7A20 File Offset: 0x001A5C20
		internal float[] GetCategoryVolumesArray()
		{
			string[] names = Enum.GetNames(typeof(AudioSettings.SoundCategory));
			float[] array = new float[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				array[i] = this.CategoryVolumes[names[i]];
			}
			return array;
		}

		// Token: 0x0400350F RID: 13583
		public const string MasterVolumeRTPCName = "MASTERVOLUME";

		// Token: 0x04003510 RID: 13584
		public uint OutputDeviceId;

		// Token: 0x04003511 RID: 13585
		public float MasterVolume = 100f;

		// Token: 0x04003512 RID: 13586
		public Dictionary<string, float> CategoryVolumes = new Dictionary<string, float>();

		// Token: 0x02000F18 RID: 3864
		public enum SoundCategory
		{
			// Token: 0x04004A0D RID: 18957
			MusicVolume,
			// Token: 0x04004A0E RID: 18958
			AmbienceVolume,
			// Token: 0x04004A0F RID: 18959
			SFXVolume,
			// Token: 0x04004A10 RID: 18960
			UIVolume
		}
	}
}
