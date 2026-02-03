using System;
using System.Collections.Generic;
using System.IO;
using HytaleClient.Application;
using Newtonsoft.Json.Linq;
using SDL2;

namespace HytaleClient.Utils
{
	// Token: 0x020007CD RID: 1997
	internal class ScreenResolutions
	{
		// Token: 0x06003422 RID: 13346 RVA: 0x00053574 File Offset: 0x00051774
		public static List<KeyValuePair<string, string>> GetAvailableResolutionOptions(App app)
		{
			bool flag = ScreenResolutions.AvailableScreenResolutions.Count == 0;
			if (flag)
			{
				ScreenResolutions.LoadResolutionsFromFile(app);
			}
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (ScreenResolution screenResolution in ScreenResolutions.AvailableScreenResolutions)
			{
				list.Add(new KeyValuePair<string, string>(screenResolution.GetOptionName(screenResolution.Fullscreen), screenResolution.ToString()));
			}
			return list;
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x00053610 File Offset: 0x00051810
		public static List<ScreenResolution> GetAvailableResolutions(App app)
		{
			bool flag = ScreenResolutions.AvailableScreenResolutions.Count == 0;
			if (flag)
			{
				ScreenResolutions.LoadResolutionsFromFile(app);
			}
			return ScreenResolutions.AvailableScreenResolutions;
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x00053640 File Offset: 0x00051840
		private static void LoadResolutionsFromFile(App app)
		{
			int displayIndex = SDL.SDL_GetWindowDisplayIndex(app.Engine.Window.Handle);
			SDL.SDL_Rect sdl_Rect;
			SDL.SDL_GetDisplayBounds(displayIndex, out sdl_Rect);
			string text = Path.Combine(Paths.GameData, "ScreenResolutions.json");
			bool flag = !File.Exists(text);
			if (!flag)
			{
				JArray jarray = JArray.Parse(File.ReadAllText(text));
				bool flag2 = !jarray.HasValues;
				if (!flag2)
				{
					bool fullscreen = false;
					ScreenResolutions.AvailableScreenResolutions.Clear();
					foreach (JToken jtoken in jarray)
					{
						JObject jobject = (JObject)jtoken;
						bool flag3 = jobject == null;
						if (!flag3)
						{
							ScreenResolution screenResolution = default(ScreenResolution);
							JToken jtoken2;
							bool flag4 = jobject.TryGetValue("Width", ref jtoken2);
							if (flag4)
							{
								screenResolution.Width = (int)jtoken2;
								bool flag5 = jobject.TryGetValue("Height", ref jtoken2);
								if (flag5)
								{
									screenResolution.Height = (int)jtoken2;
									bool flag6 = screenResolution.Width > sdl_Rect.w || screenResolution.Height > sdl_Rect.h;
									if (!flag6)
									{
										bool flag7 = screenResolution.Width == sdl_Rect.w && screenResolution.Height == sdl_Rect.h;
										if (flag7)
										{
											fullscreen = true;
										}
										screenResolution.Fullscreen = fullscreen;
										ScreenResolutions.AvailableScreenResolutions.Add(screenResolution);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0400175B RID: 5979
		public static ScreenResolution DefaultScreenResolution = new ScreenResolution(1280, 720, false);

		// Token: 0x0400175C RID: 5980
		public static ScreenResolution CustomScreenResolution = new ScreenResolution(1025, 769, false);

		// Token: 0x0400175D RID: 5981
		private static List<ScreenResolution> AvailableScreenResolutions = new List<ScreenResolution>();
	}
}
