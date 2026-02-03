using System;
using System.Diagnostics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000872 RID: 2162
	[UIMarkupElement(AcceptsChildren = true)]
	public class ReorderableListGrip : Group
	{
		// Token: 0x06003CF3 RID: 15603 RVA: 0x0009A47A File Offset: 0x0009867A
		public ReorderableListGrip(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x0009A490 File Offset: 0x00098690
		protected override void OnUnmounted()
		{
			base.OnUnmounted();
			bool isDragging = this._isDragging;
			if (isDragging)
			{
				this._isDragging = false;
				this._listElement = null;
				this.Desktop.ClearMouseDrag();
			}
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x0009A4E3 File Offset: 0x000986E3
		protected override void OnMounted()
		{
			base.OnMounted();
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x0009A508 File Offset: 0x00098708
		private void Animate(float deltaTime)
		{
			bool flag = !this._isDragging;
			if (!flag)
			{
				this.UpdateTargetIndex();
				this._scrollDeltaTime += deltaTime;
				bool flag2 = this._scrollDeltaTime < 0.005f;
				if (!flag2)
				{
					int num = (int)(this._scrollDeltaTime / 0.005f);
					this._scrollDeltaTime %= 0.005f;
					float num2 = 20f * this.Desktop.Scale;
					Element element = this.Desktop.GetInteractiveLayer();
					while (element != null)
					{
						bool flag3 = (element.LayoutMode == LayoutMode.TopScrolling || element.LayoutMode == LayoutMode.BottomScrolling) && (float)element.RectangleAfterPadding.Height >= num2 * 3f;
						if (flag3)
						{
							bool flag4 = (float)element.RectangleAfterPadding.Top + this.Desktop.Scale * num2 >= (float)this.Desktop.MousePosition.Y;
							if (flag4)
							{
								bool flag5 = element.Scroll(0f, (float)num);
								if (flag5)
								{
									break;
								}
							}
							else
							{
								bool flag6 = (float)element.RectangleAfterPadding.Bottom - this.Desktop.Scale * num2 <= (float)this.Desktop.MousePosition.Y;
								if (flag6)
								{
									bool flag7 = element.Scroll(0f, (float)(-(float)num));
									if (flag7)
									{
										break;
									}
								}
							}
						}
						else
						{
							bool flag8 = (element.LayoutMode == LayoutMode.LeftScrolling || element.LayoutMode == LayoutMode.RightScrolling) && (float)element.RectangleAfterPadding.Width >= num2 * 3f;
							if (flag8)
							{
								bool flag9 = (float)element.RectangleAfterPadding.Left + this.Desktop.Scale * num2 >= (float)this.Desktop.MousePosition.X;
								if (flag9)
								{
									bool flag10 = element.Scroll((float)num, 0f);
									if (flag10)
									{
										break;
									}
								}
								else
								{
									bool flag11 = (float)element.RectangleAfterPadding.Right - this.Desktop.Scale * num2 <= (float)this.Desktop.MousePosition.X;
									if (flag11)
									{
										bool flag12 = element.Scroll((float)(-(float)num), 0f);
										if (flag12)
										{
											break;
										}
									}
								}
							}
						}
						bool flag13 = false;
						for (int i = element.Children.Count - 1; i >= 0; i--)
						{
							Element element2 = element.Children[i];
							bool flag14 = !element2.IsMounted;
							if (!flag14)
							{
								bool flag15 = element2.AnchoredRectangle.Contains(this.Desktop.MousePosition);
								if (flag15)
								{
									flag13 = true;
									element = element2;
									break;
								}
							}
						}
						bool flag16 = !flag13;
						if (flag16)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x0009A7F8 File Offset: 0x000989F8
		public override Element HitTest(Point position)
		{
			Debug.Assert(base.IsMounted);
			bool flag = !this._anchoredRectangle.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (base.HitTest(position) ?? this);
			}
			return result;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x0009A83C File Offset: 0x00098A3C
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = (long)evt.Button != 1L || !this.IsDragEnabled;
			if (!flag)
			{
				Element element;
				for (element = this; element != null; element = element.Parent)
				{
					bool flag2 = element.Parent is ReorderableList;
					if (flag2)
					{
						break;
					}
				}
				bool flag3 = element == null;
				if (!flag3)
				{
					this._mouseDownPosition = this.Desktop.MousePosition;
					this._wasDragging = false;
					this._listElement = element;
				}
			}
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x0009A8BC File Offset: 0x00098ABC
		protected override void OnMouseMove()
		{
			bool flag = this._isDragging || this._listElement == null || base.CapturedMouseButton == null || (long)base.CapturedMouseButton.Value != 1L;
			if (!flag)
			{
				float num = new Vector2((float)(this.Desktop.MousePosition.X - this._mouseDownPosition.X), (float)(this.Desktop.MousePosition.Y - this._mouseDownPosition.Y)).Length();
				bool flag2 = num < 3f;
				if (!flag2)
				{
					this._isDragging = true;
					this._wasDragging = true;
					this.Desktop.FocusElement(this, true);
					this.Desktop.StartMouseDrag(null, this, this._listElement);
					SDL.SDL_SetCursor(this.Desktop.Cursors.Move);
					this.OnMouseStartDrag();
				}
			}
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x0009A9B4 File Offset: 0x00098BB4
		protected internal override void Dismiss()
		{
			bool isDragging = this._isDragging;
			if (isDragging)
			{
				SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
				((ReorderableList)this._listElement.Parent).SetDropTargetIndex(-1);
				this._isDragging = false;
				this._listElement = null;
				this.Desktop.ClearMouseDrag();
			}
			else
			{
				base.Dismiss();
			}
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x0009AA1D File Offset: 0x00098C1D
		protected virtual void OnMouseStartDrag()
		{
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x0009AA20 File Offset: 0x00098C20
		protected internal override void OnMouseDragComplete(Element element, object data)
		{
			this.OnMouseDragCancel(data);
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x0009AA2C File Offset: 0x00098C2C
		protected internal override void OnMouseDragCancel(object data)
		{
			SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
			this.UpdateTargetIndex();
			Element listElement = this._listElement;
			this._isDragging = false;
			this._listElement = null;
			((ReorderableList)listElement.Parent).SetDropTargetIndex(-1);
			bool flag = !listElement.IsMounted;
			if (!flag)
			{
				ReorderableList reorderableList = (ReorderableList)listElement.Parent;
				int num = -1;
				for (int i = 0; i < reorderableList.Children.Count; i++)
				{
					bool flag2 = reorderableList.Children[i] != listElement;
					if (!flag2)
					{
						num = i;
						break;
					}
				}
				bool flag3 = this._targetIndex == num || this._targetIndex == num + 1;
				if (!flag3)
				{
					int num2 = (this._targetIndex > num) ? (this._targetIndex - 1) : this._targetIndex;
					reorderableList.Reorder(listElement, num2);
					reorderableList.Layout(null, true);
					Action<int, int> elementReordered = reorderableList.ElementReordered;
					if (elementReordered != null)
					{
						elementReordered(num, num2);
					}
				}
			}
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x0009AB4C File Offset: 0x00098D4C
		private void UpdateTargetIndex()
		{
			bool flag = !this._isDragging;
			if (!flag)
			{
				ReorderableList reorderableList = (ReorderableList)this._listElement.Parent;
				bool flag2 = reorderableList.LayoutMode == LayoutMode.Full;
				if (flag2)
				{
					this._targetIndex = -1;
					reorderableList.SetDropTargetIndex(-1);
				}
				else
				{
					bool flag3 = reorderableList.LayoutMode == LayoutMode.Top || reorderableList.LayoutMode == LayoutMode.Bottom || reorderableList.LayoutMode == LayoutMode.Middle || reorderableList.LayoutMode == LayoutMode.MiddleCenter || reorderableList.LayoutMode == LayoutMode.TopScrolling || reorderableList.LayoutMode == LayoutMode.BottomScrolling;
					this._targetIndex = -1;
					Element element = null;
					for (int i = 0; i < reorderableList.Children.Count; i++)
					{
						Element element2 = reorderableList.Children[i];
						bool flag4 = !element2.IsMounted;
						if (!flag4)
						{
							bool flag5 = flag3;
							if (flag5)
							{
								bool flag6 = element != null;
								if (flag6)
								{
									bool flag7 = element.AnchoredRectangle.Center.Y <= this.Desktop.MousePosition.Y && this.Desktop.MousePosition.Y < element.AnchoredRectangle.Center.Y + (element2.AnchoredRectangle.Center.Y - element.AnchoredRectangle.Center.Y);
									if (flag7)
									{
										this._targetIndex = i;
										break;
									}
								}
								else
								{
									bool flag8 = element2.AnchoredRectangle.Y <= this.Desktop.MousePosition.Y && this.Desktop.MousePosition.Y < element2.AnchoredRectangle.Y + element2.AnchoredRectangle.Height / 2;
									if (flag8)
									{
										this._targetIndex = i;
										break;
									}
								}
							}
							else
							{
								bool flag9 = element != null;
								if (flag9)
								{
									bool flag10 = element.AnchoredRectangle.Center.X <= this.Desktop.MousePosition.X && this.Desktop.MousePosition.X < element.AnchoredRectangle.Center.X + (element2.AnchoredRectangle.Center.X - element.AnchoredRectangle.Center.X);
									if (flag10)
									{
										this._targetIndex = i;
										break;
									}
								}
								else
								{
									bool flag11 = element2.AnchoredRectangle.X <= this.Desktop.MousePosition.X && this.Desktop.MousePosition.X < element2.AnchoredRectangle.X + element2.AnchoredRectangle.Width / 2;
									if (flag11)
									{
										this._targetIndex = i;
										break;
									}
								}
							}
							element = element2;
						}
					}
					bool flag12 = this._targetIndex == -1;
					if (flag12)
					{
						bool flag13 = flag3;
						if (flag13)
						{
							this._targetIndex = ((element != null && this.Desktop.MousePosition.Y > element.AnchoredRectangle.Center.Y) ? reorderableList.Children.Count : 0);
						}
						else
						{
							this._targetIndex = ((element != null && this.Desktop.MousePosition.X > element.AnchoredRectangle.Center.X) ? reorderableList.Children.Count : 0);
						}
					}
					reorderableList.SetDropTargetIndex(this._targetIndex);
				}
			}
		}

		// Token: 0x04001C5B RID: 7259
		private bool _isDragging;

		// Token: 0x04001C5C RID: 7260
		private Element _listElement;

		// Token: 0x04001C5D RID: 7261
		private int _targetIndex;

		// Token: 0x04001C5E RID: 7262
		private Point _mouseDownPosition;

		// Token: 0x04001C5F RID: 7263
		protected bool _wasDragging;

		// Token: 0x04001C60 RID: 7264
		private float _scrollDeltaTime;

		// Token: 0x04001C61 RID: 7265
		[UIMarkupProperty]
		public bool IsDragEnabled = true;
	}
}
