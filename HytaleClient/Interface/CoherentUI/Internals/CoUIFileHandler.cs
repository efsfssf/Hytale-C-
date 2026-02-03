using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Coherent.UI;
using HytaleClient.Core;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Interface.CoherentUI.Internals
{
	// Token: 0x020008DE RID: 2270
	internal class CoUIFileHandler : FileHandler
	{
		// Token: 0x0600423A RID: 16954 RVA: 0x000C83C8 File Offset: 0x000C65C8
		protected virtual byte[] GetFile(string filePath)
		{
			bool flag = filePath.StartsWith("coui://builtin-assets/");
			if (flag)
			{
				try
				{
					return AssetManager.GetBuiltInAsset(filePath.Substring("coui://builtin-assets/".Length));
				}
				catch (FileNotFoundException ex)
				{
					CoUIFileHandler.Logger.Error(ex, "UI requested built-in file which doesn't exist \"{0}\", {1}", new object[]
					{
						ex.FileName,
						filePath
					});
				}
			}
			else
			{
				bool flag2 = filePath.StartsWith("coui://interface/");
				if (flag2)
				{
					filePath = filePath.Substring("coui://interface/".Length);
					try
					{
						return File.ReadAllBytes(Path.Combine(Paths.CoherentUI, filePath));
					}
					catch
					{
					}
				}
				else
				{
					bool flag3 = filePath.StartsWith("coui://monaco-editor/");
					if (flag3)
					{
						filePath = filePath.Substring("coui://monaco-editor/".Length);
						try
						{
							return File.ReadAllBytes(Path.Combine(Paths.MonacoEditor, filePath));
						}
						catch
						{
						}
					}
					else
					{
						bool flag4 = filePath.StartsWith("coui://world-previews/");
						if (flag4)
						{
							filePath = filePath.Substring("coui://world-previews/".Length);
							string path = Uri.UnescapeDataString(filePath.Substring(0, filePath.Length - ".png".Length)).Replace("/", string.Empty).Replace("\\", string.Empty);
							return File.ReadAllBytes(Path.Combine(Paths.Saves, path, "preview.png"));
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x000C8560 File Offset: 0x000C6760
		public override void ReadFile(string url, URLRequestBase request, ResourceResponse response)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			int num = url.LastIndexOfAny(CoUIFileHandler.FilePathEndMarkers);
			string text = (num > -1) ? url.Substring(0, num) : url;
			byte[] file = this.GetFile(text);
			bool flag = file == null;
			if (flag)
			{
				response.SignalFailure();
			}
			else
			{
				IntPtr buffer = response.GetBuffer((uint)file.Length);
				Marshal.Copy(file, 0, buffer, file.Length);
				CoUIFileHandler.SetResponseHeaders(response, CoUIFileHandler.GetMimeType(Path.GetExtension(text)), file.Length);
				response.SignalSuccess();
			}
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x000C85E9 File Offset: 0x000C67E9
		public override void WriteFile(string url, ResourceData resource)
		{
			resource.SignalFailure();
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x000C85F3 File Offset: 0x000C67F3
		private static void SetResponseHeaders(ResourceResponse response, string contentType, int size)
		{
			response.SetResponseHeader("Content-Type", contentType);
			response.SetResponseHeader("Cache-Control", "max-age=86400");
			response.SetResponseHeader("Content-Length", size.ToString());
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x000C8628 File Offset: 0x000C6828
		private static string GetMimeType(string extension)
		{
			string text;
			return CoUIFileHandler.MimeTypes.TryGetValue(extension, out text) ? text : "application/octet-stream";
		}

		// Token: 0x04002063 RID: 8291
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002064 RID: 8292
		private static readonly char[] FilePathEndMarkers = new char[]
		{
			'?',
			'#'
		};

		// Token: 0x04002065 RID: 8293
		private const string BuiltInAssetsPrefix = "coui://builtin-assets/";

		// Token: 0x04002066 RID: 8294
		private const string InterfacePrefix = "coui://interface/";

		// Token: 0x04002067 RID: 8295
		private const string MonacoEditorPrefix = "coui://monaco-editor/";

		// Token: 0x04002068 RID: 8296
		private const string WorldPreviewsPrefix = "coui://world-previews/";

		// Token: 0x04002069 RID: 8297
		private static readonly IDictionary<string, string> MimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{
				".html",
				"text/html"
			},
			{
				".js",
				"text/javascript"
			},
			{
				".jsx",
				"text/jsx"
			},
			{
				".json",
				"application/json"
			},
			{
				".ogg",
				"audio/ogg"
			},
			{
				".css",
				"text/css"
			},
			{
				".png",
				"image/png"
			},
			{
				".ttf",
				"application/x-font-ttf"
			},
			{
				".otf",
				"application/x-font-opentype"
			},
			{
				".woff",
				"application/font-woff"
			},
			{
				".svg",
				"image/svg+xml"
			}
		};
	}
}
