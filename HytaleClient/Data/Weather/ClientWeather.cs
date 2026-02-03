using System;
using System.Collections.Generic;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Weather
{
	// Token: 0x02000ACC RID: 2764
	internal class ClientWeather
	{
		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x06005715 RID: 22293 RVA: 0x001A685C File Offset: 0x001A4A5C
		// (set) Token: 0x06005716 RID: 22294 RVA: 0x001A6864 File Offset: 0x001A4A64
		public string Id { get; private set; }

		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x06005717 RID: 22295 RVA: 0x001A686D File Offset: 0x001A4A6D
		// (set) Token: 0x06005718 RID: 22296 RVA: 0x001A6875 File Offset: 0x001A4A75
		public Weather.WeatherParticle Particle { get; private set; }

		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x06005719 RID: 22297 RVA: 0x001A687E File Offset: 0x001A4A7E
		// (set) Token: 0x0600571A RID: 22298 RVA: 0x001A6886 File Offset: 0x001A4A86
		public string Stars { get; private set; }

		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x0600571B RID: 22299 RVA: 0x001A688F File Offset: 0x001A4A8F
		// (set) Token: 0x0600571C RID: 22300 RVA: 0x001A6897 File Offset: 0x001A4A97
		public Tuple<float, Color>[] SunlightColors { get; private set; }

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x0600571D RID: 22301 RVA: 0x001A68A0 File Offset: 0x001A4AA0
		// (set) Token: 0x0600571E RID: 22302 RVA: 0x001A68A8 File Offset: 0x001A4AA8
		public Tuple<float, ColorAlpha>[] SkyTopColors { get; private set; }

		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x0600571F RID: 22303 RVA: 0x001A68B1 File Offset: 0x001A4AB1
		// (set) Token: 0x06005720 RID: 22304 RVA: 0x001A68B9 File Offset: 0x001A4AB9
		public Tuple<float, ColorAlpha>[] SkyBottomColors { get; private set; }

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x06005721 RID: 22305 RVA: 0x001A68C2 File Offset: 0x001A4AC2
		// (set) Token: 0x06005722 RID: 22306 RVA: 0x001A68CA File Offset: 0x001A4ACA
		public Tuple<float, ColorAlpha>[] SkySunsetColors { get; private set; }

		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x06005723 RID: 22307 RVA: 0x001A68D3 File Offset: 0x001A4AD3
		// (set) Token: 0x06005724 RID: 22308 RVA: 0x001A68DB File Offset: 0x001A4ADB
		public Tuple<float, Color>[] WaterTints { get; private set; }

		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x06005725 RID: 22309 RVA: 0x001A68E4 File Offset: 0x001A4AE4
		// (set) Token: 0x06005726 RID: 22310 RVA: 0x001A68EC File Offset: 0x001A4AEC
		public Tuple<float, float>[] SunlightDampingMultipliers { get; private set; }

		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x06005727 RID: 22311 RVA: 0x001A68F5 File Offset: 0x001A4AF5
		// (set) Token: 0x06005728 RID: 22312 RVA: 0x001A68FD File Offset: 0x001A4AFD
		public Tuple<float, Color>[] SunColors { get; private set; }

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x06005729 RID: 22313 RVA: 0x001A6906 File Offset: 0x001A4B06
		// (set) Token: 0x0600572A RID: 22314 RVA: 0x001A690E File Offset: 0x001A4B0E
		public Tuple<float, float>[] SunScales { get; private set; }

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x0600572B RID: 22315 RVA: 0x001A6917 File Offset: 0x001A4B17
		// (set) Token: 0x0600572C RID: 22316 RVA: 0x001A691F File Offset: 0x001A4B1F
		public Tuple<float, ColorAlpha>[] SunGlowColors { get; private set; }

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x0600572D RID: 22317 RVA: 0x001A6928 File Offset: 0x001A4B28
		// (set) Token: 0x0600572E RID: 22318 RVA: 0x001A6930 File Offset: 0x001A4B30
		public Dictionary<int, string> Moons { get; private set; }

		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x0600572F RID: 22319 RVA: 0x001A6939 File Offset: 0x001A4B39
		// (set) Token: 0x06005730 RID: 22320 RVA: 0x001A6941 File Offset: 0x001A4B41
		public Tuple<float, ColorAlpha>[] MoonColors { get; private set; }

		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x06005731 RID: 22321 RVA: 0x001A694A File Offset: 0x001A4B4A
		// (set) Token: 0x06005732 RID: 22322 RVA: 0x001A6952 File Offset: 0x001A4B52
		public Tuple<float, float>[] MoonScales { get; private set; }

		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x06005733 RID: 22323 RVA: 0x001A695B File Offset: 0x001A4B5B
		// (set) Token: 0x06005734 RID: 22324 RVA: 0x001A6963 File Offset: 0x001A4B63
		public Tuple<float, ColorAlpha>[] MoonGlowColors { get; private set; }

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x06005735 RID: 22325 RVA: 0x001A696C File Offset: 0x001A4B6C
		// (set) Token: 0x06005736 RID: 22326 RVA: 0x001A6974 File Offset: 0x001A4B74
		public string ScreenEffect { get; private set; }

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x06005737 RID: 22327 RVA: 0x001A697D File Offset: 0x001A4B7D
		// (set) Token: 0x06005738 RID: 22328 RVA: 0x001A6985 File Offset: 0x001A4B85
		public Tuple<float, ColorAlpha>[] ScreenEffectColors { get; private set; }

		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x06005739 RID: 22329 RVA: 0x001A698E File Offset: 0x001A4B8E
		// (set) Token: 0x0600573A RID: 22330 RVA: 0x001A6996 File Offset: 0x001A4B96
		public NearFar Fog { get; private set; }

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x0600573B RID: 22331 RVA: 0x001A699F File Offset: 0x001A4B9F
		// (set) Token: 0x0600573C RID: 22332 RVA: 0x001A69A7 File Offset: 0x001A4BA7
		public Tuple<float, Color>[] FogColors { get; private set; }

		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x0600573D RID: 22333 RVA: 0x001A69B0 File Offset: 0x001A4BB0
		// (set) Token: 0x0600573E RID: 22334 RVA: 0x001A69B8 File Offset: 0x001A4BB8
		public Tuple<float, float>[] FogHeightFalloffs { get; private set; }

		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x0600573F RID: 22335 RVA: 0x001A69C1 File Offset: 0x001A4BC1
		// (set) Token: 0x06005740 RID: 22336 RVA: 0x001A69C9 File Offset: 0x001A4BC9
		public Tuple<float, float>[] FogDensities { get; private set; }

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x06005741 RID: 22337 RVA: 0x001A69D2 File Offset: 0x001A4BD2
		// (set) Token: 0x06005742 RID: 22338 RVA: 0x001A69DA File Offset: 0x001A4BDA
		public Tuple<float, Color>[] ColorFilters { get; private set; }

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x06005743 RID: 22339 RVA: 0x001A69E3 File Offset: 0x001A4BE3
		// (set) Token: 0x06005744 RID: 22340 RVA: 0x001A69EB File Offset: 0x001A4BEB
		public ClientCloud[] Clouds { get; private set; }

		// Token: 0x06005745 RID: 22341 RVA: 0x001A69F4 File Offset: 0x001A4BF4
		public ClientWeather()
		{
		}

		// Token: 0x06005746 RID: 22342 RVA: 0x001A6A00 File Offset: 0x001A4C00
		public ClientWeather(Weather weather)
		{
			this.Id = weather.Id;
			bool flag = weather.Particle != null;
			if (flag)
			{
				this.Particle = weather.Particle;
			}
			this.Stars = weather.Stars;
			this.SunlightColors = ClientWeather.ParseTimeDictionary(weather.SunlightColors, ClientWeather.DefaultWhiteColor);
			this.SkyTopColors = ClientWeather.ParseTimeDictionary(weather.SkyTopColors, ClientWeather.DefaultWhiteColor);
			this.SkyBottomColors = ClientWeather.ParseTimeDictionary(weather.SkyBottomColors, ClientWeather.DefaultWhiteColor);
			this.SkySunsetColors = ClientWeather.ParseTimeDictionary(weather.SkySunsetColors, ClientWeather.DefaultWhiteColor);
			bool flag2 = weather.WaterTints != null;
			if (flag2)
			{
				this.WaterTints = ClientWeather.ParseTimeDictionary(weather.WaterTints, ClientWeather.DefaultWhiteColor);
			}
			else
			{
				this.WaterTints = ClientWeather.CloneDownColorAlphaDictionary(this.SkyTopColors);
			}
			this.SunlightDampingMultipliers = ClientWeather.ParseTimeDictionary(weather.SunlightDampingMultiplier, 1f);
			this.SunColors = ClientWeather.ParseTimeDictionary(weather.SunColors, ClientWeather.DefaultWhiteColor);
			this.SunScales = ClientWeather.ParseTimeDictionary(weather.SunScales, 1f);
			this.SunGlowColors = ClientWeather.ParseTimeDictionary(weather.SunGlowColors, ClientWeather.DefaultTransparentColor);
			this.Moons = ((weather.Moons != null) ? weather.Moons : new Dictionary<int, string>());
			this.MoonColors = ClientWeather.ParseTimeDictionary(weather.MoonColors, ClientWeather.DefaultWhiteColor);
			this.MoonScales = ClientWeather.ParseTimeDictionary(weather.MoonScales, 1f);
			this.MoonGlowColors = ClientWeather.ParseTimeDictionary(weather.MoonGlowColors, ClientWeather.DefaultTransparentColor);
			this.ScreenEffect = weather.ScreenEffect;
			this.ScreenEffectColors = ClientWeather.ParseTimeDictionary(weather.ScreenEffectColors, ClientWeather.DefaultWhiteColor);
			this.Fog = weather.Fog;
			this.FogColors = ClientWeather.ParseTimeDictionary(weather.FogColors, ClientWeather.DefaultWhiteColor);
			this.ColorFilters = ClientWeather.ParseTimeDictionary(weather.ColorFilters, ClientWeather.DefaultWhiteColor);
			this.FogHeightFalloffs = ClientWeather.ParseTimeDictionary(weather.FogHeightFalloffs, 10f);
			this.FogDensities = ClientWeather.ParseTimeDictionary(weather.FogDensities, 0f);
			this.Clouds = new ClientCloud[4];
			for (int i = 0; i < 4; i++)
			{
				Cloud cloud = (weather.Clouds != null && i < weather.Clouds.Length) ? weather.Clouds[i] : new Cloud();
				this.Clouds[i] = new ClientCloud(cloud);
			}
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x001A6C7C File Offset: 0x001A4E7C
		public ClientWeather Clone()
		{
			ClientWeather clientWeather = new ClientWeather();
			clientWeather.Id = this.Id;
			bool flag = this.Particle != null;
			if (flag)
			{
				clientWeather.Particle = new Weather.WeatherParticle(this.Particle);
			}
			clientWeather.Stars = this.Stars;
			clientWeather.SunlightDampingMultipliers = this.SunlightDampingMultipliers;
			clientWeather.SunlightColors = ClientWeather.CloneColorDictionary(this.SunlightColors);
			clientWeather.SkyTopColors = ClientWeather.CloneColorAlphaDictionary(this.SkyTopColors);
			clientWeather.SkyBottomColors = ClientWeather.CloneColorAlphaDictionary(this.SkyBottomColors);
			clientWeather.SkySunsetColors = ClientWeather.CloneColorAlphaDictionary(this.SkySunsetColors);
			clientWeather.WaterTints = ClientWeather.CloneColorDictionary(this.WaterTints);
			clientWeather.SunColors = ClientWeather.CloneColorDictionary(this.SunColors);
			clientWeather.SunScales = this.SunScales;
			clientWeather.SunGlowColors = ClientWeather.CloneColorAlphaDictionary(this.SunGlowColors);
			clientWeather.Moons = new Dictionary<int, string>(this.Moons);
			clientWeather.MoonColors = ClientWeather.CloneColorAlphaDictionary(this.MoonColors);
			clientWeather.MoonScales = this.MoonScales;
			clientWeather.MoonGlowColors = ClientWeather.CloneColorAlphaDictionary(this.MoonGlowColors);
			clientWeather.ScreenEffect = this.ScreenEffect;
			clientWeather.ScreenEffectColors = ClientWeather.CloneColorAlphaDictionary(this.ScreenEffectColors);
			clientWeather.Fog = new NearFar(this.Fog.Near, this.Fog.Far);
			clientWeather.FogColors = ClientWeather.CloneColorDictionary(this.FogColors);
			clientWeather.FogHeightFalloffs = this.FogHeightFalloffs;
			clientWeather.FogDensities = this.FogDensities;
			clientWeather.ColorFilters = ClientWeather.CloneColorDictionary(this.ColorFilters);
			clientWeather.Clouds = new ClientCloud[4];
			for (int i = 0; i < clientWeather.Clouds.Length; i++)
			{
				clientWeather.Clouds[i] = this.Clouds[i].Clone();
			}
			return clientWeather;
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x001A6E64 File Offset: 0x001A5064
		public static Tuple<float, Color>[] CloneColorDictionary(Tuple<float, Color>[] list)
		{
			Tuple<float, Color>[] array = new Tuple<float, Color>[list.Length];
			for (int i = 0; i < list.Length; i++)
			{
				Tuple<float, Color> tuple = list[i];
				array[i] = new Tuple<float, Color>(tuple.Item1, new Color(tuple.Item2));
			}
			return array;
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x001A6EB4 File Offset: 0x001A50B4
		public static Tuple<float, Color>[] CloneDownColorAlphaDictionary(Tuple<float, ColorAlpha>[] list)
		{
			Tuple<float, Color>[] array = new Tuple<float, Color>[list.Length];
			for (int i = 0; i < list.Length; i++)
			{
				Tuple<float, ColorAlpha> tuple = list[i];
				array[i] = new Tuple<float, Color>(tuple.Item1, new Color(tuple.Item2.Red, tuple.Item2.Green, tuple.Item2.Blue));
			}
			return array;
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x001A6F20 File Offset: 0x001A5120
		public static Tuple<float, ColorAlpha>[] CloneColorAlphaDictionary(Tuple<float, ColorAlpha>[] list)
		{
			Tuple<float, ColorAlpha>[] array = new Tuple<float, ColorAlpha>[list.Length];
			for (int i = 0; i < list.Length; i++)
			{
				Tuple<float, ColorAlpha> tuple = list[i];
				array[i] = new Tuple<float, ColorAlpha>(tuple.Item1, new ColorAlpha(tuple.Item2));
			}
			return array;
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x001A6F70 File Offset: 0x001A5170
		public static Tuple<float, float>[] ParseTimeDictionary(Dictionary<float, float> dictionary, float defaultValue)
		{
			bool flag = dictionary == null || dictionary.Count == 0;
			Tuple<float, float>[] result;
			if (flag)
			{
				result = new Tuple<float, float>[]
				{
					new Tuple<float, float>(0f, defaultValue)
				};
			}
			else
			{
				Tuple<float, float>[] array = new Tuple<float, float>[dictionary.Count];
				int num = 0;
				foreach (KeyValuePair<float, float> keyValuePair in dictionary)
				{
					array[num++] = new Tuple<float, float>(keyValuePair.Key, keyValuePair.Value);
				}
				Array.Sort<Tuple<float, float>>(array, (Tuple<float, float> a, Tuple<float, float> b) => a.Item1.CompareTo(b.Item1));
				result = array;
			}
			return result;
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x001A7040 File Offset: 0x001A5240
		public static Tuple<float, Color>[] ParseTimeDictionary(Dictionary<float, Color> dictionary, byte[] defaultValue)
		{
			bool flag = dictionary == null || dictionary.Count == 0;
			Tuple<float, Color>[] result;
			if (flag)
			{
				result = new Tuple<float, Color>[]
				{
					new Tuple<float, Color>(0f, new Color((sbyte)defaultValue[0], (sbyte)defaultValue[1], (sbyte)defaultValue[2]))
				};
			}
			else
			{
				Tuple<float, Color>[] array = new Tuple<float, Color>[dictionary.Count];
				int num = 0;
				foreach (KeyValuePair<float, Color> keyValuePair in dictionary)
				{
					array[num++] = new Tuple<float, Color>(keyValuePair.Key, keyValuePair.Value);
				}
				Array.Sort<Tuple<float, Color>>(array, (Tuple<float, Color> a, Tuple<float, Color> b) => a.Item1.CompareTo(b.Item1));
				result = array;
			}
			return result;
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x001A7120 File Offset: 0x001A5320
		public static Tuple<float, ColorAlpha>[] ParseTimeDictionary(Dictionary<float, ColorAlpha> dictionary, byte[] defaultValue)
		{
			bool flag = dictionary == null || dictionary.Count == 0;
			Tuple<float, ColorAlpha>[] result;
			if (flag)
			{
				result = new Tuple<float, ColorAlpha>[]
				{
					new Tuple<float, ColorAlpha>(0f, new ColorAlpha((sbyte)defaultValue[3], (sbyte)defaultValue[0], (sbyte)defaultValue[1], (sbyte)defaultValue[2]))
				};
			}
			else
			{
				Tuple<float, ColorAlpha>[] array = new Tuple<float, ColorAlpha>[dictionary.Count];
				int num = 0;
				foreach (KeyValuePair<float, ColorAlpha> keyValuePair in dictionary)
				{
					array[num++] = new Tuple<float, ColorAlpha>(keyValuePair.Key, keyValuePair.Value);
				}
				Array.Sort<Tuple<float, ColorAlpha>>(array, (Tuple<float, ColorAlpha> a, Tuple<float, ColorAlpha> b) => a.Item1.CompareTo(b.Item1));
				result = array;
			}
			return result;
		}

		// Token: 0x040034DF RID: 13535
		public const string SunTexture = "Sky/Sun.png";

		// Token: 0x040034E0 RID: 13536
		public const float DefaultHour = 0f;

		// Token: 0x040034E1 RID: 13537
		public static readonly byte[] DefaultWhiteColor = new byte[]
		{
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue
		};

		// Token: 0x040034E2 RID: 13538
		public static readonly byte[] DefaultTransparentColor = new byte[]
		{
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			0
		};
	}
}
