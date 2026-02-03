using System;
using System.Globalization;
using System.IO;
using HytaleClient.AssetEditor.Interface.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Utils
{
	// Token: 0x02000B93 RID: 2963
	public class JsonUtils
	{
		// Token: 0x06005B6C RID: 23404 RVA: 0x001C960C File Offset: 0x001C780C
		public static void ValidateJson(string json)
		{
			using (JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(json)))
			{
				while (jsonTextReader.Read())
				{
					bool flag = jsonTextReader.TokenType != 5;
					if (!flag)
					{
						throw new JsonReaderException("Comments are not allowed in JSON!");
					}
				}
			}
		}

		// Token: 0x06005B6D RID: 23405 RVA: 0x001C9670 File Offset: 0x001C7870
		public static bool IsNull(JToken token)
		{
			return token == null || token.Type == 10;
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x001C9684 File Offset: 0x001C7884
		public static void RemoveProperty(JObject obj, PropertyPath path)
		{
			JToken jtoken = obj;
			foreach (string text in path.Elements)
			{
				JToken jtoken2 = jtoken[text];
				bool flag = jtoken2 == null;
				if (flag)
				{
					return;
				}
				jtoken = jtoken2;
			}
			bool flag2 = jtoken.Parent is JProperty;
			if (flag2)
			{
				jtoken.Parent.Remove();
				return;
			}
			jtoken.Remove();
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x001C96F4 File Offset: 0x001C78F4
		public static JToken ParseLenient(string value)
		{
			bool flag2;
			bool flag = bool.TryParse(value, out flag2);
			JToken result;
			if (flag)
			{
				result = flag2;
			}
			else
			{
				decimal num;
				bool flag3 = decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out num);
				if (flag3)
				{
					result = num;
				}
				else
				{
					bool flag4 = value.StartsWith("{") || value.StartsWith("[");
					if (flag4)
					{
						try
						{
							return JToken.Parse(value);
						}
						catch (Exception)
						{
						}
					}
					result = value;
				}
			}
			return result;
		}

		// Token: 0x06005B70 RID: 23408 RVA: 0x001C9784 File Offset: 0x001C7984
		public static string GetTitleFromKey(string text)
		{
			string text2 = "";
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				bool flag = char.IsUpper(c) && text2.Length > 0 && (i >= text.Length - 1 || !char.IsUpper(text[i + 1]));
				if (flag)
				{
					text2 += " ";
				}
				text2 += c.ToString();
			}
			return text2;
		}

		// Token: 0x06005B71 RID: 23409 RVA: 0x001C9814 File Offset: 0x001C7A14
		public static decimal ConvertToDecimal(double value)
		{
			bool flag = value > 7.922816251426434E+28;
			decimal result;
			if (flag)
			{
				result = decimal.MaxValue;
			}
			else
			{
				bool flag2 = value < -7.922816251426434E+28;
				if (flag2)
				{
					result = decimal.MinValue;
				}
				else
				{
					result = Convert.ToDecimal(value);
				}
			}
			return result;
		}

		// Token: 0x06005B72 RID: 23410 RVA: 0x001C9866 File Offset: 0x001C7A66
		public static decimal ConvertToDecimal(JToken token)
		{
			return JsonUtils.ConvertToDecimal((double)token);
		}
	}
}
