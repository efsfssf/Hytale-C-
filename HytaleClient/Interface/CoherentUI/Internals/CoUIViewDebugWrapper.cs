using System;
using System.Threading;
using Coherent.UI;
using Coherent.UI.Binding;

namespace HytaleClient.Interface.CoherentUI.Internals
{
	// Token: 0x020008E0 RID: 2272
	internal class CoUIViewDebugWrapper
	{
		// Token: 0x06004244 RID: 16964 RVA: 0x000C8810 File Offset: 0x000C6A10
		public CoUIViewDebugWrapper(View view)
		{
			this._innerView = view;
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x000C8824 File Offset: 0x000C6A24
		private void AssertCoherentThread()
		{
			bool flag = Thread.CurrentThread.Name != "CoherentUIManager";
			if (flag)
			{
				throw new Exception("Should be called from CoherentUIManager thread!");
			}
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x000C8858 File Offset: 0x000C6A58
		public BoundEventHandle BindCall(string name, Delegate handler)
		{
			this.AssertCoherentThread();
			return this._innerView.BindCall(name, handler);
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x000C8880 File Offset: 0x000C6A80
		public BoundEventHandle RegisterForEvent(string name, Delegate handler)
		{
			this.AssertCoherentThread();
			return this._innerView.RegisterForEvent(name, handler);
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x000C88A6 File Offset: 0x000C6AA6
		public void KeyEvent(KeyEventData arg0)
		{
			this.AssertCoherentThread();
			this._innerView.KeyEvent(arg0);
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x000C88BD File Offset: 0x000C6ABD
		public void MouseEvent(MouseEventData arg0)
		{
			this.AssertCoherentThread();
			this._innerView.MouseEvent(arg0);
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x000C88D4 File Offset: 0x000C6AD4
		public void TriggerEvent(string name, object data1, object data2 = null, object data3 = null, object data4 = null, object data5 = null)
		{
			this.AssertCoherentThread();
			ViewExtensions.TriggerEvent<object, object, object, object, object>(this._innerView, name, data1, data2, data3, data4, data5);
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x000C88F3 File Offset: 0x000C6AF3
		public void UnbindCall(BoundEventHandle handle)
		{
			this.AssertCoherentThread();
			this._innerView.UnbindCall(handle);
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x000C890A File Offset: 0x000C6B0A
		public void UnregisterFromEvent(BoundEventHandle handle)
		{
			this.AssertCoherentThread();
			this._innerView.UnregisterFromEvent(handle);
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x000C8924 File Offset: 0x000C6B24
		public ImageData CreateImageData(string name, int width, int height, IntPtr data, bool flipY)
		{
			this.AssertCoherentThread();
			return this._innerView.CreateImageData(name, width, height, data, flipY);
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x000C894F File Offset: 0x000C6B4F
		public void SetMasterVolume(double volume)
		{
			this.AssertCoherentThread();
			this._innerView.SetMasterVolume(volume);
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x000C8966 File Offset: 0x000C6B66
		public void KillFocus()
		{
			this.AssertCoherentThread();
			this._innerView.KillFocus();
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x000C897C File Offset: 0x000C6B7C
		public void SetFocus()
		{
			this.AssertCoherentThread();
			this._innerView.SetFocus();
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x000C8992 File Offset: 0x000C6B92
		public void Load(string path)
		{
			this.AssertCoherentThread();
			this._innerView.Load(path);
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x000C89A9 File Offset: 0x000C6BA9
		public void Resize(uint width, uint height)
		{
			this.AssertCoherentThread();
			this._innerView.Resize(width, height);
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x000C89C1 File Offset: 0x000C6BC1
		public void SetZoomLevel(double zoomLevel)
		{
			this.AssertCoherentThread();
			this._innerView.SetZoomLevel(zoomLevel);
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x000C89D8 File Offset: 0x000C6BD8
		public void Redraw()
		{
			this.AssertCoherentThread();
			this._innerView.Redraw();
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x000C89EE File Offset: 0x000C6BEE
		public void Destroy()
		{
			this.AssertCoherentThread();
			this._innerView.Destroy();
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x000C8A04 File Offset: 0x000C6C04
		public void Reload(bool ignoreCache)
		{
			this.AssertCoherentThread();
			this._innerView.Reload(ignoreCache);
		}

		// Token: 0x0400206D RID: 8301
		private readonly View _innerView;
	}
}
