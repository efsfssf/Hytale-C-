using System;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Weather
{
	// Token: 0x02000ACB RID: 2763
	internal class ClientCloud
	{
		// Token: 0x17001348 RID: 4936
		// (get) Token: 0x0600570E RID: 22286 RVA: 0x001A6799 File Offset: 0x001A4999
		// (set) Token: 0x0600570F RID: 22287 RVA: 0x001A67A1 File Offset: 0x001A49A1
		public Tuple<float, ColorAlpha>[] Colors { get; private set; }

		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x06005710 RID: 22288 RVA: 0x001A67AA File Offset: 0x001A49AA
		// (set) Token: 0x06005711 RID: 22289 RVA: 0x001A67B2 File Offset: 0x001A49B2
		public Tuple<float, float>[] Speeds { get; private set; }

		// Token: 0x06005712 RID: 22290 RVA: 0x001A67BB File Offset: 0x001A49BB
		public ClientCloud()
		{
		}

		// Token: 0x06005713 RID: 22291 RVA: 0x001A67C8 File Offset: 0x001A49C8
		public ClientCloud(Cloud cloud)
		{
			this.Texture = cloud.Texture;
			this.Colors = ClientWeather.ParseTimeDictionary(cloud.Colors, ClientWeather.DefaultWhiteColor);
			this.Speeds = ClientWeather.ParseTimeDictionary(cloud.Speeds, 1f);
		}

		// Token: 0x06005714 RID: 22292 RVA: 0x001A6818 File Offset: 0x001A4A18
		public ClientCloud Clone()
		{
			return new ClientCloud
			{
				Texture = this.Texture,
				Colors = ClientWeather.CloneColorAlphaDictionary(this.Colors),
				Speeds = this.Speeds
			};
		}

		// Token: 0x040034DB RID: 13531
		public const int LayerCount = 4;

		// Token: 0x040034DC RID: 13532
		public string Texture;
	}
}
