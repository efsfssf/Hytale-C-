using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using HytaleClient.Utils;

namespace HytaleClient.Data.Audio
{
	// Token: 0x02000B75 RID: 2933
	internal class WwiseHeaderParser
	{
		// Token: 0x06005A11 RID: 23057 RVA: 0x001BF7C8 File Offset: 0x001BD9C8
		public static void Parse(string wwiseHeaderPath, out Dictionary<string, WwiseResource> upcomingWwiseIds)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			upcomingWwiseIds = new Dictionary<string, WwiseResource>();
			FileStream stream = File.OpenRead(wwiseHeaderPath);
			using (StreamReader streamReader = new StreamReader(stream))
			{
				char[] separator = new char[]
				{
					' '
				};
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				while (!flag2 || !flag4)
				{
					string text = streamReader.ReadLine();
					bool flag5 = text == null;
					if (flag5)
					{
						break;
					}
					string[] array = text.Split(separator, 6, StringSplitOptions.RemoveEmptyEntries);
					bool flag6 = array.Length == 0;
					if (!flag6)
					{
						string a = array[0];
						bool flag7 = a == "namespace" && array.Length > 1;
						if (flag7)
						{
							bool flag8 = array[1] == "EVENTS";
							if (flag8)
							{
								flag = true;
							}
							bool flag9 = array[1] == "GAME_PARAMETERS";
							if (flag9)
							{
								flag3 = true;
							}
						}
						else
						{
							bool flag10 = flag;
							if (flag10)
							{
								bool flag11 = a == "static" && array.Length > 5;
								if (flag11)
								{
									string key = array[3];
									string text2 = array[5];
									uint id = uint.Parse(text2.Remove(text2.Length - 2));
									upcomingWwiseIds[key] = new WwiseResource(WwiseResource.WwiseResourceType.Event, id);
								}
								else
								{
									bool flag12 = a == "}";
									if (flag12)
									{
										flag = false;
										flag2 = true;
									}
								}
							}
							else
							{
								bool flag13 = flag3;
								if (flag13)
								{
									bool flag14 = a == "static" && array.Length > 5;
									if (flag14)
									{
										string key2 = array[3];
										string text3 = array[5];
										uint id2 = uint.Parse(text3.Remove(text3.Length - 2));
										upcomingWwiseIds[key2] = new WwiseResource(WwiseResource.WwiseResourceType.GameParameter, id2);
									}
									else
									{
										bool flag15 = a == "}";
										if (flag15)
										{
											flag3 = false;
											flag4 = true;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0400384C RID: 14412
		private const string NamespaceToken = "namespace";

		// Token: 0x0400384D RID: 14413
		private const string StaticToken = "static";

		// Token: 0x0400384E RID: 14414
		private const string RightCurlyBracketToken = "}";

		// Token: 0x0400384F RID: 14415
		private const string EventNamespace = "EVENTS";

		// Token: 0x04003850 RID: 14416
		private const string GameParametersNamespace = "GAME_PARAMETERS";
	}
}
