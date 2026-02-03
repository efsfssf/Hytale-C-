using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using SDL2;

namespace HytaleClient.Interface.InGame.Pages
{
	// Token: 0x0200088B RID: 2187
	internal class CustomPage : InterfaceComponent
	{
		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x06003E78 RID: 15992 RVA: 0x000A7F3F File Offset: 0x000A613F
		public bool HasPageDesktopFocusedElement
		{
			get
			{
				return this._pageDesktop.FocusedElement != null;
			}
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x000A7F50 File Offset: 0x000A6150
		public CustomPage(InGameView inGameView) : base(inGameView.Interface, null)
		{
			this._inGameView = inGameView;
			this._pageDesktop = new Desktop(this.Interface.InGameCustomUIProvider, this.Desktop.Graphics, this.Interface.Engine.Graphics.Batcher2D);
			this._pageLayer = new Element(this._pageDesktop, null);
			this._loadingOverlay = new Group(this.Desktop, null)
			{
				Background = new PatchStyle(0U),
				LayoutMode = LayoutMode.Center
			};
			this._loadingLabel = new Label(this.Desktop, this._loadingOverlay)
			{
				Anchor = new Anchor
				{
					Bottom = new int?(140),
					Height = new int?(70)
				},
				Background = new PatchStyle(0U),
				Padding = new Padding
				{
					Full = new int?(10)
				},
				Style = new LabelStyle
				{
					FontSize = 36f
				}
			};
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x000A8068 File Offset: 0x000A6268
		public void Build()
		{
			base.Clear();
			this._loadingLabel.Text = this.Interface.GetText("ui.general.loading", null, true);
			bool isLoading = this._isLoading;
			if (isLoading)
			{
				base.Add(this._loadingOverlay, -1);
			}
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x000A80B3 File Offset: 0x000A62B3
		public void ResetState()
		{
			this._pageLayer.Clear();
			base.Clear();
			this._isLoading = false;
			this._loadingTimer = 0f;
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x000A80DB File Offset: 0x000A62DB
		protected override void OnMounted()
		{
			this._pageDesktop.SetLayer(0, this._pageLayer);
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x000A8109 File Offset: 0x000A6309
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			this._pageDesktop.ClearAllLayers();
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x000A8130 File Offset: 0x000A6330
		private void Animate(float deltaTime)
		{
			this._pageDesktop.Update(deltaTime);
			bool isLoading = this._isLoading;
			if (isLoading)
			{
				this._loadingTimer += deltaTime;
			}
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x000A8164 File Offset: 0x000A6364
		public void Apply(CustomPage packet)
		{
			bool isLoading = this._isLoading;
			if (isLoading)
			{
				this._isLoading = false;
				base.Remove(this._loadingOverlay);
			}
			bool clear = packet.Clear;
			if (clear)
			{
				this._pageLayer.Clear();
			}
			try
			{
				this.Interface.InGameCustomUIProvider.ApplyCommands(packet.Commands, this._pageLayer);
			}
			catch (Exception ex)
			{
				this._pageLayer.Clear();
				this._inGameView.DisconnectWithError(ex.Message, ex);
				return;
			}
			try
			{
				CustomUIEventBinding[] eventBindings = packet.EventBindings;
				for (int i = 0; i < eventBindings.Length; i++)
				{
					CustomPage.<>c__DisplayClass15_0 CS$<>8__locals1 = new CustomPage.<>c__DisplayClass15_0();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.binding = eventBindings[i];
					Element element;
					List<string> list;
					CustomUIProvider.ResolveSelector(CS$<>8__locals1.binding.Selector, this._pageLayer, out element, out list);
					bool flag = element == null;
					if (flag)
					{
						throw new Exception("Target element in CustomUI event binding was not found. Selector: " + CS$<>8__locals1.binding.Selector);
					}
					bool flag2 = list != null;
					if (flag2)
					{
						throw new Exception("CustomUI event cannot be bound on a property. Selector: " + CS$<>8__locals1.binding.Selector);
					}
					JObject template = (CS$<>8__locals1.binding.Data != null) ? ((JObject)BsonHelper.FromBson(CS$<>8__locals1.binding.Data)) : new JObject();
					FieldInfo field = element.GetType().GetField(CS$<>8__locals1.binding.Type.ToString(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty);
					bool flag3 = CS$<>8__locals1.binding.Type == 6;
					if (flag3)
					{
						bool flag4 = element.GetType() != typeof(ItemGrid) || field == null || field.FieldType != typeof(Action<int, int>);
						if (flag4)
						{
							throw new Exception(string.Format("Target element in CustomUI event binding has no compatible {0} event. Selector: {1}", CS$<>8__locals1.binding.Type, CS$<>8__locals1.binding.Selector));
						}
						field.SetValue(element, new Action<int, int>(delegate(int index, int button)
						{
							JObject jobject = new JObject();
							jobject.Add("SlotIndex", index);
							jobject.Add("PressedMouseButton", button);
							base.<Apply>g__SendData|0(jobject);
						}));
					}
					else
					{
						bool flag5 = CS$<>8__locals1.binding.Type == 3;
						if (flag5)
						{
							bool flag6 = element.GetType() != typeof(ReorderableList) || field == null || field.FieldType != typeof(Action<int, int>);
							if (flag6)
							{
								throw new Exception(string.Format("Target element in CustomUI event binding has no compatible {0} event. Selector: {1}", CS$<>8__locals1.binding.Type, CS$<>8__locals1.binding.Selector));
							}
							field.SetValue(element, new Action<int, int>(delegate(int sourceIndex, int targetIndex)
							{
								JObject jobject = new JObject();
								jobject.Add("SourceIndex", sourceIndex);
								jobject.Add("TargetIndex", targetIndex);
								base.<Apply>g__SendData|0(jobject);
							}));
						}
						else
						{
							bool flag7 = field == null || field.FieldType != typeof(Action);
							if (flag7)
							{
								throw new Exception(string.Format("Target element in CustomUI event binding has no compatible {0} event. Selector: {1}", CS$<>8__locals1.binding.Type, CS$<>8__locals1.binding.Selector));
							}
							field.SetValue(element, new Action(delegate()
							{
								base.<Apply>g__SendData|0(null);
							}));
						}
					}
				}
			}
			catch (Exception exception)
			{
				this._inGameView.DisconnectWithError("Failed to apply CustomUI event bindings", exception);
				return;
			}
			this.Desktop.RefreshHover();
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x000A855C File Offset: 0x000A675C
		private JObject GatherDataFromTemplate(JObject template)
		{
			JObject jobject = new JObject();
			this.<GatherDataFromTemplate>g__Recurse|16_0(template, jobject);
			return jobject;
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x000A8580 File Offset: 0x000A6780
		public void StartLoading()
		{
			bool isLoading = this._isLoading;
			if (!isLoading)
			{
				this._isLoading = true;
				this._loadingTimer = 0f;
				base.Add(this._loadingOverlay, -1);
				this._loadingOverlay.Layout(new Rectangle?(this._pageDesktop.RootLayoutRectangle), true);
				this._pageDesktop.ClearInput(false);
				this._pageDesktop.RefreshHover();
			}
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x000A85F0 File Offset: 0x000A67F0
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x000A8604 File Offset: 0x000A6804
		protected override void OnMouseMove()
		{
			this._pageDesktop.OnMouseMove(this.Desktop.MousePosition);
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x000A8620 File Offset: 0x000A6820
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = !this._isLoading;
			if (flag)
			{
				this._pageDesktop.OnMouseDown(evt.Button, evt.Clicks);
			}
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x000A8654 File Offset: 0x000A6854
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = !this._isLoading;
			if (flag)
			{
				this._pageDesktop.OnMouseUp(evt.Button, evt.Clicks);
			}
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x000A8688 File Offset: 0x000A6888
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			bool flag = keycode == SDL.SDL_Keycode.SDLK_ESCAPE;
			if (flag)
			{
				this.Dismiss();
			}
			else
			{
				bool flag2 = !this._isLoading;
				if (flag2)
				{
					this._pageDesktop.OnKeyDown(keycode, repeat);
				}
			}
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x000A86C4 File Offset: 0x000A68C4
		protected internal override void OnKeyUp(SDL.SDL_Keycode keycode)
		{
			bool flag = !this._isLoading;
			if (flag)
			{
				this._pageDesktop.OnKeyUp(keycode);
			}
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x000A86EC File Offset: 0x000A68EC
		protected internal override void OnTextInput(string text)
		{
			bool flag = !this._isLoading;
			if (flag)
			{
				this._pageDesktop.OnTextInput(text);
			}
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x000A8714 File Offset: 0x000A6914
		protected internal override bool OnMouseWheel(Point offset)
		{
			bool flag = !this._isLoading;
			if (flag)
			{
				this._pageDesktop.OnMouseWheel(offset);
			}
			return true;
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x000A8741 File Offset: 0x000A6941
		protected override void LayoutSelf()
		{
			this._pageDesktop.SetViewport(this.Desktop.ViewportRectangle, this.Desktop.Scale);
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x000A8768 File Offset: 0x000A6968
		protected override void PrepareForDrawSelf()
		{
			bool isLoading = this._isLoading;
			if (isLoading)
			{
				float num = (this._loadingTimer > 0.2f) ? MathHelper.Min(1f, (this._loadingTimer - 0.2f) / 0.5f) : 0f;
				double num2 = Math.Pow((double)num, 3.0);
				this._loadingOverlay.Background.Color = UInt32Color.FromRGBA(0, 0, 0, (byte)(127.0 * num2));
				this._loadingLabel.Background.Color = UInt32Color.FromRGBA(0, 0, 0, (byte)(255.0 * num2));
				this._loadingLabel.Style.TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255.0 * num2));
				this._loadingOverlay.Layout(null, true);
			}
			this._pageDesktop.PrepareForDraw();
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x000A8861 File Offset: 0x000A6A61
		public void OnChangeDrawOutlines()
		{
			this._pageDesktop.DrawOutlines = this.Desktop.DrawOutlines;
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x000A887C File Offset: 0x000A6A7C
		[CompilerGenerated]
		private void <GatherDataFromTemplate>g__Recurse|16_0(JObject templacePiece, JObject resultPiece)
		{
			foreach (KeyValuePair<string, JToken> keyValuePair in templacePiece)
			{
				bool flag = keyValuePair.Key.StartsWith("@");
				if (flag)
				{
					JToken jtoken;
					bool flag2 = !CustomUIProvider.TryGetPropertyValueAsJsonFromSelector((string)keyValuePair.Value, this._pageLayer, out jtoken);
					if (flag2)
					{
						throw new Exception("Could not gather property value for CustomUI event binding. Key: " + keyValuePair.Key);
					}
					resultPiece[keyValuePair.Key] = jtoken;
				}
				else
				{
					bool flag3 = keyValuePair.Value.Type == 1;
					if (flag3)
					{
						JObject jobject = new JObject();
						resultPiece[keyValuePair.Key] = jobject;
						this.<GatherDataFromTemplate>g__Recurse|16_0((JObject)keyValuePair.Value, jobject);
					}
					else
					{
						resultPiece[keyValuePair.Key] = keyValuePair.Value;
					}
				}
			}
		}

		// Token: 0x04001D69 RID: 7529
		private readonly InGameView _inGameView;

		// Token: 0x04001D6A RID: 7530
		private readonly Desktop _pageDesktop;

		// Token: 0x04001D6B RID: 7531
		private readonly Element _pageLayer;

		// Token: 0x04001D6C RID: 7532
		private readonly Group _loadingOverlay;

		// Token: 0x04001D6D RID: 7533
		private readonly Label _loadingLabel;

		// Token: 0x04001D6E RID: 7534
		private bool _isLoading;

		// Token: 0x04001D6F RID: 7535
		private float _loadingTimer;
	}
}
