using System;
using HytaleClient.Data.Entities;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008BF RID: 2239
	internal class MovementIndicator : InterfaceComponent
	{
		// Token: 0x060040F7 RID: 16631 RVA: 0x000BD316 File Offset: 0x000BB516
		public MovementIndicator(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x000BD32C File Offset: 0x000BB52C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/MovementIndicator/MovementIndicator.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._icon = uifragment.Get<Group>("Icon");
			this._iconCrouch = new PatchStyle(document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "IconCrouch").Value);
			this._iconSlide = new PatchStyle(document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "IconSlide").Value);
			this._iconRoll = new PatchStyle(document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "IconRoll").Value);
			this._iconFly = new PatchStyle(document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "IconFly").Value);
			this._iconSprint = new PatchStyle(document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "IconSprint").Value);
			this._iconSprintSwim = new PatchStyle(document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "IconSprintSwim").Value);
			this._iconSwim = new PatchStyle(document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "IconSwim").Value);
			this._iconWalk = new PatchStyle(document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "IconWalk").Value);
			this._iconRun = new PatchStyle(document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "IconRun").Value);
			this.UpdateIcon(MovementIndicator.State.Running, false);
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x000BD4D4 File Offset: 0x000BB6D4
		public void Update(ClientMovementStates movementStates)
		{
			bool isFlying = movementStates.IsFlying;
			MovementIndicator.State state;
			if (isFlying)
			{
				state = MovementIndicator.State.Flying;
			}
			else
			{
				bool isSwimming = movementStates.IsSwimming;
				if (isSwimming)
				{
					state = (movementStates.IsSprinting ? MovementIndicator.State.SprintSwimming : MovementIndicator.State.Swimming);
				}
				else
				{
					bool isRolling = movementStates.IsRolling;
					if (isRolling)
					{
						state = MovementIndicator.State.Rolling;
					}
					else
					{
						bool isSliding = movementStates.IsSliding;
						if (isSliding)
						{
							state = MovementIndicator.State.Sliding;
						}
						else
						{
							bool isCrouching = movementStates.IsCrouching;
							if (isCrouching)
							{
								state = MovementIndicator.State.Crouching;
							}
							else
							{
								bool isSprinting = movementStates.IsSprinting;
								if (isSprinting)
								{
									state = MovementIndicator.State.Sprinting;
								}
								else
								{
									state = (movementStates.IsWalking ? MovementIndicator.State.Walking : MovementIndicator.State.Running);
								}
							}
						}
					}
				}
			}
			bool flag = state != this._previousState;
			if (flag)
			{
				this.UpdateIcon(state, true);
			}
			this._previousState = state;
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x000BD584 File Offset: 0x000BB784
		private void UpdateIcon(MovementIndicator.State state, bool doLayout = true)
		{
			switch (state)
			{
			case MovementIndicator.State.Sliding:
				this._icon.Background = this._iconSlide;
				break;
			case MovementIndicator.State.Crouching:
				this._icon.Background = this._iconCrouch;
				break;
			case MovementIndicator.State.Flying:
				this._icon.Background = this._iconFly;
				break;
			case MovementIndicator.State.Running:
				this._icon.Background = this._iconRun;
				break;
			case MovementIndicator.State.Sprinting:
				this._icon.Background = this._iconSprint;
				break;
			case MovementIndicator.State.SprintSwimming:
				this._icon.Background = this._iconSprintSwim;
				break;
			case MovementIndicator.State.Swimming:
				this._icon.Background = this._iconSwim;
				break;
			case MovementIndicator.State.Walking:
				this._icon.Background = this._iconWalk;
				break;
			case MovementIndicator.State.Rolling:
				this._icon.Background = this._iconRoll;
				break;
			}
			if (doLayout)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x04001F2D RID: 7981
		private MovementIndicator.State _previousState;

		// Token: 0x04001F2E RID: 7982
		private Group _icon;

		// Token: 0x04001F2F RID: 7983
		private PatchStyle _iconCrouch;

		// Token: 0x04001F30 RID: 7984
		private PatchStyle _iconSlide;

		// Token: 0x04001F31 RID: 7985
		private PatchStyle _iconRoll;

		// Token: 0x04001F32 RID: 7986
		private PatchStyle _iconFly;

		// Token: 0x04001F33 RID: 7987
		private PatchStyle _iconSprint;

		// Token: 0x04001F34 RID: 7988
		private PatchStyle _iconSprintSwim;

		// Token: 0x04001F35 RID: 7989
		private PatchStyle _iconSwim;

		// Token: 0x04001F36 RID: 7990
		private PatchStyle _iconWalk;

		// Token: 0x04001F37 RID: 7991
		private PatchStyle _iconRun;

		// Token: 0x02000D81 RID: 3457
		private enum State
		{
			// Token: 0x0400422D RID: 16941
			Sliding,
			// Token: 0x0400422E RID: 16942
			Crouching,
			// Token: 0x0400422F RID: 16943
			Flying,
			// Token: 0x04004230 RID: 16944
			Running,
			// Token: 0x04004231 RID: 16945
			Sprinting,
			// Token: 0x04004232 RID: 16946
			SprintSwimming,
			// Token: 0x04004233 RID: 16947
			Swimming,
			// Token: 0x04004234 RID: 16948
			Walking,
			// Token: 0x04004235 RID: 16949
			Rolling
		}
	}
}
