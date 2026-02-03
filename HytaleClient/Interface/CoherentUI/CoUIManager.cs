using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Coherent.UI;
using HytaleClient.Core;
using HytaleClient.Interface.CoherentUI.Internals;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Interface.CoherentUI
{
	// Token: 0x020008DA RID: 2266
	internal class CoUIManager : Disposable
	{
		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x060041F5 RID: 16885 RVA: 0x000C62A7 File Offset: 0x000C44A7
		// (set) Token: 0x060041F6 RID: 16886 RVA: 0x000C62AF File Offset: 0x000C44AF
		public WebView FocusedWebView { get; private set; }

		// Token: 0x060041F7 RID: 16887 RVA: 0x000C62B8 File Offset: 0x000C44B8
		public void SetFocusedWebView(WebView webView)
		{
			this.FocusedWebView = webView;
			bool flag = webView != null;
			if (flag)
			{
				this.RunInThread(new Action(webView.SetFocus));
			}
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x000C62EC File Offset: 0x000C44EC
		public CoUIManager(Engine engine, CoUIFileHandler fileHandler)
		{
			this._engine = engine;
			bool flag = !CoherentUI_Native.CheckUISDKVersionCompatible(Versioning.SDKVersion);
			if (flag)
			{
				CoUIManager.Logger.Warn("SDK versions for .NET and native DLLs are not compatible.");
			}
			else
			{
				bool flag2 = !CoherentUI_Native.CheckUISDKVersionExact(Versioning.SDKVersion);
				if (flag2)
				{
					CoUIManager.Logger.Warn("SDK versions for .NET and native DLLs are compatible but different.");
				}
			}
			this._threadCancellationToken = this._threadCancellationTokenSource.Token;
			this._threadActions = new BlockingCollection<Action>();
			this.TextureBufferHelper = new TextureBufferHelper();
			FactorySettings factorySettings = new FactorySettings
			{
				EnableSupportForProprietaryCodecs = true
			};
			this._contextFactory = CoherentUI_Native.InitializeCoherentUI(Versioning.SDKVersion, "AAEAADx/j4VGlJfs90rFyPnshqewq2iW5BFkrivHU6Uf/E16e/vUMH8b09vmf8stulr41/KrbNahUElWMGZneOz8zjIkGIm7EhTLwApzOtH0ukwbDGkLqGOIUw7SB5MtUdGiefYIrH6rPt1yGaPaMBZFbKlXXwx6uQwDPy/HUyZupyTgfdFquzjLFYAN51ZhabzaHKew+LafEgPt69DhMKBBfjp/pq7KM8gEsAsk9nUH31eSROuP9hFLOV1+O/SpFHtrKQ1PoWk5eA7pODReLSKUzHJdNoQiyCTWwJERCC44YtJyEREjQs8YpzCyQMA8QcKKWjWpdTwvYEQcMU24QjRB+zMBK9oAYDWAi6oAPKIMOLJXtE+OtBkhInmMron8uSxF8hwjYgvuN6wgK53mXjwODd3VzJ+2p7iGX4DsQwYkPvT0mcVe19JouczZrBelXNibpJ993+RCBb3Z/1fFYepyM2SI7MTjuQ4HVW+zH+Im89vz92bcWzZTy3bUZ7rLSWo42/zxIeR2RKSuO29dcg3ZjaebcmMXh6H6xvbQwO0I5yQdJcFF1bbDzTAJWlFyGKQuWenIpRaq6O07zii/1rjsd8eusEUm35OYulqGqwMAGZwy1ZUmtzTg6GSm8FaILSLq8bA8KJh8bDSacrAwePpiSqT+qjtYzJOOXgcKSjuXcLSf", factorySettings, new RenderingParameters(), 3, null);
			ContextSettings contextSettings = new ContextSettings
			{
				DebuggerPort = 9999,
				DisableWebSecurity = true
			};
			this._fileHandler = fileHandler;
			this._contextListener = new CoUIContextListener(new Action(this.OnContextReady));
			this._viewContext = this._contextFactory.CreateViewContext(contextSettings, this._contextListener, this._fileHandler);
			bool flag3 = this._viewContext == null;
			if (flag3)
			{
				CoUIManager.Logger.Warn("Could not create CoherentUI view context, Web views will not be available.");
			}
			else
			{
				this._thread = new Thread(new ThreadStart(this.CoherentUIManagerThreadStart))
				{
					Name = "CoherentUIManager",
					IsBackground = true
				};
				this._thread.Start();
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x060041F9 RID: 16889 RVA: 0x000C6463 File Offset: 0x000C4663
		public bool IsInitialized
		{
			get
			{
				return this._viewContext != null;
			}
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x000C6470 File Offset: 0x000C4670
		public void RegisterWebView(WebView webView)
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(CoUIManager).FullName);
			}
			this._webViews.Add(webView);
			bool viewContextReady = this._viewContextReady;
			if (viewContextReady)
			{
				webView.Initialize(this._viewContext);
			}
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x000C64C4 File Offset: 0x000C46C4
		public void RunInThread(Action action)
		{
			bool flag = this._thread == null;
			if (!flag)
			{
				Debug.Assert(!ThreadHelper.IsOnThread(this._thread));
				bool disposed = base.Disposed;
				if (disposed)
				{
					throw new ObjectDisposedException(typeof(CoUIManager).FullName);
				}
				this._threadActions.Add(action);
			}
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x000C6524 File Offset: 0x000C4724
		public void Update()
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(CoUIManager).FullName);
			}
			bool fetchSurfaces = this._engine.Window.GetState() != Window.WindowState.Minimized;
			this.RunInThread(delegate
			{
				this._viewContext.Update();
				bool fetchSurfaces = fetchSurfaces;
				if (fetchSurfaces)
				{
					this._viewContext.FetchSurfaces();
				}
			});
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x000C6590 File Offset: 0x000C4790
		private void CoherentUIManagerThreadStart()
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(CoUIManager).FullName);
			}
			while (!this._threadCancellationToken.IsCancellationRequested)
			{
				Action action;
				try
				{
					action = this._threadActions.Take(this._threadCancellationToken);
				}
				catch (OperationCanceledException)
				{
					break;
				}
				action();
			}
			foreach (WebView webView in this._webViews)
			{
				webView.Destroy();
			}
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x000C6650 File Offset: 0x000C4850
		private void OnContextReady()
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(CoUIManager).FullName);
			}
			this._viewContextReady = true;
			foreach (WebView webView in this._webViews)
			{
				webView.Initialize(this._viewContext);
			}
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x000C66D4 File Offset: 0x000C48D4
		protected override void DoDispose()
		{
			this._threadCancellationTokenSource.Cancel();
			Thread thread = this._thread;
			if (thread != null)
			{
				thread.Join();
			}
			this._threadCancellationTokenSource.Dispose();
			foreach (WebView webView in this._webViews)
			{
				webView.Dispose();
			}
			bool flag = this._viewContext != null;
			if (flag)
			{
				this._viewContext.Uninitialize();
				this._viewContext.Dispose();
			}
			this._contextListener.Dispose();
			this._fileHandler.Dispose();
			this._contextFactory.Destroy();
			this._contextFactory.Dispose();
		}

		// Token: 0x0400203B RID: 8251
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400203C RID: 8252
		private const string LicenseKey = "AAEAADx/j4VGlJfs90rFyPnshqewq2iW5BFkrivHU6Uf/E16e/vUMH8b09vmf8stulr41/KrbNahUElWMGZneOz8zjIkGIm7EhTLwApzOtH0ukwbDGkLqGOIUw7SB5MtUdGiefYIrH6rPt1yGaPaMBZFbKlXXwx6uQwDPy/HUyZupyTgfdFquzjLFYAN51ZhabzaHKew+LafEgPt69DhMKBBfjp/pq7KM8gEsAsk9nUH31eSROuP9hFLOV1+O/SpFHtrKQ1PoWk5eA7pODReLSKUzHJdNoQiyCTWwJERCC44YtJyEREjQs8YpzCyQMA8QcKKWjWpdTwvYEQcMU24QjRB+zMBK9oAYDWAi6oAPKIMOLJXtE+OtBkhInmMron8uSxF8hwjYgvuN6wgK53mXjwODd3VzJ+2p7iGX4DsQwYkPvT0mcVe19JouczZrBelXNibpJ993+RCBb3Z/1fFYepyM2SI7MTjuQ4HVW+zH+Im89vz92bcWzZTy3bUZ7rLSWo42/zxIeR2RKSuO29dcg3ZjaebcmMXh6H6xvbQwO0I5yQdJcFF1bbDzTAJWlFyGKQuWenIpRaq6O07zii/1rjsd8eusEUm35OYulqGqwMAGZwy1ZUmtzTg6GSm8FaILSLq8bA8KJh8bDSacrAwePpiSqT+qjtYzJOOXgcKSjuXcLSf";

		// Token: 0x0400203D RID: 8253
		public readonly TextureBufferHelper TextureBufferHelper;

		// Token: 0x0400203E RID: 8254
		private readonly Engine _engine;

		// Token: 0x0400203F RID: 8255
		private readonly ViewContextFactory _contextFactory;

		// Token: 0x04002040 RID: 8256
		private readonly CancellationTokenSource _threadCancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04002041 RID: 8257
		private readonly CancellationToken _threadCancellationToken;

		// Token: 0x04002042 RID: 8258
		private readonly BlockingCollection<Action> _threadActions;

		// Token: 0x04002043 RID: 8259
		private readonly CoUIFileHandler _fileHandler;

		// Token: 0x04002044 RID: 8260
		private readonly CoUIContextListener _contextListener;

		// Token: 0x04002045 RID: 8261
		private readonly ViewContext _viewContext;

		// Token: 0x04002046 RID: 8262
		private bool _viewContextReady;

		// Token: 0x04002048 RID: 8264
		private readonly List<WebView> _webViews = new List<WebView>();

		// Token: 0x04002049 RID: 8265
		private readonly Thread _thread;
	}
}
