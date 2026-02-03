using System;
using System.IO;
using System.Text;
using HytaleClient.Data;
using HytaleClient.Interface.Messages;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.AssetEditor.Utils
{
	// Token: 0x02000B91 RID: 2961
	public static class DataConversionUtils
	{
		// Token: 0x06005B63 RID: 23395 RVA: 0x001C88F8 File Offset: 0x001C6AF8
		private static byte[] GetPNGBytes(Image image)
		{
			object imageSaveLock = DataConversionUtils.ImageSaveLock;
			byte[] result;
			lock (imageSaveLock)
			{
				image.SavePNG(DataConversionUtils.TempPngFilePath, 255U, 65280U, 16711680U, 4278190080U);
				result = File.ReadAllBytes(DataConversionUtils.TempPngFilePath);
				File.Delete(DataConversionUtils.TempPngFilePath);
			}
			return result;
		}

		// Token: 0x06005B64 RID: 23396 RVA: 0x001C8974 File Offset: 0x001C6B74
		public static sbyte[] EncodeObject(object data)
		{
			JObject jobject = data as JObject;
			sbyte[] result;
			if (jobject == null)
			{
				string text = data as string;
				if (text == null)
				{
					Image image = data as Image;
					if (image == null)
					{
						string str = "Invalid object of type ";
						Type type = data.GetType();
						throw new Exception(str + ((type != null) ? type.ToString() : null) + " passed");
					}
					result = (sbyte[])DataConversionUtils.GetPNGBytes(image);
				}
				else
				{
					result = (sbyte[])Encoding.UTF8.GetBytes(text);
				}
			}
			else
			{
				string s = jobject.ToString();
				result = (sbyte[])Encoding.UTF8.GetBytes(s);
			}
			return result;
		}

		// Token: 0x06005B65 RID: 23397 RVA: 0x001C8A1C File Offset: 0x001C6C1C
		public static bool TryDecodeBytes(sbyte[] data, AssetEditorEditorType editorType, out object result, out FormattedMessage error)
		{
			result = null;
			error = null;
			switch (editorType)
			{
			case 1:
			{
				string @string;
				try
				{
					@string = Encoding.UTF8.GetString((byte[])data);
				}
				catch (Exception value)
				{
					DataConversionUtils.Logger.Error<Exception>(value);
					error = new FormattedMessage
					{
						MessageId = "ui.assetEditor.errors.errorOccurredFetching"
					};
					return false;
				}
				result = @string;
				return true;
			}
			case 2:
			case 3:
			{
				JObject jobject;
				try
				{
					string string2 = Encoding.UTF8.GetString((byte[])data);
					jobject = JObject.Parse(string2);
				}
				catch (Exception value2)
				{
					DataConversionUtils.Logger.Error<Exception>(value2);
					error = new FormattedMessage
					{
						MessageId = "ui.assetEditor.errors.invalidJson"
					};
					return false;
				}
				result = jobject;
				return true;
			}
			case 5:
			{
				Image image;
				try
				{
					image = new Image((byte[])data);
				}
				catch (Exception value3)
				{
					DataConversionUtils.Logger.Error<Exception>(value3);
					error = new FormattedMessage
					{
						MessageId = "ui.assetEditor.errors.errorOccurredFetching"
					};
					return false;
				}
				result = image;
				return true;
			}
			}
			error = new FormattedMessage
			{
				RawText = "Invalid format"
			};
			return false;
		}

		// Token: 0x04003938 RID: 14648
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003939 RID: 14649
		private static readonly string TempPngFilePath = Path.Combine(Paths.UserData, "AssetEditorPNGConversion.tmp");

		// Token: 0x0400393A RID: 14650
		private static readonly object ImageSaveLock = new object();
	}
}
