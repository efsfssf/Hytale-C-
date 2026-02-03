using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200085E RID: 2142
	public class Element
	{
		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x06003B9D RID: 15261 RVA: 0x0008E159 File Offset: 0x0008C359
		// (set) Token: 0x06003B9E RID: 15262 RVA: 0x0008E161 File Offset: 0x0008C361
		public bool IsMounted { get; private set; }

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x06003B9F RID: 15263 RVA: 0x0008E16A File Offset: 0x0008C36A
		// (set) Token: 0x06003BA0 RID: 15264 RVA: 0x0008E172 File Offset: 0x0008C372
		public bool IsHovered { get; private set; }

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06003BA1 RID: 15265 RVA: 0x0008E17B File Offset: 0x0008C37B
		// (set) Token: 0x06003BA2 RID: 15266 RVA: 0x0008E183 File Offset: 0x0008C383
		public int? CapturedMouseButton { get; private set; }

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06003BA3 RID: 15267 RVA: 0x0008E18C File Offset: 0x0008C38C
		// (set) Token: 0x06003BA4 RID: 15268 RVA: 0x0008E194 File Offset: 0x0008C394
		[UIMarkupProperty]
		public bool Visible
		{
			get
			{
				return this._visible;
			}
			set
			{
				this._visible = value;
				bool flag = !this._visible;
				if (flag)
				{
					bool isMounted = this.IsMounted;
					if (isMounted)
					{
						this.Unmount();
					}
				}
				else
				{
					bool flag2;
					if (!this.IsMounted)
					{
						Element parent = this.Parent;
						flag2 = (parent != null && parent.IsMounted);
					}
					else
					{
						flag2 = false;
					}
					bool flag3 = flag2;
					if (flag3)
					{
						this.Mount();
					}
				}
			}
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06003BA5 RID: 15269 RVA: 0x0008E1F5 File Offset: 0x0008C3F5
		public Rectangle ContainerRectangle
		{
			get
			{
				return this._containerRectangle;
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06003BA6 RID: 15270 RVA: 0x0008E1FD File Offset: 0x0008C3FD
		public Rectangle AnchoredRectangle
		{
			get
			{
				return this._anchoredRectangle;
			}
		}

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06003BA7 RID: 15271 RVA: 0x0008E205 File Offset: 0x0008C405
		public Rectangle RectangleAfterPadding
		{
			get
			{
				return this._rectangleAfterPadding;
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06003BA8 RID: 15272 RVA: 0x0008E20D File Offset: 0x0008C40D
		public LayoutMode LayoutMode
		{
			get
			{
				return this._layoutMode;
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06003BA9 RID: 15273 RVA: 0x0008E215 File Offset: 0x0008C415
		public Point ScaledScrollOffset
		{
			get
			{
				return this._scaledScrollOffset;
			}
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06003BAA RID: 15274 RVA: 0x0008E21D File Offset: 0x0008C41D
		public Point ScaledScrollSize
		{
			get
			{
				return this._scaledScrollSize;
			}
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06003BAB RID: 15275 RVA: 0x0008E225 File Offset: 0x0008C425
		// (set) Token: 0x06003BAC RID: 15276 RVA: 0x0008E22D File Offset: 0x0008C42D
		private protected TextureArea _maskTextureArea { protected get; private set; }

		// Token: 0x1700104A RID: 4170
		// (set) Token: 0x06003BAD RID: 15277 RVA: 0x0008E238 File Offset: 0x0008C438
		[UIMarkupProperty]
		public string TooltipText
		{
			set
			{
				bool flag = this._tooltip == null;
				if (flag)
				{
					this._tooltip = new TextTooltipLayer(this.Desktop);
				}
				this._hasTooltipText = (value != null);
				this._tooltip.Text = value;
			}
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06003BAE RID: 15278 RVA: 0x0008E27B File Offset: 0x0008C47B
		// (set) Token: 0x06003BAF RID: 15279 RVA: 0x0008E290 File Offset: 0x0008C490
		[UIMarkupProperty]
		public TextTooltipStyle TextTooltipStyle
		{
			get
			{
				TextTooltipLayer tooltip = this._tooltip;
				return (tooltip != null) ? tooltip.Style : null;
			}
			set
			{
				bool flag = this._tooltip == null;
				if (flag)
				{
					this._tooltip = new TextTooltipLayer(this.Desktop);
				}
				this._tooltip.Style = value;
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x0008E2C8 File Offset: 0x0008C4C8
		// (set) Token: 0x06003BB1 RID: 15281 RVA: 0x0008E2F4 File Offset: 0x0008C4F4
		[UIMarkupProperty]
		public float? TextTooltipShowDelay
		{
			get
			{
				TextTooltipLayer tooltip = this._tooltip;
				return (tooltip != null) ? new float?(tooltip.ShowDelay) : null;
			}
			set
			{
				bool flag = this._tooltip == null;
				if (flag)
				{
					this._tooltip = new TextTooltipLayer(this.Desktop);
				}
				bool flag2 = value != null;
				if (flag2)
				{
					this._tooltip.ShowDelay = value.Value;
				}
			}
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x0008E340 File Offset: 0x0008C540
		public string GetPathInTree()
		{
			string text = base.GetType().Name;
			bool flag = this.Name != null;
			if (flag)
			{
				text = text + "#" + this.Name;
			}
			return (this.Parent == null) ? text : (this.Parent.GetPathInTree() + " > " + text);
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x0008E39E File Offset: 0x0008C59E
		public override string ToString()
		{
			return this.GetPathInTree();
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x0008E3A6 File Offset: 0x0008C5A6
		internal virtual void AddFromMarkup(Element child)
		{
			this.Add(child, -1);
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x0008E3B4 File Offset: 0x0008C5B4
		public Element(Desktop desktop, Element parent)
		{
			this.Desktop = desktop;
			if (parent != null)
			{
				parent.Add(this, -1);
			}
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06003BB6 RID: 15286 RVA: 0x0008E417 File Offset: 0x0008C617
		public IReadOnlyList<Element> Children
		{
			get
			{
				return this._children;
			}
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x0008E420 File Offset: 0x0008C620
		public void Add(Element child, int index = -1)
		{
			Debug.Assert(child.Parent == null, "Can't add element as child, it already has a parent.");
			child.Parent = this;
			bool flag = index == -1;
			if (flag)
			{
				index = this._children.Count;
			}
			this._children.Insert(index, child);
			bool flag2 = this.IsMounted && child.Visible;
			if (flag2)
			{
				child.Mount();
			}
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x0008E488 File Offset: 0x0008C688
		public void Add(Element child, Element before)
		{
			this.Add(child, this._children.IndexOf(before));
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x0008E4A0 File Offset: 0x0008C6A0
		public void Remove(Element child)
		{
			Debug.Assert(child.Parent == this, "Element isn't a child, can't unparent.");
			this._children.Remove(child);
			child.Parent = null;
			bool isMounted = child.IsMounted;
			if (isMounted)
			{
				child.Unmount();
			}
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x0008E4E8 File Offset: 0x0008C6E8
		public void RemoveAt(int index)
		{
			bool flag = index >= this._children.Count;
			if (flag)
			{
				throw new IndexOutOfRangeException(index.ToString());
			}
			Element element = this._children[index];
			Debug.Assert(element != null, "Element doesn't exist from index " + index.ToString() + ", can't remove.");
			this.Remove(element);
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x0008E550 File Offset: 0x0008C750
		public void Reorder(Element child, int index = -1)
		{
			Debug.Assert(child.Parent == this, "Element isn't a child, can't reorder.");
			this._children.Remove(child);
			bool flag = index == -1;
			if (flag)
			{
				index = this._children.Count;
			}
			this._children.Insert(index, child);
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0008E5A4 File Offset: 0x0008C7A4
		public void Clear()
		{
			foreach (Element element in this._children)
			{
				bool isMounted = element.IsMounted;
				if (isMounted)
				{
					element.Unmount();
				}
				element.Parent = null;
			}
			this._children.Clear();
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x0008E61C File Offset: 0x0008C81C
		public T Find<T>(string name) where T : Element
		{
			foreach (Element element in this._children)
			{
				bool flag = element.Name == name;
				if (flag)
				{
					return element as T;
				}
				T t = element.Find<T>(name);
				bool flag2 = t != null;
				if (flag2)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x0008E6B8 File Offset: 0x0008C8B8
		internal void Mount()
		{
			Debug.Assert(!this.IsMounted);
			this.IsMounted = true;
			this._waitingForLayoutAfterMount = true;
			foreach (Element element in this._children)
			{
				bool flag = !element.IsMounted && element.Visible;
				if (flag)
				{
					element.Mount();
				}
			}
			this.OnMounted();
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x0008E748 File Offset: 0x0008C948
		internal void Unmount()
		{
			Debug.Assert(this.IsMounted);
			this.IsMounted = false;
			bool flag = !this.KeepScrollPosition;
			if (flag)
			{
				this._scaledScrollOffset = Point.Zero;
			}
			this._horizontalScrollbarState = Element.ScrollbarState.Default;
			this._verticalScrollbarState = Element.ScrollbarState.Default;
			foreach (Element element in this._children)
			{
				bool isMounted = element.IsMounted;
				if (isMounted)
				{
					element.Unmount();
				}
			}
			bool isHovered = this.IsHovered;
			if (isHovered)
			{
				this.Desktop.RefreshHover();
			}
			bool flag2 = this.CapturedMouseButton != null;
			if (flag2)
			{
				this.Desktop.ClearMouseCapture();
			}
			TextTooltipLayer tooltip = this._tooltip;
			if (tooltip != null)
			{
				tooltip.Stop();
			}
			this.OnUnmounted();
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x0008E834 File Offset: 0x0008CA34
		protected virtual void OnMounted()
		{
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x0008E837 File Offset: 0x0008CA37
		protected virtual void OnUnmounted()
		{
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x0008E83C File Offset: 0x0008CA3C
		internal void Hover()
		{
			Debug.Assert(!this.IsHovered);
			this.IsHovered = true;
			this.OnMouseEnter();
			bool hasTooltipText = this._hasTooltipText;
			if (hasTooltipText)
			{
				this._tooltip.Start(false);
			}
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x0008E87F File Offset: 0x0008CA7F
		internal void Unhover()
		{
			Debug.Assert(this.IsHovered);
			this.IsHovered = false;
			this._horizontalScrollbarState = Element.ScrollbarState.Default;
			this._verticalScrollbarState = Element.ScrollbarState.Default;
			this.OnMouseLeave();
			TextTooltipLayer tooltip = this._tooltip;
			if (tooltip != null)
			{
				tooltip.Stop();
			}
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x0008E8BD File Offset: 0x0008CABD
		protected virtual void OnMouseEnter()
		{
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x0008E8C0 File Offset: 0x0008CAC0
		protected virtual void OnMouseLeave()
		{
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x0008E8C3 File Offset: 0x0008CAC3
		protected internal virtual void OnFocus()
		{
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x0008E8C6 File Offset: 0x0008CAC6
		protected internal virtual void OnBlur()
		{
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x0008E8C9 File Offset: 0x0008CAC9
		protected internal virtual void Validate()
		{
			Element parent = this.Parent;
			if (parent != null)
			{
				parent.Validate();
			}
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x0008E8DD File Offset: 0x0008CADD
		protected internal virtual void Dismiss()
		{
			Element parent = this.Parent;
			if (parent != null)
			{
				parent.Dismiss();
			}
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x0008E8F4 File Offset: 0x0008CAF4
		internal void PressMouseButton(int button, int clicks)
		{
			Debug.Assert(this.CapturedMouseButton == null);
			this.CapturedMouseButton = new int?(button);
			bool flag = (long)button == 1L;
			if (flag)
			{
				bool flag2 = this._scaledScrollArea.X > 0 && this._horizontalScrollbarHandleRectangle.Contains(this.Desktop.MousePosition);
				if (flag2)
				{
					this._horizontalScrollbarState = Element.ScrollbarState.Dragged;
					this._scrollbarDragOffset = this._scrollbarOffsets.X - this.Desktop.MousePosition.X;
					return;
				}
				bool flag3 = this._scaledScrollArea.Y > 0 && this._verticalScrollbarHandleRectangle.Contains(this.Desktop.MousePosition);
				if (flag3)
				{
					this._verticalScrollbarState = Element.ScrollbarState.Dragged;
					this._scrollbarDragOffset = this._scrollbarOffsets.Y - this.Desktop.MousePosition.Y;
					return;
				}
			}
			this.OnMouseButtonDown(new MouseButtonEvent(button, clicks));
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x0008E9F4 File Offset: 0x0008CBF4
		internal void ReleaseMouseButton(int button, int clicks, bool activate)
		{
			int? capturedMouseButton = this.CapturedMouseButton;
			Debug.Assert(capturedMouseButton.GetValueOrDefault() == button & capturedMouseButton != null);
			this.CapturedMouseButton = null;
			bool flag = (long)button == 1L;
			if (flag)
			{
				bool flag2 = this._horizontalScrollbarState == Element.ScrollbarState.Dragged;
				if (flag2)
				{
					this._horizontalScrollbarState = ((this._scaledScrollArea.X > 0 && this._horizontalScrollbarHandleRectangle.Contains(this.Desktop.MousePosition)) ? Element.ScrollbarState.Hovered : Element.ScrollbarState.Default);
					return;
				}
				bool flag3 = this._verticalScrollbarState == Element.ScrollbarState.Dragged;
				if (flag3)
				{
					this._verticalScrollbarState = ((this._scaledScrollArea.Y > 0 && this._verticalScrollbarHandleRectangle.Contains(this.Desktop.MousePosition)) ? Element.ScrollbarState.Hovered : Element.ScrollbarState.Default);
					return;
				}
			}
			this.OnMouseButtonUp(new MouseButtonEvent(button, clicks), activate);
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x0008EAD4 File Offset: 0x0008CCD4
		internal void MoveMouse()
		{
			bool flag = this._horizontalScrollbarState == Element.ScrollbarState.Dragged;
			if (flag)
			{
				int x = this._scaledScrollOffset.X;
				int num = this.Desktop.MousePosition.X + this._scrollbarDragOffset;
				this._scaledScrollOffset.X = this._scaledScrollArea.X * num / this._viewRectangle.Width;
				this.ComputeScrollbars();
				Point point = new Point(this._scaledScrollOffset.X - x, 0);
				bool flag2 = point == Point.Zero;
				if (!flag2)
				{
					foreach (Element element in this._children)
					{
						element.ApplyParentScroll(point);
					}
				}
			}
			else
			{
				this._horizontalScrollbarState = ((this._scaledScrollArea.X > 0 && this._horizontalScrollbarHandleRectangle.Contains(this.Desktop.MousePosition)) ? Element.ScrollbarState.Hovered : Element.ScrollbarState.Default);
				bool flag3 = this._verticalScrollbarState == Element.ScrollbarState.Dragged;
				if (flag3)
				{
					int y = this._scaledScrollOffset.Y;
					int num2 = this.Desktop.MousePosition.Y + this._scrollbarDragOffset;
					this._scaledScrollOffset.Y = this._scaledScrollArea.Y * num2 / this._viewRectangle.Height;
					this.ComputeScrollbars();
					Point point2 = new Point(0, this._scaledScrollOffset.Y - y);
					bool flag4 = point2 == Point.Zero;
					if (!flag4)
					{
						foreach (Element element2 in this._children)
						{
							element2.ApplyParentScroll(point2);
						}
					}
				}
				else
				{
					this._verticalScrollbarState = ((this._scaledScrollArea.Y > 0 && this._verticalScrollbarHandleRectangle.Contains(this.Desktop.MousePosition)) ? Element.ScrollbarState.Hovered : Element.ScrollbarState.Default);
					this.OnMouseMove();
				}
			}
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x0008ED04 File Offset: 0x0008CF04
		public virtual void OnMouseOut()
		{
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x0008ED07 File Offset: 0x0008CF07
		public virtual void OnMouseIn()
		{
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x0008ED0A File Offset: 0x0008CF0A
		protected virtual void OnMouseButtonDown(MouseButtonEvent evt)
		{
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x0008ED0D File Offset: 0x0008CF0D
		protected virtual void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x0008ED10 File Offset: 0x0008CF10
		protected virtual void OnMouseMove()
		{
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x0008ED14 File Offset: 0x0008CF14
		protected internal virtual bool OnMouseWheel(Point offset)
		{
			Element.MouseWheelScrollBehaviourType mouseWheelScrollBehaviour = this.MouseWheelScrollBehaviour;
			Element.MouseWheelScrollBehaviourType mouseWheelScrollBehaviourType = mouseWheelScrollBehaviour;
			bool result;
			if (mouseWheelScrollBehaviourType != Element.MouseWheelScrollBehaviourType.VerticalOnly)
			{
				if (mouseWheelScrollBehaviourType != Element.MouseWheelScrollBehaviourType.HorizontalOnly)
				{
					result = this.Scroll((float)offset.X * 30f, (float)offset.Y * 30f);
				}
				else
				{
					result = this.Scroll((float)((offset.X != 0) ? offset.X : offset.Y) * 30f, 0f);
				}
			}
			else
			{
				result = this.Scroll(0f, (float)((offset.Y != 0) ? offset.Y : offset.X) * 30f);
			}
			return result;
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x0008EDB4 File Offset: 0x0008CFB4
		public bool Scroll(float x, float y)
		{
			bool flag = this._scaledScrollArea == Point.Zero;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Point scaledScrollOffset = this._scaledScrollOffset;
				this._scaledScrollOffset.X = this._scaledScrollOffset.X - this.Desktop.ScaleRound(x);
				this._scaledScrollOffset.Y = this._scaledScrollOffset.Y - this.Desktop.ScaleRound(y);
				this.ComputeScrollbars();
				Point point = this._scaledScrollOffset - scaledScrollOffset;
				bool flag2 = point == Point.Zero;
				if (flag2)
				{
					result = false;
				}
				else
				{
					foreach (Element element in this._children)
					{
						element.ApplyParentScroll(point);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x0008EE94 File Offset: 0x0008D094
		public void ScrollChildElementIntoView(Element element)
		{
			Debug.Assert(this.IsMounted);
			Debug.Assert(!this._waitingForLayoutAfterMount);
			Debug.Assert(!element._waitingForLayoutAfterMount);
			bool flag = this.LayoutMode == LayoutMode.TopScrolling || this.LayoutMode == LayoutMode.BottomScrolling;
			if (flag)
			{
				bool flag2 = this._viewRectangle.Top > element.AnchoredRectangle.Top;
				if (flag2)
				{
					int value = element.AnchoredRectangle.Top - this._viewRectangle.Top + this._scaledScrollOffset.Y;
					int? y = new int?(value);
					this.SetScroll(null, y);
				}
				else
				{
					bool flag3 = element.AnchoredRectangle.Bottom > this._viewRectangle.Bottom;
					if (flag3)
					{
						int value2 = element.AnchoredRectangle.Bottom - this._viewRectangle.Bottom + this._scaledScrollOffset.Y;
						int? y = new int?(value2);
						this.SetScroll(null, y);
					}
				}
			}
			else
			{
				bool flag4 = this.LayoutMode == LayoutMode.LeftScrolling || this.LayoutMode == LayoutMode.RightScrolling;
				if (flag4)
				{
					bool flag5 = this._viewRectangle.Left > element.AnchoredRectangle.Left;
					if (flag5)
					{
						int value3 = element.AnchoredRectangle.Left - this._viewRectangle.Left + this._scaledScrollOffset.X;
						int? x = new int?(value3);
						int? y = null;
						this.SetScroll(x, y);
					}
					else
					{
						bool flag6 = element.AnchoredRectangle.Right > this._viewRectangle.Right;
						if (flag6)
						{
							int value4 = element.AnchoredRectangle.Right - this._viewRectangle.Right + this._scaledScrollOffset.X;
							int? x2 = new int?(value4);
							int? y = null;
							this.SetScroll(x2, y);
						}
					}
				}
				else
				{
					Debug.Fail("Incompatible LayoutMode: " + this.LayoutMode.ToString());
				}
			}
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x0008F0CE File Offset: 0x0008D2CE
		protected internal virtual void OnMouseDragEnter(object data, Element sourceElement)
		{
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x0008F0D1 File Offset: 0x0008D2D1
		protected internal virtual void OnMouseDragExit(object data, Element sourceElement)
		{
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x0008F0D4 File Offset: 0x0008D2D4
		protected internal virtual void OnMouseDragMove()
		{
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x0008F0D7 File Offset: 0x0008D2D7
		protected internal virtual void OnMouseDrop(object data, Element sourceElement, out bool accepted)
		{
			accepted = false;
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x0008F0DD File Offset: 0x0008D2DD
		protected internal virtual void OnMouseDragCancel(object data)
		{
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x0008F0E0 File Offset: 0x0008D2E0
		protected internal virtual void OnMouseDragComplete(Element element, object data)
		{
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x0008F0E4 File Offset: 0x0008D2E4
		protected internal virtual void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			bool flag = keycode == SDL.SDL_Keycode.SDLK_RETURN || keycode == SDL.SDL_Keycode.SDLK_KP_ENTER;
			if (flag)
			{
				this.Validate();
			}
			else
			{
				bool flag2 = keycode == SDL.SDL_Keycode.SDLK_ESCAPE;
				if (flag2)
				{
					this.Dismiss();
				}
			}
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x0008F11F File Offset: 0x0008D31F
		protected internal virtual void OnKeyUp(SDL.SDL_Keycode keycode)
		{
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x0008F122 File Offset: 0x0008D322
		protected internal virtual void OnTextInput(string text)
		{
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x0008F128 File Offset: 0x0008D328
		public void PrepareForDraw()
		{
			bool flag = !this.Visible;
			if (!flag)
			{
				Debug.Assert(!this._waitingForLayoutAfterMount, "Element was mounted but not laid out", "{0}", new object[]
				{
					this
				});
				bool flag2 = this._maskTextureArea != null;
				if (flag2)
				{
					this.Desktop.Batcher2D.PushMask(this._maskTextureArea, this._anchoredRectangle, this.Desktop.ViewportRectangle);
				}
				this.PrepareForDrawSelf();
				bool flag3 = this._scaledScrollArea != Point.Zero;
				if (flag3)
				{
					this.Desktop.Batcher2D.PushScissor(this._rectangleAfterPadding);
				}
				this.PrepareForDrawContent();
				bool flag4 = this._scaledScrollArea != Point.Zero;
				if (flag4)
				{
					this.Desktop.Batcher2D.PopScissor();
				}
				bool flag5 = this._scaledScrollArea != Point.Zero && (!this._scrollbarStyle.OnlyVisibleWhenHovered || this.IsHovered);
				if (flag5)
				{
					int num = this.Desktop.ScaleRound((float)this._scrollbarStyle.Size);
					bool flag6 = this._scaledScrollArea.X > 0;
					if (flag6)
					{
						int num2 = this._viewRectangle.Width - this._horizontalScrollbarLength;
						bool flag7 = this._scrollbarBackgroundPatch != null;
						if (flag7)
						{
							this.Desktop.Batcher2D.RequestDrawPatch(this._scrollbarBackgroundPatch, new Rectangle(this._rectangleAfterPadding.X, this._rectangleAfterPadding.Bottom - num, this._rectangleAfterPadding.Width, num), this.Desktop.Scale);
						}
						bool flag8 = num2 > 0;
						if (flag8)
						{
							TexturePatch scrollbarStatePatch = this.GetScrollbarStatePatch(this._horizontalScrollbarState);
							bool flag9 = scrollbarStatePatch != null;
							if (flag9)
							{
								this.Desktop.Batcher2D.RequestDrawPatch(scrollbarStatePatch, this._horizontalScrollbarHandleRectangle, this.Desktop.Scale);
							}
						}
					}
					bool flag10 = this._scaledScrollArea.Y > 0;
					if (flag10)
					{
						int num3 = this._viewRectangle.Height - this._verticalScrollbarLength;
						bool flag11 = this._scrollbarBackgroundPatch != null;
						if (flag11)
						{
							this.Desktop.Batcher2D.RequestDrawPatch(this._scrollbarBackgroundPatch, new Rectangle(this._rectangleAfterPadding.Right - num, this._rectangleAfterPadding.Y, num, this._rectangleAfterPadding.Height), this.Desktop.Scale);
						}
						bool flag12 = num3 > 0;
						if (flag12)
						{
							TexturePatch scrollbarStatePatch2 = this.GetScrollbarStatePatch(this._verticalScrollbarState);
							bool flag13 = scrollbarStatePatch2 != null;
							if (flag13)
							{
								this.Desktop.Batcher2D.RequestDrawPatch(scrollbarStatePatch2, this._verticalScrollbarHandleRectangle, this.Desktop.Scale);
							}
						}
					}
				}
				bool flag14 = this._maskTextureArea != null;
				if (flag14)
				{
					this.Desktop.Batcher2D.PopMask();
				}
			}
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x0008F414 File Offset: 0x0008D614
		protected virtual void PrepareForDrawContent()
		{
			foreach (Element element in this._children)
			{
				element.PrepareForDraw();
			}
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x0008F46C File Offset: 0x0008D66C
		protected virtual void PrepareForDrawSelf()
		{
			bool flag = this._backgroundPatch != null;
			if (flag)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._backgroundPatch, this._backgroundRectangle, this.Desktop.Scale);
			}
			bool flag2 = this.OutlineSize > 0f;
			if (flag2)
			{
				TextureArea whitePixel = this.Desktop.Provider.WhitePixel;
				this.Desktop.Batcher2D.RequestDrawOutline(whitePixel.Texture, whitePixel.Rectangle, this._anchoredRectangle, this.OutlineSize * this.Desktop.Scale, this.OutlineColor);
			}
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x0008F510 File Offset: 0x0008D710
		private TexturePatch GetScrollbarStatePatch(Element.ScrollbarState state)
		{
			TexturePatch result;
			if (state != Element.ScrollbarState.Hovered)
			{
				if (state != Element.ScrollbarState.Dragged)
				{
					result = this._scrollbarHandlePatch;
				}
				else
				{
					result = (this._scrollbarDraggedHandlePatch ?? this._scrollbarHandlePatch);
				}
			}
			else
			{
				result = (this._scrollbarHoveredHandlePatch ?? this._scrollbarHandlePatch);
			}
			return result;
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x0008F560 File Offset: 0x0008D760
		public virtual void PrepareForDrawOutline()
		{
			bool flag = !this.Visible;
			if (!flag)
			{
				UInt32Color color = UInt32Color.FromRGBA(4278190144U);
				bool flag2 = this.CapturedMouseButton != null;
				if (flag2)
				{
					color = UInt32Color.FromRGBA(16711808U);
				}
				else
				{
					bool isHovered = this.IsHovered;
					if (isHovered)
					{
						color = UInt32Color.FromRGBA(65408U);
					}
				}
				TextureArea whitePixel = this.Desktop.Provider.WhitePixel;
				this.Desktop.Batcher2D.RequestDrawOutline(whitePixel.Texture, whitePixel.Rectangle, this._anchoredRectangle, 1f, color);
				foreach (Element element in this._children)
				{
					element.PrepareForDrawOutline();
				}
			}
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x0008F648 File Offset: 0x0008D848
		protected Point ComputeScaledAnchorAndPaddingSize(int? maxWidth)
		{
			int num = this.Anchor.Left.GetValueOrDefault() + this.Anchor.Right.GetValueOrDefault();
			bool flag = this.Anchor.MaxWidth != null;
			if (flag)
			{
				num += Math.Min(this.Anchor.MaxWidth.Value, maxWidth.GetValueOrDefault(int.MaxValue));
			}
			else
			{
				num += (this.Anchor.Width ?? (this.Padding.Left.GetValueOrDefault() + this.Padding.Right.GetValueOrDefault()));
			}
			int num2 = this.Anchor.Top.GetValueOrDefault() + this.Anchor.Bottom.GetValueOrDefault();
			num2 += (this.Anchor.Height ?? (this.Padding.Top.GetValueOrDefault() + this.Padding.Bottom.GetValueOrDefault()));
			return new Point(this.Desktop.ScaleRound((float)num), this.Desktop.ScaleRound((float)num2));
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x0008F784 File Offset: 0x0008D984
		public virtual Point ComputeScaledMinSize(int? maxWidth, int? maxHeight)
		{
			Point zero = Point.Zero;
			int num = this.Desktop.ScaleRound((float)(this._scrollbarStyle.Size + this._scrollbarStyle.Spacing));
			bool flag = this._layoutMode == LayoutMode.LeftScrolling || this._layoutMode == LayoutMode.RightScrolling;
			bool flag2 = this._layoutMode == LayoutMode.TopScrolling || this._layoutMode == LayoutMode.BottomScrolling;
			int? num2 = flag ? null : maxWidth;
			bool flag3 = !flag && (this.Anchor.MaxWidth != null || this.Anchor.Width != null);
			if (flag3)
			{
				num2 = new int?(this.Desktop.ScaleRound((float)(this.Anchor.MaxWidth ?? this.Anchor.Width.Value)));
			}
			else
			{
				bool flag4 = num2 != null && flag2;
				if (flag4)
				{
					num2 -= num;
				}
			}
			int? maxHeight2 = flag2 ? null : maxHeight;
			bool flag5 = !flag2 && this.Anchor.Height != null;
			if (flag5)
			{
				maxHeight2 = new int?(this.Desktop.ScaleRound((float)this.Anchor.Height.Value));
			}
			bool flag6 = num2 != null;
			if (flag6)
			{
				int num3 = this.Desktop.ScaleRound((float)(this.Padding.Left.GetValueOrDefault() + this.Padding.Right.GetValueOrDefault()));
				num2 -= num3;
			}
			switch (this._layoutMode)
			{
			case LayoutMode.Full:
				foreach (Element element in this._children)
				{
					bool flag7 = !element.Visible;
					if (!flag7)
					{
						Point point = element.ComputeScaledMinSize(num2, maxHeight2);
						bool flag8 = point.X > zero.X;
						if (flag8)
						{
							zero.X = point.X;
						}
						bool flag9 = point.Y > zero.Y;
						if (flag9)
						{
							zero.Y = point.Y;
						}
					}
				}
				break;
			case LayoutMode.Left:
			case LayoutMode.Center:
			case LayoutMode.Right:
			case LayoutMode.LeftScrolling:
			case LayoutMode.RightScrolling:
			case LayoutMode.CenterMiddle:
				foreach (Element element2 in this._children)
				{
					bool flag10 = !element2.Visible;
					if (!flag10)
					{
						Point point2 = element2.ComputeScaledMinSize(num2, maxHeight2);
						zero.X += point2.X;
						bool flag11 = point2.Y > zero.Y;
						if (flag11)
						{
							zero.Y = point2.Y;
						}
					}
				}
				break;
			case LayoutMode.Top:
			case LayoutMode.Middle:
			case LayoutMode.Bottom:
			case LayoutMode.TopScrolling:
			case LayoutMode.BottomScrolling:
			case LayoutMode.MiddleCenter:
				foreach (Element element3 in this._children)
				{
					bool flag12 = !element3.Visible;
					if (!flag12)
					{
						Point point3 = element3.ComputeScaledMinSize(num2, maxHeight2);
						zero.Y += point3.Y;
						bool flag13 = point3.X > zero.X;
						if (flag13)
						{
							zero.X = point3.X;
						}
					}
				}
				break;
			default:
				throw new NotImplementedException();
			}
			Point point4 = this.ComputeScaledAnchorAndPaddingSize(maxWidth);
			bool flag14 = this.Anchor.Width == null && this.Anchor.MaxWidth == null;
			if (flag14)
			{
				point4.X += zero.X;
				bool flag15 = this._layoutMode == LayoutMode.TopScrolling || this._layoutMode == LayoutMode.BottomScrolling;
				if (flag15)
				{
					point4.X += num;
				}
				bool flag16 = maxWidth != null;
				if (flag16)
				{
					point4.X = Math.Min(point4.X, maxWidth.Value);
				}
			}
			bool flag17 = this.Anchor.MinWidth != null;
			if (flag17)
			{
				point4.X = Math.Max(this.Desktop.ScaleRound((float)this.Anchor.MinWidth.Value), point4.X);
			}
			bool flag18 = this.Anchor.Height == null;
			if (flag18)
			{
				point4.Y += zero.Y;
				bool flag19 = this._layoutMode == LayoutMode.LeftScrolling || this._layoutMode == LayoutMode.RightScrolling;
				if (flag19)
				{
					point4.Y += num;
				}
				bool flag20 = maxHeight != null;
				if (flag20)
				{
					point4.Y = Math.Min(point4.Y, maxHeight.Value);
				}
			}
			return point4;
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x0008FD1C File Offset: 0x0008DF1C
		internal void Rescale(float scaleRatio)
		{
			this._scaledScrollOffset = new Point(MathHelper.Round((float)this._scaledScrollOffset.X * scaleRatio), MathHelper.Round((float)this._scaledScrollOffset.Y * scaleRatio));
			foreach (Element element in this._children)
			{
				element.Rescale(scaleRatio);
			}
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x0008FDA4 File Offset: 0x0008DFA4
		protected virtual float? GetScaledWidth()
		{
			bool flag = this.Anchor.MaxWidth != null;
			float? result;
			if (flag)
			{
				result = new float?(Math.Min((float)this.Anchor.MaxWidth.Value * this.Desktop.Scale, (float)this._containerRectangle.Width));
			}
			else
			{
				bool flag2 = this.Anchor.Width != null;
				if (flag2)
				{
					result = new float?((float)this.Anchor.Width.Value * this.Desktop.Scale);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x0008FE50 File Offset: 0x0008E050
		protected virtual float? GetScaledHeight()
		{
			bool flag = this.Anchor.Height != null;
			float? result;
			if (flag)
			{
				result = new float?((float)this.Anchor.Height.Value * this.Desktop.Scale);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x0008FEA4 File Offset: 0x0008E0A4
		public void Layout(Rectangle? containerRectangle = null, bool layoutChildren = true)
		{
			bool flag = containerRectangle != null;
			if (flag)
			{
				this._containerRectangle = containerRectangle.Value;
				this._waitingForLayoutAfterMount = false;
			}
			bool flag2 = !this.IsMounted || this._waitingForLayoutAfterMount;
			if (!flag2)
			{
				this.ApplyStyles();
				float num = (float)this._containerRectangle.Left + (float)this.Anchor.Left.GetValueOrDefault() * this.Desktop.Scale;
				float num2 = (float)this._containerRectangle.Right - (float)this.Anchor.Right.GetValueOrDefault() * this.Desktop.Scale;
				float num3 = (float)this._containerRectangle.Top + (float)this.Anchor.Top.GetValueOrDefault() * this.Desktop.Scale;
				float num4 = (float)this._containerRectangle.Bottom - (float)this.Anchor.Bottom.GetValueOrDefault() * this.Desktop.Scale;
				float? scaledWidth = this.GetScaledWidth();
				bool flag3 = scaledWidth != null;
				if (flag3)
				{
					float value = scaledWidth.Value;
					bool flag4 = this.Anchor.Left != null;
					if (flag4)
					{
						bool flag5 = this.Anchor.Right == null;
						if (flag5)
						{
							num2 = num + value;
						}
					}
					else
					{
						bool flag6 = this.Anchor.Right != null;
						if (flag6)
						{
							num = num2 - value;
						}
						else
						{
							num = (float)this._containerRectangle.Center.X - value / 2f;
							num2 = num + value;
						}
					}
				}
				float? scaledHeight = this.GetScaledHeight();
				bool flag7 = scaledHeight != null;
				if (flag7)
				{
					bool flag8 = this.Anchor.Top != null;
					if (flag8)
					{
						bool flag9 = this.Anchor.Bottom == null;
						if (flag9)
						{
							num4 = num3 + scaledHeight.Value;
						}
					}
					else
					{
						bool flag10 = this.Anchor.Bottom != null;
						if (flag10)
						{
							num3 = num4 - scaledHeight.Value;
						}
						else
						{
							num3 = (float)this._containerRectangle.Center.Y - scaledHeight.Value / 2f;
							num4 = num3 + scaledHeight.Value;
						}
					}
				}
				int num5 = MathHelper.Round(num);
				int num6 = MathHelper.Round(num2);
				int num7 = MathHelper.Round(num3);
				int num8 = MathHelper.Round(num4);
				int num9 = MathHelper.Round(num + (float)this.Padding.Left.GetValueOrDefault() * this.Desktop.Scale);
				int num10 = MathHelper.Round(num2 - (float)this.Padding.Right.GetValueOrDefault() * this.Desktop.Scale);
				int num11 = MathHelper.Round(num3 + (float)this.Padding.Top.GetValueOrDefault() * this.Desktop.Scale);
				int num12 = MathHelper.Round(num4 - (float)this.Padding.Bottom.GetValueOrDefault() * this.Desktop.Scale);
				this._anchoredRectangle = new Rectangle(num5, num7, num6 - num5, num8 - num7);
				this._rectangleAfterPadding = new Rectangle(num9, num11, num10 - num9, num12 - num11);
				PatchStyle background = this.Background;
				bool flag11 = background != null && background.Anchor != null;
				if (flag11)
				{
					Anchor value2 = this.Background.Anchor.Value;
					float num13 = (float)this._anchoredRectangle.Left + (float)value2.Left.GetValueOrDefault() * this.Desktop.Scale;
					float num14 = (float)this._anchoredRectangle.Right - (float)value2.Right.GetValueOrDefault() * this.Desktop.Scale;
					float num15 = (float)this._anchoredRectangle.Top + (float)value2.Top.GetValueOrDefault() * this.Desktop.Scale;
					float num16 = (float)this._anchoredRectangle.Bottom - (float)value2.Bottom.GetValueOrDefault() * this.Desktop.Scale;
					bool flag12 = value2.Width != null;
					if (flag12)
					{
						float num17 = (float)value2.Width.Value * this.Desktop.Scale;
						bool flag13 = value2.Left != null;
						if (flag13)
						{
							bool flag14 = value2.Right == null;
							if (flag14)
							{
								num14 = num13 + num17;
							}
						}
						else
						{
							bool flag15 = value2.Right != null;
							if (flag15)
							{
								num13 = num14 - num17;
							}
							else
							{
								num13 = (float)this._anchoredRectangle.Center.X - num17 / 2f;
								num14 = num13 + num17;
							}
						}
					}
					bool flag16 = value2.Height != null;
					if (flag16)
					{
						float num18 = (float)value2.Height.Value * this.Desktop.Scale;
						bool flag17 = value2.Top != null;
						if (flag17)
						{
							bool flag18 = value2.Bottom == null;
							if (flag18)
							{
								num16 = num15 + num18;
							}
						}
						else
						{
							bool flag19 = value2.Bottom != null;
							if (flag19)
							{
								num15 = num16 - num18;
							}
							else
							{
								num15 = (float)this._anchoredRectangle.Center.Y - num18 / 2f;
								num16 = num15 + num18;
							}
						}
					}
					this._backgroundRectangle = new Rectangle(MathHelper.Round(num13), MathHelper.Round(num15), MathHelper.Round(num14 - num13), MathHelper.Round(num16 - num15));
				}
				else
				{
					this._backgroundRectangle = this._anchoredRectangle;
				}
				this.LayoutSelf();
				bool flag20 = this._children.Count == 0 && this.ContentWidth == null && this.ContentHeight == null && layoutChildren;
				if (!flag20)
				{
					this.LayoutChildren();
					this.AfterChildrenLayout();
				}
			}
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x00090480 File Offset: 0x0008E680
		protected void LayoutChildren()
		{
			int? num = new int?(this._rectangleAfterPadding.Width);
			int? num2 = new int?(this._rectangleAfterPadding.Height);
			int num3 = this.Desktop.ScaleRound((float)(this._scrollbarStyle.Size + this._scrollbarStyle.Spacing));
			this._scaledScrollArea = new Point(this.Desktop.ScaleRound((float)this.ContentWidth.GetValueOrDefault()), this.Desktop.ScaleRound((float)this.ContentHeight.GetValueOrDefault()));
			bool flag = false;
			LayoutMode layoutMode = this._layoutMode;
			LayoutMode layoutMode2 = layoutMode;
			if (layoutMode2 - LayoutMode.LeftScrolling > 1)
			{
				if (layoutMode2 - LayoutMode.TopScrolling <= 1)
				{
					flag = true;
					num2 = null;
					num -= num3;
					int num4 = 0;
					foreach (Element element in this._children)
					{
						num4 += (element.Visible ? element.ComputeScaledMinSize(num, null).Y : 0);
					}
					this._scaledScrollArea.Y = num4;
					bool overscroll = this.Overscroll;
					if (overscroll)
					{
						this._scaledScrollArea.Y = this._scaledScrollArea.Y + this._rectangleAfterPadding.Height;
					}
				}
			}
			else
			{
				flag = true;
				num = null;
				num2 -= num3;
				int num5 = 0;
				foreach (Element element2 in this._children)
				{
					num5 += (element2.Visible ? element2.ComputeScaledMinSize(null, num2).X : 0);
				}
				this._scaledScrollArea.X = num5;
				bool overscroll2 = this.Overscroll;
				if (overscroll2)
				{
					this._scaledScrollArea.Y = this._scaledScrollArea.Y + this._rectangleAfterPadding.Height;
				}
			}
			this._viewRectangle = this._rectangleAfterPadding;
			bool flag2 = this._scaledScrollArea != Point.Zero;
			if (flag2)
			{
				bool flag3 = this._scaledScrollArea.X > 0;
				if (flag3)
				{
					this._viewRectangle.Height = this._viewRectangle.Height - num3;
				}
				bool flag4 = this._scaledScrollArea.Y > 0;
				if (flag4)
				{
					this._viewRectangle.Width = this._viewRectangle.Width - num3;
				}
				bool flag5 = this._scaledScrollArea.X > 0;
				if (flag5)
				{
					this._horizontalScrollbarLength = Math.Max(10, MathHelper.Round((float)this._viewRectangle.Width * Math.Min(1f, (float)this._viewRectangle.Width / (float)this._scaledScrollArea.X)));
				}
				bool flag6 = this._scaledScrollArea.Y > 0;
				if (flag6)
				{
					this._verticalScrollbarLength = Math.Max(10, MathHelper.Round((float)this._viewRectangle.Height * Math.Min(1f, (float)this._viewRectangle.Height / (float)this._scaledScrollArea.Y)));
				}
				this.ComputeScrollbars();
			}
			this._contentRectangle = this._rectangleAfterPadding;
			bool flag7 = this._scaledScrollArea.X > 0;
			if (flag7)
			{
				bool flag8 = this._layoutMode == LayoutMode.LeftScrolling;
				if (flag8)
				{
					this._contentRectangle.Width = this._scaledScrollArea.X;
				}
				else
				{
					bool flag9 = this._layoutMode == LayoutMode.RightScrolling;
					if (flag9)
					{
						this._contentRectangle.Width = Math.Max(this._rectangleAfterPadding.Width, this._scaledScrollArea.X);
					}
				}
			}
			bool flag10 = this._scaledScrollArea.Y > 0;
			if (flag10)
			{
				bool flag11 = this._layoutMode == LayoutMode.TopScrolling;
				if (flag11)
				{
					this._contentRectangle.Height = this._scaledScrollArea.Y;
				}
				else
				{
					bool flag12 = this._layoutMode == LayoutMode.BottomScrolling;
					if (flag12)
					{
						this._contentRectangle.Height = Math.Max(this._rectangleAfterPadding.Height, this._scaledScrollArea.Y);
					}
				}
			}
			bool flag13 = this._scaledScrollArea.X > 0;
			if (flag13)
			{
				this._contentRectangle.Height = this._contentRectangle.Height - num3;
			}
			bool flag14 = this._scaledScrollArea.Y > 0;
			if (flag14)
			{
				this._contentRectangle.Width = this._contentRectangle.Width - num3;
			}
			this._contentRectangle.Offset(-this._scaledScrollOffset.X, -this._scaledScrollOffset.Y);
			switch (this._layoutMode)
			{
			case LayoutMode.Full:
				foreach (Element element3 in this._children)
				{
					bool visible = element3.Visible;
					if (visible)
					{
						element3.Layout(new Rectangle?(this._contentRectangle), true);
					}
				}
				break;
			case LayoutMode.Left:
			case LayoutMode.Center:
			case LayoutMode.LeftScrolling:
			case LayoutMode.CenterMiddle:
			{
				float num6 = 0f;
				int num7 = 0;
				bool flag15 = false;
				foreach (Element element4 in this._children)
				{
					bool flag16 = !element4.Visible;
					if (!flag16)
					{
						bool flag17 = !flag && element4.FlexWeight > 0;
						if (flag17)
						{
							num7 += element4.FlexWeight;
							bool flag18 = element4.Anchor.MinWidth != null;
							if (flag18)
							{
								flag15 = true;
							}
						}
						else
						{
							num6 += (float)element4.ComputeScaledMinSize(num, num2).X;
						}
					}
				}
				float num8 = Math.Max(0f, (float)this._contentRectangle.Width - num6);
				bool flag19 = flag15;
				if (flag19)
				{
					foreach (Element element5 in this._children)
					{
						bool flag20 = !element5.Visible || element5.FlexWeight <= 0 || element5.Anchor.MinWidth == null;
						if (!flag20)
						{
							int num9 = (int)(num8 * (float)element5.FlexWeight / (float)num7);
							int num10 = this.Desktop.ScaleRound((float)element5.Anchor.MinWidth.Value);
							bool flag21 = num9 < num10;
							if (flag21)
							{
								num7 -= element5.FlexWeight;
								num8 -= (float)num10;
							}
						}
					}
				}
				int num11 = this._contentRectangle.X;
				bool flag22 = (this._layoutMode == LayoutMode.Center || this._layoutMode == LayoutMode.CenterMiddle) && num7 == 0;
				if (flag22)
				{
					num11 += (this._contentRectangle.Width - (int)num6) / 2;
				}
				foreach (Element element6 in this._children)
				{
					bool flag23 = !element6.Visible;
					if (!flag23)
					{
						Point point = element6.ComputeScaledMinSize(num, num2);
						bool flag24 = !flag && element6.FlexWeight > 0;
						if (flag24)
						{
							int num12 = (int)(num8 * (float)element6.FlexWeight / (float)num7);
							bool flag25 = element6.Anchor.MinWidth != null;
							if (flag25)
							{
								int num13 = this.Desktop.ScaleRound((float)element6.Anchor.MinWidth.Value);
								point.X = ((num12 > num13) ? num12 : num13);
							}
							else
							{
								point.X = num12;
							}
						}
						bool flag26 = this._layoutMode != LayoutMode.CenterMiddle;
						if (flag26)
						{
							point.Y = this._contentRectangle.Height;
						}
						element6.Layout(new Rectangle?(new Rectangle(num11, this._contentRectangle.Center.Y - point.Y / 2, point.X, point.Y)), true);
						num11 += point.X;
					}
				}
				break;
			}
			case LayoutMode.Right:
			case LayoutMode.RightScrolling:
			{
				float num14 = 0f;
				int num15 = 0;
				bool flag27 = false;
				foreach (Element element7 in this._children)
				{
					bool flag28 = !element7.Visible;
					if (!flag28)
					{
						bool flag29 = !flag && element7.FlexWeight > 0;
						if (flag29)
						{
							num15 += element7.FlexWeight;
						}
						else
						{
							num14 += (float)element7.ComputeScaledMinSize(num, num2).X;
						}
					}
				}
				float num16 = Math.Max(0f, (float)this._contentRectangle.Width - num14);
				bool flag30 = flag27;
				if (flag30)
				{
					foreach (Element element8 in this._children)
					{
						bool flag31 = !element8.Visible || element8.FlexWeight <= 0 || element8.Anchor.MinWidth == null;
						if (!flag31)
						{
							int num17 = (int)(num16 * (float)element8.FlexWeight / (float)num15);
							int num18 = this.Desktop.ScaleRound((float)element8.Anchor.MinWidth.Value);
							bool flag32 = num17 < num18;
							if (flag32)
							{
								num15 -= element8.FlexWeight;
								num16 -= (float)num18;
							}
						}
					}
				}
				int num19 = this._contentRectangle.Right;
				for (int i = this._children.Count - 1; i >= 0; i--)
				{
					Element element9 = this._children[i];
					bool flag33 = !element9.Visible;
					if (!flag33)
					{
						Point point2 = element9.ComputeScaledMinSize(num, num2);
						bool flag34 = !flag && element9.FlexWeight > 0;
						if (flag34)
						{
							int num20 = (int)(num16 * (float)element9.FlexWeight / (float)num15);
							bool flag35 = element9.Anchor.MinWidth != null;
							if (flag35)
							{
								int num21 = this.Desktop.ScaleRound((float)element9.Anchor.MinWidth.Value);
								point2.X = ((num20 > num21) ? num20 : num21);
							}
							else
							{
								point2.X = num20;
							}
						}
						num19 -= point2.X;
						element9.Layout(new Rectangle?(new Rectangle(num19, this._contentRectangle.Y, point2.X, this._contentRectangle.Height)), true);
					}
				}
				break;
			}
			case LayoutMode.Top:
			case LayoutMode.Middle:
			case LayoutMode.TopScrolling:
			case LayoutMode.MiddleCenter:
			{
				float num22 = 0f;
				int num23 = 0;
				foreach (Element element10 in this._children)
				{
					bool flag36 = !element10.Visible;
					if (!flag36)
					{
						bool flag37 = !flag && element10.FlexWeight > 0;
						if (flag37)
						{
							num23 += element10.FlexWeight;
						}
						else
						{
							num22 += (float)element10.ComputeScaledMinSize(num, num2).Y;
						}
					}
				}
				float num24 = Math.Max(0f, (float)this._contentRectangle.Height - num22);
				int num25 = this._contentRectangle.Y;
				bool flag38 = (this._layoutMode == LayoutMode.Middle || this._layoutMode == LayoutMode.MiddleCenter) && num23 == 0;
				if (flag38)
				{
					num25 += (this._contentRectangle.Height - (int)num22) / 2;
				}
				foreach (Element element11 in this._children)
				{
					bool flag39 = !element11.Visible;
					if (!flag39)
					{
						Point point3 = element11.ComputeScaledMinSize(num, num2);
						bool flag40 = !flag && element11.FlexWeight > 0;
						if (flag40)
						{
							point3.Y = (int)(num24 * (float)element11.FlexWeight / (float)num23);
						}
						bool flag41 = this._layoutMode != LayoutMode.MiddleCenter;
						if (flag41)
						{
							point3.X = this._contentRectangle.Width;
						}
						element11.Layout(new Rectangle?(new Rectangle(this._contentRectangle.Center.X - point3.X / 2, num25, point3.X, point3.Y)), true);
						num25 += point3.Y;
					}
				}
				break;
			}
			case LayoutMode.Bottom:
			case LayoutMode.BottomScrolling:
			{
				float num26 = 0f;
				int num27 = 0;
				foreach (Element element12 in this._children)
				{
					bool flag42 = !element12.Visible;
					if (!flag42)
					{
						bool flag43 = !flag && element12.FlexWeight > 0;
						if (flag43)
						{
							num27 += element12.FlexWeight;
						}
						else
						{
							num26 += (float)element12.ComputeScaledMinSize(num, num2).Y;
						}
					}
				}
				float num28 = Math.Max(0f, (float)this._contentRectangle.Height - num26);
				int num29 = this._contentRectangle.Bottom;
				for (int j = this._children.Count - 1; j >= 0; j--)
				{
					Element element13 = this._children[j];
					bool flag44 = !element13.Visible;
					if (!flag44)
					{
						bool flag45 = !flag && element13.FlexWeight > 0;
						int num30;
						if (flag45)
						{
							num30 = (int)(num28 * (float)element13.FlexWeight / (float)num27);
						}
						else
						{
							num30 = element13.ComputeScaledMinSize(num, num2).Y;
						}
						num29 -= num30;
						element13.Layout(new Rectangle?(new Rectangle(this._contentRectangle.X, num29, this._contentRectangle.Width, num30)), true);
					}
				}
				break;
			}
			}
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x0009144C File Offset: 0x0008F64C
		protected virtual void ApplyStyles()
		{
			this._backgroundPatch = ((this.Background != null) ? this.Desktop.MakeTexturePatch(this.Background) : null);
			this._maskTextureArea = ((this.MaskTexturePath != null) ? this.Desktop.Provider.MakeTextureArea(this.MaskTexturePath.Value) : null);
			this._scrollbarBackgroundPatch = ((this._scrollbarStyle.Background != null) ? this.Desktop.MakeTexturePatch(this._scrollbarStyle.Background) : null);
			this._scrollbarHandlePatch = ((this._scrollbarStyle.Handle != null) ? this.Desktop.MakeTexturePatch(this._scrollbarStyle.Handle) : null);
			this._scrollbarHoveredHandlePatch = ((this._scrollbarStyle.HoveredHandle != null) ? this.Desktop.MakeTexturePatch(this._scrollbarStyle.HoveredHandle) : null);
			this._scrollbarDraggedHandlePatch = ((this._scrollbarStyle.DraggedHandle != null) ? this.Desktop.MakeTexturePatch(this._scrollbarStyle.DraggedHandle) : null);
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x00091559 File Offset: 0x0008F759
		protected virtual void LayoutSelf()
		{
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x0009155C File Offset: 0x0008F75C
		protected virtual void AfterChildrenLayout()
		{
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x00091560 File Offset: 0x0008F760
		public void SetScroll(int? x = null, int? y = null)
		{
			Point scaledScrollOffset = this._scaledScrollOffset;
			this._scaledScrollOffset = new Point(x ?? this._scaledScrollOffset.X, y ?? this._scaledScrollOffset.Y);
			this.ComputeScrollbars();
			Point point = new Point(this._scaledScrollOffset.X - scaledScrollOffset.X, this._scaledScrollOffset.Y - scaledScrollOffset.Y);
			bool flag = point == Point.Zero;
			if (!flag)
			{
				foreach (Element element in this._children)
				{
					element.ApplyParentScroll(point);
				}
			}
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x00091650 File Offset: 0x0008F850
		protected void ComputeScrollbars()
		{
			bool flag = this._verticalScrollbarState != Element.ScrollbarState.Dragged && this._scaledScrollOffset.Y >= this._scaledScrollSize.Y;
			this._scaledScrollSize = new Point(Math.Max(0, this._scaledScrollArea.X - this._viewRectangle.Width), Math.Max(0, this._scaledScrollArea.Y - this._viewRectangle.Height));
			this._scaledScrollOffset = new Point(MathHelper.Clamp(this._scaledScrollOffset.X, 0, this._scaledScrollSize.X), MathHelper.Clamp(this._scaledScrollOffset.Y, 0, this._scaledScrollSize.Y));
			bool flag2 = this.AutoScrollDown && flag;
			if (flag2)
			{
				this._scaledScrollOffset.Y = this._scaledScrollSize.Y;
			}
			int num = this.Desktop.ScaleRound((float)this._scrollbarStyle.Size);
			Point scrollbarOffsets = this._scrollbarOffsets;
			int num2 = this._viewRectangle.Width - this._horizontalScrollbarLength;
			this._scrollbarOffsets.X = MathHelper.Round((float)num2 * (float)this._scaledScrollOffset.X / (float)this._scaledScrollSize.X);
			this._horizontalScrollbarHandleRectangle = new Rectangle(this._rectangleAfterPadding.X + this._scrollbarOffsets.X, this._rectangleAfterPadding.Bottom - num, this._horizontalScrollbarLength, num);
			int num3 = this._viewRectangle.Height - this._verticalScrollbarLength;
			this._scrollbarOffsets.Y = MathHelper.Round((float)num3 * (float)this._scaledScrollOffset.Y / (float)this._scaledScrollSize.Y);
			this._verticalScrollbarHandleRectangle = new Rectangle(this._rectangleAfterPadding.Right - num, this._rectangleAfterPadding.Y + this._scrollbarOffsets.Y, num, this._verticalScrollbarLength);
			bool flag3 = scrollbarOffsets != this._scrollbarOffsets;
			if (flag3)
			{
				Action scrolled = this._scrolled;
				if (scrolled != null)
				{
					scrolled();
				}
			}
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x00091864 File Offset: 0x0008FA64
		protected virtual void ApplyParentScroll(Point scaledParentScroll)
		{
			this._containerRectangle.Offset(-scaledParentScroll.X, -scaledParentScroll.Y);
			this._anchoredRectangle.Offset(-scaledParentScroll.X, -scaledParentScroll.Y);
			this._backgroundRectangle.Offset(-scaledParentScroll.X, -scaledParentScroll.Y);
			this._rectangleAfterPadding.Offset(-scaledParentScroll.X, -scaledParentScroll.Y);
			this._viewRectangle.Offset(-scaledParentScroll.X, -scaledParentScroll.Y);
			this._contentRectangle.Offset(-scaledParentScroll.X, -scaledParentScroll.Y);
			this._horizontalScrollbarHandleRectangle.Offset(-scaledParentScroll.X, -scaledParentScroll.Y);
			this._verticalScrollbarHandleRectangle.Offset(-scaledParentScroll.X, -scaledParentScroll.Y);
			foreach (Element element in this._children)
			{
				element.ApplyParentScroll(scaledParentScroll);
			}
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x0009198C File Offset: 0x0008FB8C
		public virtual Element HitTest(Point position)
		{
			Debug.Assert(this.IsMounted);
			bool flag = this._waitingForLayoutAfterMount || !this._anchoredRectangle.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				for (int i = this._children.Count - 1; i >= 0; i--)
				{
					Element element = this._children[i];
					bool flag2 = !element.IsMounted;
					if (!flag2)
					{
						Element element2 = element.HitTest(position);
						bool flag3 = element2 != null;
						if (flag3)
						{
							return element2;
						}
					}
				}
				bool flag4 = this._scaledScrollArea != Point.Zero;
				if (flag4)
				{
					result = this;
				}
				else
				{
					bool hasTooltipText = this._hasTooltipText;
					if (hasTooltipText)
					{
						result = this;
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x04001B7F RID: 7039
		public readonly Desktop Desktop;

		// Token: 0x04001B80 RID: 7040
		public Element Parent;

		// Token: 0x04001B82 RID: 7042
		protected bool _waitingForLayoutAfterMount;

		// Token: 0x04001B85 RID: 7045
		private bool _visible = true;

		// Token: 0x04001B86 RID: 7046
		[UIMarkupProperty]
		public Anchor Anchor;

		// Token: 0x04001B87 RID: 7047
		[UIMarkupProperty]
		public Padding Padding;

		// Token: 0x04001B88 RID: 7048
		[UIMarkupProperty]
		public int FlexWeight = 0;

		// Token: 0x04001B89 RID: 7049
		protected Rectangle _containerRectangle;

		// Token: 0x04001B8A RID: 7050
		protected Rectangle _anchoredRectangle;

		// Token: 0x04001B8B RID: 7051
		protected Rectangle _rectangleAfterPadding;

		// Token: 0x04001B8C RID: 7052
		protected Rectangle _backgroundRectangle;

		// Token: 0x04001B8D RID: 7053
		protected LayoutMode _layoutMode = LayoutMode.Full;

		// Token: 0x04001B8E RID: 7054
		[UIMarkupProperty]
		public int? ContentWidth;

		// Token: 0x04001B8F RID: 7055
		[UIMarkupProperty]
		public int? ContentHeight;

		// Token: 0x04001B90 RID: 7056
		[UIMarkupProperty]
		public bool AutoScrollDown;

		// Token: 0x04001B91 RID: 7057
		[UIMarkupProperty]
		public bool KeepScrollPosition = false;

		// Token: 0x04001B92 RID: 7058
		protected Action _scrolled;

		// Token: 0x04001B93 RID: 7059
		protected ScrollbarStyle _scrollbarStyle = ScrollbarStyle.MakeDefault();

		// Token: 0x04001B94 RID: 7060
		private TexturePatch _scrollbarBackgroundPatch;

		// Token: 0x04001B95 RID: 7061
		private TexturePatch _scrollbarHandlePatch;

		// Token: 0x04001B96 RID: 7062
		private TexturePatch _scrollbarHoveredHandlePatch;

		// Token: 0x04001B97 RID: 7063
		private TexturePatch _scrollbarDraggedHandlePatch;

		// Token: 0x04001B98 RID: 7064
		private Point _scaledScrollArea;

		// Token: 0x04001B99 RID: 7065
		protected Point _scaledScrollOffset;

		// Token: 0x04001B9A RID: 7066
		private Point _scaledScrollSize;

		// Token: 0x04001B9B RID: 7067
		private Element.ScrollbarState _horizontalScrollbarState;

		// Token: 0x04001B9C RID: 7068
		private Element.ScrollbarState _verticalScrollbarState;

		// Token: 0x04001B9D RID: 7069
		[UIMarkupProperty]
		public Element.MouseWheelScrollBehaviourType MouseWheelScrollBehaviour = Element.MouseWheelScrollBehaviourType.Default;

		// Token: 0x04001B9E RID: 7070
		private int _horizontalScrollbarLength;

		// Token: 0x04001B9F RID: 7071
		private int _verticalScrollbarLength;

		// Token: 0x04001BA0 RID: 7072
		private Rectangle _horizontalScrollbarHandleRectangle;

		// Token: 0x04001BA1 RID: 7073
		private Rectangle _verticalScrollbarHandleRectangle;

		// Token: 0x04001BA2 RID: 7074
		private Point _scrollbarOffsets;

		// Token: 0x04001BA3 RID: 7075
		private int _scrollbarDragOffset;

		// Token: 0x04001BA4 RID: 7076
		protected Rectangle _viewRectangle;

		// Token: 0x04001BA5 RID: 7077
		protected Rectangle _contentRectangle;

		// Token: 0x04001BA6 RID: 7078
		[UIMarkupProperty]
		public PatchStyle Background;

		// Token: 0x04001BA7 RID: 7079
		protected TexturePatch _backgroundPatch;

		// Token: 0x04001BA8 RID: 7080
		[UIMarkupProperty]
		public UIPath MaskTexturePath;

		// Token: 0x04001BAA RID: 7082
		[UIMarkupProperty]
		public UInt32Color OutlineColor;

		// Token: 0x04001BAB RID: 7083
		[UIMarkupProperty]
		public float OutlineSize;

		// Token: 0x04001BAC RID: 7084
		protected bool _hasTooltipText;

		// Token: 0x04001BAD RID: 7085
		private TextTooltipLayer _tooltip;

		// Token: 0x04001BAE RID: 7086
		[UIMarkupProperty]
		public bool Overscroll;

		// Token: 0x04001BAF RID: 7087
		public string Name;

		// Token: 0x04001BB0 RID: 7088
		protected readonly List<Element> _children = new List<Element>();

		// Token: 0x02000D2C RID: 3372
		private enum ScrollbarState
		{
			// Token: 0x040040FE RID: 16638
			Default,
			// Token: 0x040040FF RID: 16639
			Hovered,
			// Token: 0x04004100 RID: 16640
			Dragged
		}

		// Token: 0x02000D2D RID: 3373
		public enum MouseWheelScrollBehaviourType
		{
			// Token: 0x04004102 RID: 16642
			Default,
			// Token: 0x04004103 RID: 16643
			VerticalOnly,
			// Token: 0x04004104 RID: 16644
			HorizontalOnly
		}
	}
}
