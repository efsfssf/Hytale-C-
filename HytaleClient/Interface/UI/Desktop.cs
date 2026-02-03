using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Batcher2D;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.Interface.UI
{
	// Token: 0x02000826 RID: 2086
	public class Desktop
	{
		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x06003A2E RID: 14894 RVA: 0x000833A7 File Offset: 0x000815A7
		public Cursors Cursors
		{
			get
			{
				return this.Graphics.Cursors;
			}
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06003A2F RID: 14895 RVA: 0x000833B4 File Offset: 0x000815B4
		// (set) Token: 0x06003A30 RID: 14896 RVA: 0x000833BC File Offset: 0x000815BC
		public Rectangle ViewportRectangle { get; private set; }

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06003A31 RID: 14897 RVA: 0x000833C5 File Offset: 0x000815C5
		// (set) Token: 0x06003A32 RID: 14898 RVA: 0x000833CD File Offset: 0x000815CD
		public Rectangle RootLayoutRectangle { get; private set; }

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06003A33 RID: 14899 RVA: 0x000833D6 File Offset: 0x000815D6
		// (set) Token: 0x06003A34 RID: 14900 RVA: 0x000833DE File Offset: 0x000815DE
		public float Scale { get; private set; }

		// Token: 0x06003A35 RID: 14901 RVA: 0x000833E8 File Offset: 0x000815E8
		public Desktop(IUIProvider provider, GraphicsDevice graphics, Batcher2D batcher2D)
		{
			this.Graphics = graphics;
			this.Provider = provider;
			this.Batcher2D = batcher2D;
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x00083468 File Offset: 0x00081668
		public void SetViewport(Rectangle viewportRectangle, float newScale)
		{
			this.ClearInput(false);
			this.ViewportRectangle = viewportRectangle;
			this.RootLayoutRectangle = new Rectangle(0, 0, viewportRectangle.Width, viewportRectangle.Height);
			bool flag = this.Scale != 0f;
			if (flag)
			{
				float scaleRatio = newScale / this.Scale;
				foreach (Element element in this._layerStack.Values)
				{
					element.Rescale(scaleRatio);
				}
				Element transientLayer = this._transientLayer;
				if (transientLayer != null)
				{
					transientLayer.Rescale(scaleRatio);
				}
				Element passiveLayer = this._passiveLayer;
				if (passiveLayer != null)
				{
					passiveLayer.Rescale(scaleRatio);
				}
			}
			this.Scale = newScale;
			this.Layout();
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x0008353C File Offset: 0x0008173C
		public int ScaleRound(float value)
		{
			return MathHelper.Round(value * this.Scale);
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x0008354B File Offset: 0x0008174B
		public float ScaleNoRound(float value)
		{
			return value * this.Scale;
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x00083555 File Offset: 0x00081755
		public Point ScaleRound(Point value)
		{
			return new Point(this.ScaleRound((float)value.X), this.ScaleRound((float)value.Y));
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x00083576 File Offset: 0x00081776
		public int UnscaleRound(float value)
		{
			return MathHelper.Round(value / this.Scale);
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x00083588 File Offset: 0x00081788
		public TexturePatch MakeTexturePatch(PatchStyle style)
		{
			TextureArea textureArea = style.TextureArea ?? ((style.TexturePath != null) ? this.Provider.MakeTextureArea(style.TexturePath.Value) : this.Provider.WhitePixel);
			bool flag = style.Area != null;
			if (flag)
			{
				Rectangle value = style.Area.Value;
				bool flag2 = style.TextureArea != null;
				if (flag2)
				{
					textureArea = textureArea.Clone();
				}
				textureArea.Rectangle = new Rectangle(textureArea.Rectangle.X + value.X * textureArea.Scale, textureArea.Rectangle.Y + value.Y * textureArea.Scale, value.Width * textureArea.Scale, value.Height * textureArea.Scale);
			}
			return new TexturePatch
			{
				TextureArea = textureArea,
				HorizontalBorder = style.HorizontalBorder,
				VerticalBorder = style.VerticalBorder,
				Color = style.Color
			};
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x0008368C File Offset: 0x0008188C
		public Element GetLayer(int key)
		{
			Element element;
			bool flag = this._layerStack.TryGetValue(key, ref element);
			Element result;
			if (flag)
			{
				result = element;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x000836B5 File Offset: 0x000818B5
		public Element GetTransientLayer()
		{
			return this._transientLayer;
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x000836BD File Offset: 0x000818BD
		public Element GetInteractiveLayer()
		{
			return this._transientLayer ?? ((this._layerStack.Count > 0) ? this._layerStack.Values[this._layerStack.Count - 1] : null);
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x000836F8 File Offset: 0x000818F8
		public void SetLayer(int key, Element layer)
		{
			bool flag = this._layerStack.ContainsKey(key);
			if (flag)
			{
				throw new Exception(string.Format("Cannot set layer at key {0}, there is alreay one.", key));
			}
			bool flag2 = this._layerStack.ContainsValue(layer) || layer.Parent != null;
			if (flag2)
			{
				throw new Exception("Cannot set element as layer, it is already in use.");
			}
			bool flag3 = !layer.Visible;
			if (flag3)
			{
				throw new Exception("Cannot set an invisible layer.");
			}
			bool flag4 = this._layerStack.Count == 0 || this._layerStack.Keys[this._layerStack.Count - 1] < key;
			bool flag5 = flag4;
			if (flag5)
			{
				bool flag6 = this._transientLayer != null;
				if (flag6)
				{
					this.SetTransientLayer(null);
				}
				this.ClearInput(true);
			}
			this._layerStack.Add(key, layer);
			layer.Mount();
			layer.Layout(new Rectangle?(this.RootLayoutRectangle), true);
			bool flag7 = flag4;
			if (flag7)
			{
				this.RefreshHover();
			}
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x000837FC File Offset: 0x000819FC
		public void ClearLayer(int key)
		{
			bool flag = !this._layerStack.ContainsKey(key);
			if (flag)
			{
				throw new Exception(string.Format("Cannot clear layer at key {0}, there none.", key));
			}
			bool flag2 = this._layerStack.Keys[this._layerStack.Count - 1] == key;
			bool flag3 = flag2;
			if (flag3)
			{
				this.ClearInput(true);
			}
			Element element = this._layerStack[key];
			this._layerStack.Remove(key);
			element.Unmount();
			bool flag4 = flag2;
			if (flag4)
			{
				this.RefreshHover();
			}
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x00083890 File Offset: 0x00081A90
		public void ClearAllLayers()
		{
			this.ClearInput(true);
			this.SetPassiveLayer(null);
			this.SetTransientLayer(null);
			while (this._layerStack.Count > 0)
			{
				Element element = this._layerStack.Values[this._layerStack.Count - 1];
				this._layerStack.RemoveAt(this._layerStack.Count - 1);
				element.Unmount();
			}
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x0008390C File Offset: 0x00081B0C
		public void SetTransientLayer(Element element)
		{
			this.ClearInput(true);
			Element transientLayer = this._transientLayer;
			if (transientLayer != null)
			{
				transientLayer.Unmount();
			}
			this._transientLayer = element;
			bool flag = this._transientLayer != null;
			if (flag)
			{
				this._transientLayer.Mount();
				this._transientLayer.Layout(new Rectangle?(this.RootLayoutRectangle), true);
			}
			this.RefreshHover();
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x00083978 File Offset: 0x00081B78
		public void SetPassiveLayer(Element element)
		{
			Element passiveLayer = this._passiveLayer;
			if (passiveLayer != null)
			{
				passiveLayer.Unmount();
			}
			this._passiveLayer = element;
			bool flag = this._passiveLayer != null;
			if (flag)
			{
				this._passiveLayer.Mount();
				this._passiveLayer.Layout(new Rectangle?(this.RootLayoutRectangle), true);
			}
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x000839D4 File Offset: 0x00081BD4
		public void Layout()
		{
			foreach (Element element in this._layerStack.Values)
			{
				element.Layout(new Rectangle?(this.RootLayoutRectangle), true);
			}
			Element transientLayer = this._transientLayer;
			if (transientLayer != null)
			{
				transientLayer.Layout(new Rectangle?(this.RootLayoutRectangle), true);
			}
			Element passiveLayer = this._passiveLayer;
			if (passiveLayer != null)
			{
				passiveLayer.Layout(new Rectangle?(this.RootLayoutRectangle), true);
			}
			this.RefreshHover();
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x00083A78 File Offset: 0x00081C78
		public void RegisterAnimationCallback(Action<float> animate)
		{
			this._animationCallbackChanges.Add(new Tuple<bool, Action<float>>(true, animate));
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x00083A8D File Offset: 0x00081C8D
		public void UnregisterAnimationCallback(Action<float> animate)
		{
			this._animationCallbackChanges.Add(new Tuple<bool, Action<float>>(false, animate));
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x00083AA4 File Offset: 0x00081CA4
		public void Update(float deltaTime)
		{
			foreach (Tuple<bool, Action<float>> tuple in this._animationCallbackChanges)
			{
				bool item = tuple.Item1;
				Action<float> item2 = tuple.Item2;
				bool flag = item;
				if (flag)
				{
					Debug.Assert(!this._animationCallbacks.Contains(item2));
					this._animationCallbacks.Add(item2);
				}
				else
				{
					Debug.Assert(this._animationCallbacks.Contains(item2));
					this._animationCallbacks.Remove(item2);
				}
			}
			this._animationCallbackChanges.Clear();
			foreach (Action<float> action in this._animationCallbacks)
			{
				action(deltaTime);
			}
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x00083BA8 File Offset: 0x00081DA8
		public void PrepareForDraw()
		{
			bool flag = this.Scale == 0f;
			if (flag)
			{
				throw new Exception("Viewport must be set before drawing.");
			}
			foreach (Element element in this._layerStack.Values)
			{
				element.PrepareForDraw();
			}
			Element transientLayer = this._transientLayer;
			if (transientLayer != null)
			{
				transientLayer.PrepareForDraw();
			}
			Element passiveLayer = this._passiveLayer;
			if (passiveLayer != null)
			{
				passiveLayer.PrepareForDraw();
			}
			bool flag2 = this._mouseDragElementToDraw != null;
			if (flag2)
			{
				Vector3 position = new Vector3((float)(this.MousePosition.X - this._mouseDragElementToDraw.AnchoredRectangle.X) - this._mouseDragMouseOffset.X, (float)(this.MousePosition.Y - this._mouseDragElementToDraw.AnchoredRectangle.Y) - this._mouseDragMouseOffset.Y, 0f);
				this.Graphics.Batcher2D.SetTransformationMatrix(position, Quaternion.Identity, 1f);
				this._mouseDragElementToDraw.PrepareForDraw();
				this.Graphics.Batcher2D.SetTransformationMatrix(Matrix.Identity);
			}
			bool drawOutlines = this.DrawOutlines;
			if (drawOutlines)
			{
				foreach (Element element2 in this._layerStack.Values)
				{
					element2.PrepareForDrawOutline();
				}
				Element transientLayer2 = this._transientLayer;
				if (transientLayer2 != null)
				{
					transientLayer2.PrepareForDrawOutline();
				}
				Element passiveLayer2 = this._passiveLayer;
				if (passiveLayer2 != null)
				{
					passiveLayer2.PrepareForDrawOutline();
				}
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x06003A49 RID: 14921 RVA: 0x00083D68 File Offset: 0x00081F68
		// (set) Token: 0x06003A4A RID: 14922 RVA: 0x00083D70 File Offset: 0x00081F70
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				this._isFocused = value;
				bool flag = !this._isFocused;
				if (flag)
				{
					this.ClearInput(true);
				}
			}
		}

		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x06003A4B RID: 14923 RVA: 0x00083D9A File Offset: 0x00081F9A
		// (set) Token: 0x06003A4C RID: 14924 RVA: 0x00083DA2 File Offset: 0x00081FA2
		public bool IsShortcutKeyDown { get; private set; }

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06003A4D RID: 14925 RVA: 0x00083DAB File Offset: 0x00081FAB
		// (set) Token: 0x06003A4E RID: 14926 RVA: 0x00083DB3 File Offset: 0x00081FB3
		public bool IsGuiKeyDown { get; private set; }

		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06003A4F RID: 14927 RVA: 0x00083DBC File Offset: 0x00081FBC
		// (set) Token: 0x06003A50 RID: 14928 RVA: 0x00083DC4 File Offset: 0x00081FC4
		public bool IsShiftKeyDown { get; private set; }

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x06003A51 RID: 14929 RVA: 0x00083DCD File Offset: 0x00081FCD
		// (set) Token: 0x06003A52 RID: 14930 RVA: 0x00083DD5 File Offset: 0x00081FD5
		public bool IsCtrlKeyDown { get; private set; }

		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x06003A53 RID: 14931 RVA: 0x00083DDE File Offset: 0x00081FDE
		// (set) Token: 0x06003A54 RID: 14932 RVA: 0x00083DE6 File Offset: 0x00081FE6
		public bool IsAltKeyDown { get; private set; }

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x06003A55 RID: 14933 RVA: 0x00083DEF File Offset: 0x00081FEF
		// (set) Token: 0x06003A56 RID: 14934 RVA: 0x00083DF7 File Offset: 0x00081FF7
		public bool IsWordSkipDown { get; private set; }

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06003A57 RID: 14935 RVA: 0x00083E00 File Offset: 0x00082000
		// (set) Token: 0x06003A58 RID: 14936 RVA: 0x00083E08 File Offset: 0x00082008
		public bool IsLineSkipDown { get; private set; }

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06003A59 RID: 14937 RVA: 0x00083E11 File Offset: 0x00082011
		// (set) Token: 0x06003A5A RID: 14938 RVA: 0x00083E19 File Offset: 0x00082019
		public Point MousePosition { get; private set; }

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x06003A5B RID: 14939 RVA: 0x00083E22 File Offset: 0x00082022
		// (set) Token: 0x06003A5C RID: 14940 RVA: 0x00083E2A File Offset: 0x0008202A
		public Element FocusedElement { get; private set; }

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x06003A5D RID: 14941 RVA: 0x00083E33 File Offset: 0x00082033
		public Element CapturedElement
		{
			get
			{
				return this._isMouseOverCapturedElement ? this._mouseElement : null;
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x06003A5E RID: 14942 RVA: 0x00083E46 File Offset: 0x00082046
		public bool IsMouseDragging
		{
			get
			{
				return this._mouseDragData != null || this._mouseDragElement != null;
			}
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x00083E5C File Offset: 0x0008205C
		public void StartMouseDrag(object data, Element element, Element elementToDraw = null)
		{
			bool flag = data == null && element == null;
			if (flag)
			{
				throw new ArgumentException("data and element can't be both null.");
			}
			this._mouseDragData = data;
			this._mouseDragElement = element;
			bool flag2 = elementToDraw != null;
			if (flag2)
			{
				this._mouseDragElementToDraw = elementToDraw;
				this._mouseDragMouseOffset = new Vector2((float)(this.MousePosition.X - elementToDraw.AnchoredRectangle.X), (float)(this.MousePosition.Y - elementToDraw.AnchoredRectangle.Y));
			}
			this.RefreshHover();
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x00083EE4 File Offset: 0x000820E4
		public void ClearMouseDrag()
		{
			foreach (Element element in this._dragOverStack)
			{
				element.OnMouseDragExit(this._mouseDragData, this._mouseDragElement);
			}
			this._dragOverStack.Clear();
			this._mouseDragData = null;
			this._mouseDragElement = null;
			this._mouseDragElementToDraw = null;
			this.RefreshHover();
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x00083F70 File Offset: 0x00082170
		public void CancelMouseDrag()
		{
			foreach (Element element in this._dragOverStack)
			{
				element.OnMouseDragExit(this._mouseDragData, this._mouseDragElement);
			}
			this._dragOverStack.Clear();
			object mouseDragData = this._mouseDragData;
			this._mouseDragData = null;
			Element mouseDragElement = this._mouseDragElement;
			this._mouseDragElement = null;
			this._mouseDragElementToDraw = null;
			mouseDragElement.OnMouseDragCancel(mouseDragData);
			this.RefreshHover();
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x00084010 File Offset: 0x00082210
		public void ClearInput(bool clearFocus = true)
		{
			this.IsShortcutKeyDown = false;
			this.IsShiftKeyDown = false;
			this.IsCtrlKeyDown = false;
			this.IsWordSkipDown = false;
			this.IsLineSkipDown = false;
			this.ClearMouseElement();
			if (clearFocus)
			{
				Element focusedElement = this.FocusedElement;
				if (focusedElement != null)
				{
					focusedElement.OnBlur();
				}
				this.FocusedElement = null;
			}
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x00084070 File Offset: 0x00082270
		public void FocusElement(Element element, bool clearMouseCapture = true)
		{
			bool flag = element != null;
			if (flag)
			{
				Debug.Assert(element.IsMounted, "Only mounted elements can be focused");
			}
			bool flag2 = clearMouseCapture && element != this._mouseElement;
			if (flag2)
			{
				this.ClearMouseCapture();
			}
			Element focusedElement = this.FocusedElement;
			if (focusedElement != null)
			{
				focusedElement.OnBlur();
			}
			this.FocusedElement = element;
			Element focusedElement2 = this.FocusedElement;
			if (focusedElement2 != null)
			{
				focusedElement2.OnFocus();
			}
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x000840E0 File Offset: 0x000822E0
		private void ClearMouseElement()
		{
			foreach (Element element in this._hoverStack)
			{
				element.Unhover();
			}
			this._hoverStack.Clear();
			bool flag = this._mouseElement != null;
			if (flag)
			{
				this.ClearMouseCapture();
				this._mouseElement.OnMouseOut();
				this._mouseElement = null;
			}
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x0008416C File Offset: 0x0008236C
		internal void ClearMouseCapture()
		{
			bool flag = this._mouseCaptureButton != null;
			if (flag)
			{
				this._mouseElement.ReleaseMouseButton(this._mouseCaptureButton.Value, this._mouseCaptureClicks.Value, false);
			}
			this._isMouseOverCapturedElement = false;
			this._mouseCaptureButton = null;
			this._mouseCaptureClicks = null;
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x000841CC File Offset: 0x000823CC
		public void OnMouseDown(int button, int clicks)
		{
			bool flag = this._layerStack.Count == 0;
			if (!flag)
			{
				bool isMouseDragging = this.IsMouseDragging;
				if (!isMouseDragging)
				{
					Element focusedElement = this.FocusedElement;
					bool flag2 = this._mouseElement != null && this._mouseCaptureButton == null;
					if (flag2)
					{
						this._mouseCaptureButton = new int?(button);
						this._mouseCaptureClicks = new int?(clicks);
						this._isMouseOverCapturedElement = true;
						this._mouseElement.PressMouseButton(button, clicks);
					}
					bool flag3 = this.FocusedElement != null && focusedElement == this.FocusedElement && focusedElement != this._mouseElement;
					if (flag3)
					{
						this.FocusElement(null, false);
					}
				}
			}
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x00084284 File Offset: 0x00082484
		public void OnMouseUp(int button, int clicks)
		{
			bool flag = this._layerStack.Count == 0;
			if (!flag)
			{
				bool flag2;
				if (this._mouseCaptureButton != null)
				{
					int? mouseCaptureButton = this._mouseCaptureButton;
					flag2 = !(button == mouseCaptureButton.GetValueOrDefault() & mouseCaptureButton != null);
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (!flag3)
				{
					bool isMouseDragging = this.IsMouseDragging;
					if (isMouseDragging)
					{
						foreach (Element element in this._dragOverStack)
						{
							element.OnMouseDragExit(this._mouseDragData, this._mouseDragElement);
						}
						this._dragOverStack.Clear();
						object mouseDragData = this._mouseDragData;
						this._mouseDragData = null;
						Element mouseDragElement = this._mouseDragElement;
						this._mouseDragElement = null;
						this._mouseDragElementToDraw = null;
						bool flag4 = false;
						Element dragOverElement = this._dragOverElement;
						if (dragOverElement != null)
						{
							dragOverElement.OnMouseDrop(mouseDragData, mouseDragElement, out flag4);
						}
						bool flag5 = !flag4;
						if (flag5)
						{
							mouseDragElement.OnMouseDragCancel(mouseDragData);
						}
						else
						{
							mouseDragElement.OnMouseDragComplete(this._mouseElement, mouseDragData);
						}
						this.RefreshHover();
					}
					bool flag6 = this._mouseCaptureButton != null;
					if (flag6)
					{
						Element mouseElement = this._mouseElement;
						bool isMouseOverCapturedElement = this._isMouseOverCapturedElement;
						this._mouseCaptureButton = null;
						this._mouseCaptureClicks = null;
						this._isMouseOverCapturedElement = false;
						this.RefreshHover();
						mouseElement.ReleaseMouseButton(button, clicks, isMouseOverCapturedElement);
					}
				}
			}
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x00084414 File Offset: 0x00082614
		public void OnMouseMove(Point mousePosition)
		{
			this.MousePosition = mousePosition;
			this.RefreshHover();
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x00084428 File Offset: 0x00082628
		internal void RefreshDragOver()
		{
			Debug.Assert(this._mouseDragElement != null);
			Element interactiveLayer = this.GetInteractiveLayer();
			Element element = interactiveLayer.HitTest(this.MousePosition);
			bool flag = element != null;
			if (flag)
			{
				for (Element element2 = element; element2 != null; element2 = element2.Parent)
				{
					this._upcomingDragOverStack.Add(element2);
				}
			}
			int i = 0;
			while (i < this._dragOverStack.Count)
			{
				bool flag2 = !this._upcomingDragOverStack.Contains(this._dragOverStack[i]);
				if (flag2)
				{
					this._dragOverStack[i].OnMouseDragExit(this._mouseDragData, this._mouseDragElement);
					this._dragOverStack.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			foreach (Element element3 in this._upcomingDragOverStack)
			{
				bool flag3 = !this._dragOverStack.Contains(element3);
				if (flag3)
				{
					element3.OnMouseDragEnter(this._mouseDragData, this._mouseDragElement);
				}
			}
			this._dragOverStack.Clear();
			this._dragOverStack.AddRange(this._upcomingDragOverStack);
			this._upcomingDragOverStack.Clear();
			this._dragOverElement = element;
			if (element != null)
			{
				element.OnMouseDragMove();
			}
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x000845AC File Offset: 0x000827AC
		internal void RefreshHover()
		{
			bool flag = this._mouseDragElement != null;
			if (flag)
			{
				this.RefreshDragOver();
			}
			else
			{
				bool flag2 = this._layerStack.Count == 0 || !this.IsFocused;
				if (!flag2)
				{
					Element interactiveLayer = this.GetInteractiveLayer();
					Element element = interactiveLayer.HitTest(this.MousePosition);
					bool flag3 = this._mouseCaptureButton != null;
					if (flag3)
					{
						this._isMouseOverCapturedElement = (this._mouseElement == element);
						this._mouseElement.MoveMouse();
					}
					else
					{
						bool flag4 = element != null;
						if (flag4)
						{
							for (Element element2 = element; element2 != null; element2 = element2.Parent)
							{
								this._upcomingHoverStack.Add(element2);
							}
						}
						int i = 0;
						while (i < this._hoverStack.Count)
						{
							bool flag5 = !this._upcomingHoverStack.Contains(this._hoverStack[i]);
							if (flag5)
							{
								this._hoverStack[i].Unhover();
								this._hoverStack.RemoveAt(i);
							}
							else
							{
								i++;
							}
						}
						foreach (Element element3 in this._upcomingHoverStack)
						{
							bool flag6 = !this._hoverStack.Contains(element3);
							if (flag6)
							{
								element3.Hover();
							}
						}
						this._hoverStack.Clear();
						this._hoverStack.AddRange(this._upcomingHoverStack);
						this._upcomingHoverStack.Clear();
						bool flag7 = element != this._mouseElement;
						if (flag7)
						{
							Element mouseElement = this._mouseElement;
							if (mouseElement != null)
							{
								mouseElement.OnMouseOut();
							}
							if (element != null)
							{
								element.OnMouseIn();
							}
						}
						this._mouseElement = element;
						this._isMouseOverCapturedElement = (this._mouseElement != null);
						Element mouseElement2 = this._mouseElement;
						if (mouseElement2 != null)
						{
							mouseElement2.MoveMouse();
						}
					}
				}
			}
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x000847C4 File Offset: 0x000829C4
		public void OnMouseWheel(Point offset)
		{
			bool flag = this._layerStack.Count == 0;
			if (!flag)
			{
				bool flag2 = this._mouseCaptureButton != null;
				if (!flag2)
				{
					Element element = this._mouseElement;
					while (element != null && !element.OnMouseWheel(offset))
					{
						element = element.Parent;
					}
					bool flag3 = element != null;
					if (flag3)
					{
						this.RefreshHover();
					}
				}
			}
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x0008482C File Offset: 0x00082A2C
		public void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			this.UpdateKeys(keycode, true);
			Element element = this.FocusedElement ?? this.GetInteractiveLayer();
			if (element != null)
			{
				element.OnKeyDown(keycode, repeat);
			}
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x00084856 File Offset: 0x00082A56
		public void OnKeyUp(SDL.SDL_Keycode keycode)
		{
			this.UpdateKeys(keycode, false);
			Element element = this.FocusedElement ?? this.GetInteractiveLayer();
			if (element != null)
			{
				element.OnKeyUp(keycode);
			}
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x00084880 File Offset: 0x00082A80
		private void UpdateKeys(SDL.SDL_Keycode keycode, bool isDown)
		{
			switch (keycode)
			{
			case SDL.SDL_Keycode.SDLK_LCTRL:
			case SDL.SDL_Keycode.SDLK_RCTRL:
			{
				this.IsCtrlKeyDown = isDown;
				bool flag = BuildInfo.Platform != Platform.MacOS;
				if (flag)
				{
					this.IsWordSkipDown = isDown;
					this.IsShortcutKeyDown = isDown;
				}
				break;
			}
			case SDL.SDL_Keycode.SDLK_LSHIFT:
			case SDL.SDL_Keycode.SDLK_RSHIFT:
				this.IsShiftKeyDown = isDown;
				break;
			case SDL.SDL_Keycode.SDLK_LALT:
			case SDL.SDL_Keycode.SDLK_RALT:
			{
				this.IsAltKeyDown = isDown;
				bool flag2 = BuildInfo.Platform == Platform.MacOS;
				if (flag2)
				{
					this.IsWordSkipDown = isDown;
				}
				break;
			}
			case SDL.SDL_Keycode.SDLK_LGUI:
			case SDL.SDL_Keycode.SDLK_RGUI:
			{
				this.IsGuiKeyDown = isDown;
				bool flag3 = BuildInfo.Platform == Platform.MacOS;
				if (flag3)
				{
					this.IsShortcutKeyDown = isDown;
				}
				break;
			}
			}
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x00084935 File Offset: 0x00082B35
		public void OnTextInput(string text)
		{
			Element element = this.FocusedElement ?? this.GetInteractiveLayer();
			if (element != null)
			{
				element.OnTextInput(text);
			}
		}

		// Token: 0x04001A27 RID: 6695
		public readonly IUIProvider Provider;

		// Token: 0x04001A28 RID: 6696
		public readonly Batcher2D Batcher2D;

		// Token: 0x04001A29 RID: 6697
		public readonly GraphicsDevice Graphics;

		// Token: 0x04001A2D RID: 6701
		public bool DrawOutlines;

		// Token: 0x04001A2E RID: 6702
		private readonly SortedList<int, Element> _layerStack = new SortedList<int, Element>();

		// Token: 0x04001A2F RID: 6703
		private Element _transientLayer;

		// Token: 0x04001A30 RID: 6704
		private Element _passiveLayer;

		// Token: 0x04001A31 RID: 6705
		private readonly List<Action<float>> _animationCallbacks = new List<Action<float>>();

		// Token: 0x04001A32 RID: 6706
		private readonly List<Tuple<bool, Action<float>>> _animationCallbackChanges = new List<Tuple<bool, Action<float>>>();

		// Token: 0x04001A33 RID: 6707
		private bool _isFocused = true;

		// Token: 0x04001A3D RID: 6717
		private readonly List<Element> _hoverStack = new List<Element>();

		// Token: 0x04001A3E RID: 6718
		private readonly List<Element> _upcomingHoverStack = new List<Element>();

		// Token: 0x04001A3F RID: 6719
		private readonly List<Element> _dragOverStack = new List<Element>();

		// Token: 0x04001A40 RID: 6720
		private readonly List<Element> _upcomingDragOverStack = new List<Element>();

		// Token: 0x04001A41 RID: 6721
		private Element _mouseElement;

		// Token: 0x04001A42 RID: 6722
		private Element _dragOverElement;

		// Token: 0x04001A43 RID: 6723
		private int? _mouseCaptureButton;

		// Token: 0x04001A44 RID: 6724
		private int? _mouseCaptureClicks;

		// Token: 0x04001A45 RID: 6725
		private bool _isMouseOverCapturedElement;

		// Token: 0x04001A46 RID: 6726
		public const float UnscaledMouseWheelMultiplier = 30f;

		// Token: 0x04001A47 RID: 6727
		public const int UnscaledMouseDragStartDistance = 3;

		// Token: 0x04001A48 RID: 6728
		private object _mouseDragData;

		// Token: 0x04001A49 RID: 6729
		private Element _mouseDragElement;

		// Token: 0x04001A4A RID: 6730
		private Element _mouseDragElementToDraw;

		// Token: 0x04001A4B RID: 6731
		private Vector2 _mouseDragMouseOffset;
	}
}
