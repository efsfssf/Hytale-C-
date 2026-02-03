using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Coherent.UI;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Interface.CoherentUI.Internals;
using HytaleClient.Utils;
using NLog;
using SDL2;

namespace HytaleClient.Interface.CoherentUI
{
	// Token: 0x020008DC RID: 2268
	internal class WebView : Disposable
	{
		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x06004208 RID: 16904 RVA: 0x000C71C8 File Offset: 0x000C53C8
		// (set) Token: 0x06004209 RID: 16905 RVA: 0x000C71D0 File Offset: 0x000C53D0
		public bool IsReady { get; private set; }

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x0600420A RID: 16906 RVA: 0x000C71D9 File Offset: 0x000C53D9
		// (set) Token: 0x0600420B RID: 16907 RVA: 0x000C71E1 File Offset: 0x000C53E1
		public bool IsReloading { get; private set; }

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x0600420C RID: 16908 RVA: 0x000C71EA File Offset: 0x000C53EA
		// (set) Token: 0x0600420D RID: 16909 RVA: 0x000C71F2 File Offset: 0x000C53F2
		public string URL { get; private set; }

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x000C71FB File Offset: 0x000C53FB
		// (set) Token: 0x0600420F RID: 16911 RVA: 0x000C7203 File Offset: 0x000C5403
		public int Width { get; private set; }

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x000C720C File Offset: 0x000C540C
		// (set) Token: 0x06004211 RID: 16913 RVA: 0x000C7214 File Offset: 0x000C5414
		public int Height { get; private set; }

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x000C721D File Offset: 0x000C541D
		// (set) Token: 0x06004213 RID: 16915 RVA: 0x000C7225 File Offset: 0x000C5425
		public float Scale { get; private set; }

		// Token: 0x06004214 RID: 16916 RVA: 0x000C7230 File Offset: 0x000C5430
		public WebView(Engine engine, CoUIManager coUiManager, string url, int width, int height, float scale)
		{
			this._engine = engine;
			this._coUiManager = coUiManager;
			this.URL = url;
			this.Width = width;
			this.Height = height;
			this.Scale = scale;
			this._viewListener = new CoUIViewListener(this, coUiManager);
			coUiManager.RegisterWebView(this);
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x000C72AC File Offset: 0x000C54AC
		public void Destroy()
		{
			CoUIViewDebugWrapper view = this._view;
			if (view != null)
			{
				view.Destroy();
			}
			this._view = null;
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x000C72C8 File Offset: 0x000C54C8
		protected override void DoDispose()
		{
			bool flag = this._view != null;
			if (flag)
			{
				throw new Exception("WebView must be destroyed from the CoUIManager thread before being disposed.");
			}
			bool flag2 = this._eventHandlers.Count > 0;
			if (flag2)
			{
				foreach (string argument in this._eventHandlers.Keys)
				{
					WebView.Logger.Info("Left-over event handler for: {0}", argument);
				}
				throw new Exception("Found " + this._eventHandlers.Count.ToString() + " left-over event handlers while disposing WebView.");
			}
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x000C7384 File Offset: 0x000C5584
		public void SetFocus()
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(WebView).FullName);
			}
			bool flag = this._view != null;
			if (flag)
			{
				this._view.SetFocus();
			}
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x000C73CC File Offset: 0x000C55CC
		public void Resize(int width, int height, float scale)
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(WebView).FullName);
			}
			this.Width = width;
			this.Height = height;
			this.Scale = scale;
			object coherentMemoryLock = this._coherentMemoryLock;
			lock (coherentMemoryLock)
			{
				this._coherentMemoryHandleValue = 0;
				this._coherentMemorySharedPointer = IntPtr.Zero;
			}
			bool flag2 = this._view != null;
			if (flag2)
			{
				this._coUiManager.RunInThread(delegate
				{
					this._view.Resize((uint)this.Width, (uint)this.Height);
					this._view.SetZoomLevel(Math.Log((double)this.Scale, 1.2));
					this._view.Redraw();
				});
			}
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x000C747C File Offset: 0x000C567C
		public bool IsResizing()
		{
			object coherentMemoryLock = this._coherentMemoryLock;
			bool result;
			lock (coherentMemoryLock)
			{
				result = (this._coherentMemorySharedPointer == IntPtr.Zero);
			}
			return result;
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x000C74CC File Offset: 0x000C56CC
		public void RenderToTexture()
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(WebView).FullName);
			}
			bool flag = !this._textureNeedsUpdate;
			if (!flag)
			{
				object coherentMemoryLock = this._coherentMemoryLock;
				lock (coherentMemoryLock)
				{
					IntPtr coherentMemorySharedPointer = this._coherentMemorySharedPointer;
					bool flag3 = coherentMemorySharedPointer != IntPtr.Zero;
					if (flag3)
					{
						this._engine.Graphics.GL.TexImage2D(GL.TEXTURE_2D, 0, 6408, this._coherentMemoryWidth, this._coherentMemoryHeight, 0, GL.BGRA, GL.UNSIGNED_BYTE, coherentMemorySharedPointer);
						this._textureNeedsUpdate = false;
					}
				}
			}
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x000C75A0 File Offset: 0x000C57A0
		public void Reload()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(WebView).FullName);
			}
			this.IsReady = false;
			this.IsReloading = true;
			this._coUiManager.RunInThread(delegate
			{
				this._view.Reload(true);
			});
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x000C7600 File Offset: 0x000C5800
		public void SetVolume(double volume)
		{
			bool flag = this._view == null;
			if (!flag)
			{
				this._coUiManager.RunInThread(delegate
				{
					this._view.SetMasterVolume(volume);
				});
			}
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x000C764C File Offset: 0x000C584C
		public void LoadURL(string url)
		{
			bool flag = this._view == null || url == this.URL;
			if (!flag)
			{
				this.URL = url;
				this._coUiManager.RunInThread(delegate
				{
					this._view.Load(this.URL);
				});
			}
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x000C7698 File Offset: 0x000C5898
		public void RegisterForEvent(string name, Disposable disposeGate, Action action)
		{
			WebView.<>c__DisplayClass46_0 CS$<>8__locals1 = new WebView.<>c__DisplayClass46_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.disposeGate = disposeGate;
			CS$<>8__locals1.action = action;
			CS$<>8__locals1.name = name;
			Debug.Assert(ThreadHelper.IsMainThread());
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(WebView).FullName);
			}
			Dictionary<string, WebView.WebViewEventHandler> eventHandlers = this._eventHandlers;
			lock (eventHandlers)
			{
				WebView.<>c__DisplayClass46_1 CS$<>8__locals2 = new WebView.<>c__DisplayClass46_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				bool flag2 = this._eventHandlers.ContainsKey(CS$<>8__locals2.CS$<>8__locals1.name);
				if (flag2)
				{
					throw new Exception("There's already an event handler registered for " + CS$<>8__locals2.CS$<>8__locals1.name);
				}
				CS$<>8__locals2.runActionOnMainThread = delegate()
				{
					CS$<>8__locals2.CS$<>8__locals1.<>4__this._engine.RunOnMainThread(CS$<>8__locals2.CS$<>8__locals1.disposeGate, CS$<>8__locals2.CS$<>8__locals1.action, false, false);
				};
				WebView.<>c__DisplayClass46_1 CS$<>8__locals3 = CS$<>8__locals2;
				Dictionary<string, WebView.WebViewEventHandler> eventHandlers2 = this._eventHandlers;
				string name2 = CS$<>8__locals2.CS$<>8__locals1.name;
				WebView.WebViewEventHandler webViewEventHandler = new WebView.WebViewEventHandler();
				webViewEventHandler.Action = CS$<>8__locals2.runActionOnMainThread;
				webViewEventHandler.CoherentHandle = null;
				WebView.WebViewEventHandler handler = webViewEventHandler;
				eventHandlers2[name2] = webViewEventHandler;
				CS$<>8__locals3.handler = handler;
				bool isReady = this.IsReady;
				if (isReady)
				{
					this._coUiManager.RunInThread(delegate
					{
						CS$<>8__locals2.handler.CoherentHandle = new BoundEventHandle?(CS$<>8__locals2.CS$<>8__locals1.<>4__this._view.RegisterForEvent(CS$<>8__locals2.CS$<>8__locals1.name, CS$<>8__locals2.runActionOnMainThread));
					});
				}
			}
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x000C77EC File Offset: 0x000C59EC
		public void RegisterForEvent<T>(string name, Disposable disposeGate, Action<T> action)
		{
			WebView.<>c__DisplayClass47_0<T> CS$<>8__locals1 = new WebView.<>c__DisplayClass47_0<T>();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.disposeGate = disposeGate;
			CS$<>8__locals1.action = action;
			CS$<>8__locals1.name = name;
			Debug.Assert(ThreadHelper.IsMainThread());
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(WebView).FullName);
			}
			Dictionary<string, WebView.WebViewEventHandler> eventHandlers = this._eventHandlers;
			lock (eventHandlers)
			{
				WebView.<>c__DisplayClass47_1<T> CS$<>8__locals2 = new WebView.<>c__DisplayClass47_1<T>();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				bool flag2 = this._eventHandlers.ContainsKey(CS$<>8__locals2.CS$<>8__locals1.name);
				if (flag2)
				{
					throw new Exception("There's already an event handler registered for " + CS$<>8__locals2.CS$<>8__locals1.name);
				}
				CS$<>8__locals2.runActionOnMainThread = delegate(T obj)
				{
					CS$<>8__locals2.CS$<>8__locals1.<>4__this._engine.RunOnMainThread(CS$<>8__locals2.CS$<>8__locals1.disposeGate, delegate
					{
						CS$<>8__locals2.CS$<>8__locals1.action(obj);
					}, false, false);
				};
				WebView.<>c__DisplayClass47_1<T> CS$<>8__locals3 = CS$<>8__locals2;
				Dictionary<string, WebView.WebViewEventHandler> eventHandlers2 = this._eventHandlers;
				string name2 = CS$<>8__locals2.CS$<>8__locals1.name;
				WebView.WebViewEventHandler webViewEventHandler = new WebView.WebViewEventHandler();
				webViewEventHandler.Action = CS$<>8__locals2.runActionOnMainThread;
				webViewEventHandler.CoherentHandle = null;
				WebView.WebViewEventHandler handler = webViewEventHandler;
				eventHandlers2[name2] = webViewEventHandler;
				CS$<>8__locals3.handler = handler;
				bool isReady = this.IsReady;
				if (isReady)
				{
					this._coUiManager.RunInThread(delegate
					{
						CS$<>8__locals2.handler.CoherentHandle = new BoundEventHandle?(CS$<>8__locals2.CS$<>8__locals1.<>4__this._view.RegisterForEvent(CS$<>8__locals2.CS$<>8__locals1.name, CS$<>8__locals2.runActionOnMainThread));
					});
				}
			}
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x000C7940 File Offset: 0x000C5B40
		public void RegisterForEvent<T1, T2>(string name, Disposable disposeGate, Action<T1, T2> action)
		{
			WebView.<>c__DisplayClass48_0<T1, T2> CS$<>8__locals1 = new WebView.<>c__DisplayClass48_0<T1, T2>();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.disposeGate = disposeGate;
			CS$<>8__locals1.action = action;
			CS$<>8__locals1.name = name;
			Debug.Assert(ThreadHelper.IsMainThread());
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(WebView).FullName);
			}
			Dictionary<string, WebView.WebViewEventHandler> eventHandlers = this._eventHandlers;
			lock (eventHandlers)
			{
				WebView.<>c__DisplayClass48_1<T1, T2> CS$<>8__locals2 = new WebView.<>c__DisplayClass48_1<T1, T2>();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				bool flag2 = this._eventHandlers.ContainsKey(CS$<>8__locals2.CS$<>8__locals1.name);
				if (flag2)
				{
					throw new Exception("There's already an event handler registered for " + CS$<>8__locals2.CS$<>8__locals1.name);
				}
				CS$<>8__locals2.runActionOnMainThread = delegate(T1 obj1, T2 obj2)
				{
					CS$<>8__locals2.CS$<>8__locals1.<>4__this._engine.RunOnMainThread(CS$<>8__locals2.CS$<>8__locals1.disposeGate, delegate
					{
						CS$<>8__locals2.CS$<>8__locals1.action(obj1, obj2);
					}, false, false);
				};
				WebView.<>c__DisplayClass48_1<T1, T2> CS$<>8__locals3 = CS$<>8__locals2;
				Dictionary<string, WebView.WebViewEventHandler> eventHandlers2 = this._eventHandlers;
				string name2 = CS$<>8__locals2.CS$<>8__locals1.name;
				WebView.WebViewEventHandler webViewEventHandler = new WebView.WebViewEventHandler();
				webViewEventHandler.Action = CS$<>8__locals2.runActionOnMainThread;
				webViewEventHandler.CoherentHandle = null;
				WebView.WebViewEventHandler handler = webViewEventHandler;
				eventHandlers2[name2] = webViewEventHandler;
				CS$<>8__locals3.handler = handler;
				bool isReady = this.IsReady;
				if (isReady)
				{
					this._coUiManager.RunInThread(delegate
					{
						CS$<>8__locals2.handler.CoherentHandle = new BoundEventHandle?(CS$<>8__locals2.CS$<>8__locals1.<>4__this._view.RegisterForEvent(CS$<>8__locals2.CS$<>8__locals1.name, CS$<>8__locals2.runActionOnMainThread));
					});
				}
			}
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x000C7A94 File Offset: 0x000C5C94
		public void RegisterForEvent<T1, T2, T3>(string name, Disposable disposeGate, Action<T1, T2, T3> action)
		{
			WebView.<>c__DisplayClass49_0<T1, T2, T3> CS$<>8__locals1 = new WebView.<>c__DisplayClass49_0<T1, T2, T3>();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.disposeGate = disposeGate;
			CS$<>8__locals1.action = action;
			CS$<>8__locals1.name = name;
			Debug.Assert(ThreadHelper.IsMainThread());
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(WebView).FullName);
			}
			Dictionary<string, WebView.WebViewEventHandler> eventHandlers = this._eventHandlers;
			lock (eventHandlers)
			{
				WebView.<>c__DisplayClass49_1<T1, T2, T3> CS$<>8__locals2 = new WebView.<>c__DisplayClass49_1<T1, T2, T3>();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				bool flag2 = this._eventHandlers.ContainsKey(CS$<>8__locals2.CS$<>8__locals1.name);
				if (flag2)
				{
					throw new Exception("There's already an event handler registered for " + CS$<>8__locals2.CS$<>8__locals1.name);
				}
				CS$<>8__locals2.runActionOnMainThread = delegate(T1 obj1, T2 obj2, T3 obj3)
				{
					CS$<>8__locals2.CS$<>8__locals1.<>4__this._engine.RunOnMainThread(CS$<>8__locals2.CS$<>8__locals1.disposeGate, delegate
					{
						CS$<>8__locals2.CS$<>8__locals1.action(obj1, obj2, obj3);
					}, false, false);
				};
				WebView.<>c__DisplayClass49_1<T1, T2, T3> CS$<>8__locals3 = CS$<>8__locals2;
				Dictionary<string, WebView.WebViewEventHandler> eventHandlers2 = this._eventHandlers;
				string name2 = CS$<>8__locals2.CS$<>8__locals1.name;
				WebView.WebViewEventHandler webViewEventHandler = new WebView.WebViewEventHandler();
				webViewEventHandler.Action = CS$<>8__locals2.runActionOnMainThread;
				webViewEventHandler.CoherentHandle = null;
				WebView.WebViewEventHandler handler = webViewEventHandler;
				eventHandlers2[name2] = webViewEventHandler;
				CS$<>8__locals3.handler = handler;
				bool isReady = this.IsReady;
				if (isReady)
				{
					this._coUiManager.RunInThread(delegate
					{
						CS$<>8__locals2.handler.CoherentHandle = new BoundEventHandle?(CS$<>8__locals2.CS$<>8__locals1.<>4__this._view.RegisterForEvent(CS$<>8__locals2.CS$<>8__locals1.name, CS$<>8__locals2.runActionOnMainThread));
					});
				}
			}
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x000C7BE8 File Offset: 0x000C5DE8
		public void UnregisterFromEvent(string name)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(WebView).FullName);
			}
			Dictionary<string, WebView.WebViewEventHandler> eventHandlers = this._eventHandlers;
			bool flag = false;
			try
			{
				Monitor.Enter(eventHandlers, ref flag);
				WebView.WebViewEventHandler handler;
				bool flag2 = !this._eventHandlers.TryGetValue(name, out handler);
				if (flag2)
				{
					throw new Exception("There's no event handler registered for " + name);
				}
				this._eventHandlers.Remove(name);
				bool isReady = this.IsReady;
				if (isReady)
				{
					this._coUiManager.RunInThread(delegate
					{
						CoUIViewDebugWrapper view = this._view;
						if (view != null)
						{
							view.UnregisterFromEvent(handler.CoherentHandle.Value);
						}
					});
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(eventHandlers);
				}
			}
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x000C7CBC File Offset: 0x000C5EBC
		public void TriggerEvent(string name, object data1 = null, object data2 = null, object data3 = null, object data4 = null, object data5 = null)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(WebView).FullName);
			}
			bool flag = !this.IsReady || this.IsReloading;
			if (flag)
			{
				this._eventQueue.Add(Tuple.Create<string, object, object, object, object, object>(name, data1, data2, data3, data4, data5));
			}
			else
			{
				this._coUiManager.RunInThread(delegate
				{
					CoUIViewDebugWrapper view = this._view;
					if (view != null)
					{
						view.TriggerEvent(name, data1, data2, data3, data4, data5);
					}
				});
			}
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x000C7D94 File Offset: 0x000C5F94
		public ImageData CreateImageData(string name, int width, int height, IntPtr data, bool flipY)
		{
			return this._view.CreateImageData(name, width, height, data, flipY);
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x000C7DB8 File Offset: 0x000C5FB8
		public void Initialize(ViewContext viewContext)
		{
			bool flag = this._view != null;
			if (!flag)
			{
				ViewInfo viewInfo = new ViewInfo
				{
					Width = this.Width,
					Height = this.Height,
					IsTransparent = true,
					SupportClickThrough = false,
					ForceSoftwareRendering = (BuildInfo.Platform != Platform.Linux),
					TargetFrameRate = 1000,
					UsesSharedMemory = true
				};
				viewContext.CreateView(viewInfo, this.URL, this._viewListener);
			}
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x000C7E40 File Offset: 0x000C6040
		public void OnCoherentViewCreated(View view)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			this._view = new CoUIViewDebugWrapper(view);
			WebView.Logger.Info("Coherent view has been created");
			this._view.SetZoomLevel(Math.Log((double)this.Scale, 1.2));
			bool flag = this._coUiManager.FocusedWebView == this;
			if (flag)
			{
				this._view.SetFocus();
			}
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x000C7EB8 File Offset: 0x000C60B8
		public void OnCoherentReadyForBindings(int frameId, string path, bool isMainFrame)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			bool isReady = this.IsReady;
			if (isReady)
			{
				throw new Exception("Entering WebView.OnCoherentReadyForBindings but IsReady is true already");
			}
			bool flag = !isMainFrame;
			if (!flag)
			{
				Dictionary<string, WebView.WebViewEventHandler> eventHandlers = this._eventHandlers;
				lock (eventHandlers)
				{
					this.IsReady = true;
					this.IsReloading = false;
					foreach (KeyValuePair<string, WebView.WebViewEventHandler> keyValuePair in this._eventHandlers)
					{
						keyValuePair.Value.CoherentHandle = new BoundEventHandle?(this._view.RegisterForEvent(keyValuePair.Key, keyValuePair.Value.Action));
					}
				}
				this._engine.RunOnMainThread(this, delegate
				{
					foreach (Tuple<string, object, object, object, object, object> tuple in this._eventQueue)
					{
						this.TriggerEvent(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6);
					}
					this._eventQueue.Clear();
				}, false, false);
			}
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x000C7FC4 File Offset: 0x000C61C4
		public void OnCoherentScriptMessage(ViewListenerBase.MessageLevel level, string message, string sourceId, int line)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			WebView.Logger.Info<ViewListenerBase.MessageLevel, int, string>("({0}) {1} - {2}", level, line, message);
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x000C7FE9 File Offset: 0x000C61E9
		public void OnCoherentError(ViewError error)
		{
			WebView.Logger.Error<string, ViewErrorType>("A Coherent UI error occurred: {0} ({1})", error.Error, error.ErrorCode);
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x000C8008 File Offset: 0x000C6208
		public void OnCoherentFailLoad(int frameId, string validatedPath, bool isMainFrame, string error)
		{
			WebView.Logger.Error("Failed to load Coherent UI site, Error={0}, FrameId={1}, ValidatedPath={2}, IsMainFrame={3}", new object[]
			{
				error,
				frameId,
				validatedPath,
				isMainFrame
			});
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x000C803C File Offset: 0x000C623C
		public void OnCoherentFinishLoad(int frameId, string validatedPath, bool isMainFrame, int status, HTTPHeader[] headers)
		{
			WebView.Logger.Info("Successfully loaded Coherent UI site, Status={0}, FrameId={1}, ValidatedPath={2}, IsMainFrame={3}", new object[]
			{
				status,
				frameId,
				validatedPath,
				isMainFrame
			});
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x000C8078 File Offset: 0x000C6278
		public void OnCoherentDraw(CoherentHandle handle, bool usesSharedMemory, int width, int height)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			bool flag = this._coherentMemoryHandleValue != handle.HandleValue;
			if (flag)
			{
				object coherentMemoryLock = this._coherentMemoryLock;
				lock (coherentMemoryLock)
				{
					IntPtr coherentMemorySharedPointer = this._coherentMemorySharedPointer;
					bool flag3 = coherentMemorySharedPointer != IntPtr.Zero;
					if (flag3)
					{
						this._coherentMemorySharedPointer = IntPtr.Zero;
						bool flag4 = BuildInfo.Platform == Platform.Windows;
						if (flag4)
						{
							SharedMemoryMapHelper.FreeMapSharedMemoryWindows(coherentMemorySharedPointer);
						}
						else
						{
							bool flag5 = BuildInfo.Platform == Platform.Linux;
							if (flag5)
							{
								SharedMemoryMapHelper.FreeMapSharedMemoryLinux(coherentMemorySharedPointer);
							}
							else
							{
								SharedMemoryMapHelper.FreeMapSharedMemoryMacOS(coherentMemorySharedPointer, width * height * 4);
							}
						}
					}
					this._coherentMemoryHandleValue = handle.HandleValue;
					this._coherentMemoryWidth = width;
					this._coherentMemoryHeight = height;
					bool flag6 = BuildInfo.Platform == Platform.Windows;
					if (flag6)
					{
						this._coherentMemorySharedPointer = SharedMemoryMapHelper.DoMapSharedMemoryWindows(handle.HandleValue, width * height * 4);
					}
					else
					{
						bool flag7 = BuildInfo.Platform == Platform.Linux;
						if (flag7)
						{
							this._coherentMemorySharedPointer = SharedMemoryMapHelper.DoMapSharedMemoryLinux(handle.HandleValue);
						}
						else
						{
							this._coherentMemorySharedPointer = SharedMemoryMapHelper.DoMapSharedMemoryMacOS(handle.HandleValue, width * height * 4);
						}
					}
				}
			}
			this._textureNeedsUpdate = true;
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x000C81C0 File Offset: 0x000C63C0
		public void OnCursorChanged(CursorTypes cursorType)
		{
			this._engine.RunOnMainThread(this._engine, delegate
			{
				bool flag = cursorType == 29;
				IntPtr cursor;
				if (flag)
				{
					cursor = SDL.SDL_CreateSystemCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_CROSSHAIR);
				}
				else
				{
					cursor = SDL.SDL_CreateSystemCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_ARROW);
				}
				SDL.SDL_SetCursor(cursor);
			}, false, false);
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x000C81FC File Offset: 0x000C63FC
		public void OnNavigateTo(string path)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			bool isReady = this.IsReady;
			if (isReady)
			{
				this.IsReloading = true;
			}
			this.IsReady = false;
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x000C8232 File Offset: 0x000C6432
		public void KeyEvent(KeyEventData arg0)
		{
			CoUIViewDebugWrapper view = this._view;
			if (view != null)
			{
				view.KeyEvent(arg0);
			}
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x000C8247 File Offset: 0x000C6447
		public void MouseEvent(MouseEventData arg0)
		{
			CoUIViewDebugWrapper view = this._view;
			if (view != null)
			{
				view.MouseEvent(arg0);
			}
		}

		// Token: 0x0400204E RID: 8270
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002055 RID: 8277
		private readonly CoUIViewListener _viewListener;

		// Token: 0x04002056 RID: 8278
		private CoUIViewDebugWrapper _view;

		// Token: 0x04002057 RID: 8279
		private readonly Engine _engine;

		// Token: 0x04002058 RID: 8280
		private readonly CoUIManager _coUiManager;

		// Token: 0x04002059 RID: 8281
		private readonly object _coherentMemoryLock = new object();

		// Token: 0x0400205A RID: 8282
		private int _coherentMemoryHandleValue;

		// Token: 0x0400205B RID: 8283
		private IntPtr _coherentMemorySharedPointer;

		// Token: 0x0400205C RID: 8284
		private int _coherentMemoryWidth;

		// Token: 0x0400205D RID: 8285
		private int _coherentMemoryHeight;

		// Token: 0x0400205E RID: 8286
		private bool _textureNeedsUpdate;

		// Token: 0x0400205F RID: 8287
		private readonly List<Tuple<string, object, object, object, object, object>> _eventQueue = new List<Tuple<string, object, object, object, object, object>>();

		// Token: 0x04002060 RID: 8288
		private readonly Dictionary<string, WebView.WebViewEventHandler> _eventHandlers = new Dictionary<string, WebView.WebViewEventHandler>();

		// Token: 0x02000D8F RID: 3471
		private class WebViewEventHandler
		{
			// Token: 0x04004292 RID: 17042
			public Delegate Action;

			// Token: 0x04004293 RID: 17043
			public BoundEventHandle? CoherentHandle;
		}
	}
}
