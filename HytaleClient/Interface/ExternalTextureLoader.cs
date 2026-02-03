using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using HytaleClient.Application;
using HytaleClient.Data;
using HytaleClient.Graphics;

namespace HytaleClient.Interface
{
	// Token: 0x02000801 RID: 2049
	internal class ExternalTextureLoader
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060038B6 RID: 14518 RVA: 0x00075694 File Offset: 0x00073894
		// (remove) Token: 0x060038B7 RID: 14519 RVA: 0x000756CC File Offset: 0x000738CC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<TextureArea> OnComplete;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060038B8 RID: 14520 RVA: 0x00075704 File Offset: 0x00073904
		// (remove) Token: 0x060038B9 RID: 14521 RVA: 0x0007573C File Offset: 0x0007393C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<Exception> OnFailure;

		// Token: 0x060038BA RID: 14522 RVA: 0x00075771 File Offset: 0x00073971
		public void Cancel()
		{
			this._tokenSource.Cancel();
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x00075780 File Offset: 0x00073980
		public static ExternalTextureLoader FromUrl(App app, string url)
		{
			ExternalTextureLoader loader = new ExternalTextureLoader();
			CancellationToken token = loader._tokenSource.Token;
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Image image;
				try
				{
					WebClient webClient = new WebClient();
					byte[] data = webClient.DownloadData(url);
					bool isCancellationRequested = token.IsCancellationRequested;
					if (isCancellationRequested)
					{
						return;
					}
					image = new Image(data);
					bool isCancellationRequested2 = token.IsCancellationRequested;
					if (isCancellationRequested2)
					{
						return;
					}
				}
				catch (Exception ex)
				{
					Exception exception2 = ex;
					Exception exception = exception2;
					app.Engine.RunOnMainThread(app.Interface, delegate
					{
						EventHandler<Exception> onFailure = loader.OnFailure;
						if (onFailure != null)
						{
							onFailure(null, exception);
						}
					}, false, false);
					return;
				}
				app.Engine.RunOnMainThread(app.Interface, delegate
				{
					bool isCancellationRequested3 = token.IsCancellationRequested;
					if (!isCancellationRequested3)
					{
						Texture texture = new Texture(Texture.TextureTypes.Texture2D);
						texture.CreateTexture2D(image.Width, image.Height, image.Pixels, 5, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
						TextureArea e = new TextureArea(texture, 0, 0, image.Width, image.Height, 1);
						EventHandler<TextureArea> onComplete = loader.OnComplete;
						if (onComplete != null)
						{
							onComplete(null, e);
						}
					}
				}, false, false);
			});
			return loader;
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x000757E0 File Offset: 0x000739E0
		public static TextureArea FromPath(string path)
		{
			Image image = new Image(File.ReadAllBytes(path));
			return ExternalTextureLoader.FromImage(image);
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x00075804 File Offset: 0x00073A04
		public static TextureArea FromImage(Image image)
		{
			Texture texture = new Texture(Texture.TextureTypes.Texture2D);
			texture.CreateTexture2D(image.Width, image.Height, image.Pixels, 5, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			return new TextureArea(texture, 0, 0, image.Width, image.Height, 1);
		}

		// Token: 0x040018A0 RID: 6304
		private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
	}
}
