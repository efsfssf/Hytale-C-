using System;
using System.Collections.Generic;
using System.Globalization;
using HytaleClient.Core;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame.Commands;
using HytaleClient.InGame.Modules.BuilderTools.Tools;
using HytaleClient.InGame.Modules.BuilderTools.Tools.Brush;
using HytaleClient.InGame.Modules.BuilderTools.Tools.Client;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.InGame.Modules.Shortcuts;
using HytaleClient.Interface;
using HytaleClient.Interface.InGame;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.InGame.Modules.BuilderTools
{
	// Token: 0x0200097D RID: 2429
	internal class BuilderToolsModule : Module
	{
		// Token: 0x06004CDD RID: 19677 RVA: 0x00147CEC File Offset: 0x00145EEC
		[Usage("tool", new string[]
		{
			"arg [id] [value]",
			"brush [id] [value]",
			"args",
			"reach [distance] [lock]",
			"delay [msTime]",
			"offset [value]",
			"axislock [x] [y] [z]",
			"color [red] [green] [blue]",
			"listblocks",
			"list",
			"macros"
		})]
		[Description("Builder Tools command")]
		public void ToolsCommand(string[] args)
		{
			BuilderToolsModule.<>c__DisplayClass0_0 CS$<>8__locals1 = new BuilderToolsModule.<>c__DisplayClass0_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.args = args;
			bool flag = CS$<>8__locals1.args.Length == 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = CS$<>8__locals1.args[0];
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 1322381784U)
			{
				if (num <= 1031692888U)
				{
					if (num != 217798785U)
					{
						if (num != 348705738U)
						{
							if (num != 1031692888U)
							{
								goto IL_9E8;
							}
							if (!(text2 == "color"))
							{
								goto IL_9E8;
							}
							bool flag2 = CS$<>8__locals1.args.Length != 4;
							if (flag2)
							{
								throw new InvalidCommandUsage();
							}
							int num2 = int.Parse(CS$<>8__locals1.args[1], CultureInfo.InvariantCulture);
							int num3 = int.Parse(CS$<>8__locals1.args[2], CultureInfo.InvariantCulture);
							int num4 = int.Parse(CS$<>8__locals1.args[3], CultureInfo.InvariantCulture);
							this.SelectionTool.Color = new Vector3((float)num2, (float)num3, (float)num4);
							this._gameInstance.Chat.Log(string.Format("Selection tool display color set to {0}.", this.SelectionTool.Color));
							return;
						}
						else
						{
							if (!(text2 == "offset"))
							{
								goto IL_9E8;
							}
							bool flag3 = CS$<>8__locals1.args.Length == 4;
							if (flag3)
							{
								this.ToolVectorOffset = new IntVector3(int.Parse(CS$<>8__locals1.args[1], CultureInfo.InvariantCulture), int.Parse(CS$<>8__locals1.args[2], CultureInfo.InvariantCulture), int.Parse(CS$<>8__locals1.args[3], CultureInfo.InvariantCulture));
								this._gameInstance.Chat.Log(string.Format("Offset set to {0}.", this.ToolVectorOffset));
							}
							else
							{
								bool flag4 = CS$<>8__locals1.args.Length == 2;
								if (!flag4)
								{
									throw new InvalidCommandUsage();
								}
								this.ToolVectorOffset = new IntVector3(0, int.Parse(CS$<>8__locals1.args[1], CultureInfo.InvariantCulture), 0);
								this._gameInstance.Chat.Log(string.Format("Offset set to {0}.", this.ToolVectorOffset));
							}
							return;
						}
					}
					else
					{
						if (!(text2 == "list"))
						{
							goto IL_9E8;
						}
						ClientItemBase[] builderToolItems = BuilderTool.GetBuilderToolItems(this._gameInstance);
						List<string> list = new List<string>();
						for (int i = 0; i < builderToolItems.Length; i++)
						{
							list.Add(builderToolItems[i].BuilderTool.ToString());
						}
						list.Sort();
						string str = string.Join<string>(", ", list);
						this._gameInstance.Chat.Log("Loaded builder tools: [" + str + "]");
						return;
					}
				}
				else if (num != 1047474471U)
				{
					if (num != 1291669624U)
					{
						if (num != 1322381784U)
						{
							goto IL_9E8;
						}
						if (!(text2 == "delay"))
						{
							goto IL_9E8;
						}
						bool flag5 = CS$<>8__locals1.args.Length != 2;
						if (flag5)
						{
							throw new InvalidCommandUsage();
						}
						this.builderToolsSettings.ToolDelayMin = int.Parse(CS$<>8__locals1.args[1], CultureInfo.InvariantCulture);
						this._gameInstance.Chat.Log(string.Format("Tool delay set to {0}.", this.builderToolsSettings.ToolDelayMin));
						this._gameInstance.App.Settings.Save();
						return;
					}
					else
					{
						if (!(text2 == "reach"))
						{
							goto IL_9E8;
						}
						bool flag6 = CS$<>8__locals1.args.Length == 3;
						if (flag6)
						{
							bool flag7 = CS$<>8__locals1.args[2].ToLower() != "lock";
							if (flag7)
							{
								throw new InvalidCommandUsage();
							}
							this.builderToolsSettings.ToolReachLock = true;
						}
						else
						{
							bool flag8 = CS$<>8__locals1.args.Length == 2;
							if (!flag8)
							{
								throw new InvalidCommandUsage();
							}
							this.builderToolsSettings.ToolReachLock = false;
						}
						this.builderToolsSettings.ToolReachDistance = int.Parse(CS$<>8__locals1.args[1], CultureInfo.InvariantCulture);
						bool toolReachLock = this.builderToolsSettings.ToolReachLock;
						if (toolReachLock)
						{
							this._gameInstance.Chat.Log(string.Format("Tool reach distance locked to {0}.", this.builderToolsSettings.ToolReachDistance));
						}
						else
						{
							this._gameInstance.Chat.Log(string.Format("Tool reach distance set to {0}.", this.builderToolsSettings.ToolReachDistance));
						}
						return;
					}
				}
				else if (!(text2 == "arg"))
				{
					goto IL_9E8;
				}
			}
			else if (num <= 2748840416U)
			{
				if (num != 2146971591U)
				{
					if (num != 2634721084U)
					{
						if (num != 2748840416U)
						{
							goto IL_9E8;
						}
						if (!(text2 == "macros"))
						{
							goto IL_9E8;
						}
						ShortcutsModule shortcutsModule = this._gameInstance.ShortcutsModule;
						shortcutsModule.AddMacro("w", ".tool brush width %1");
						shortcutsModule.AddMacro("h", ".tool brush height %1");
						shortcutsModule.AddMacro("s", ".tool brush shape %1");
						shortcutsModule.AddMacro("o", ".tool brush origin %1");
						shortcutsModule.AddMacro("b", ".tool brush material %1");
						shortcutsModule.AddMacro("m", ".tool brush mask %1");
						shortcutsModule.AddMacro("a", ".tool arg %1 %2");
						shortcutsModule.AddMacro("aa", ".tool args");
						this._gameInstance.Chat.Log("Tool shortcut macros added!");
						return;
					}
					else
					{
						if (!(text2 == "args"))
						{
							goto IL_9E8;
						}
						bool flag9 = !this.HasActiveTool;
						if (flag9)
						{
							this._gameInstance.Chat.Log("You need an active builder tool in hand to use this.");
							return;
						}
						string toolArgsLogText = this.ActiveTool.BuilderTool.GetToolArgsLogText(this.ActiveTool);
						this._gameInstance.Chat.Log(toolArgsLogText);
						return;
					}
				}
				else
				{
					if (!(text2 == "surfaceoffset"))
					{
						goto IL_9E8;
					}
					bool flag10 = CS$<>8__locals1.args.Length != 2;
					if (flag10)
					{
						throw new InvalidCommandUsage();
					}
					this._toolSurfaceOffset = int.Parse(CS$<>8__locals1.args[1], CultureInfo.InvariantCulture);
					this._gameInstance.Chat.Log(string.Format("Surface offset set to {0}.", this._toolSurfaceOffset));
					return;
				}
			}
			else if (num != 3323320667U)
			{
				if (num != 4207177203U)
				{
					if (num != 4290564511U)
					{
						goto IL_9E8;
					}
					if (!(text2 == "listblocks"))
					{
						goto IL_9E8;
					}
					bool flag11 = CS$<>8__locals1.args.Length >= 2;
					if (flag11)
					{
						bool flag12 = CS$<>8__locals1.args[1] == "-c";
						if (flag12)
						{
							this.SelectionArea.ListBlocks(true, null);
						}
						else
						{
							this.SelectionArea.ListBlocks(false, CS$<>8__locals1.args[1]);
						}
					}
					else
					{
						this.SelectionArea.ListBlocks(false, null);
					}
					return;
				}
				else if (!(text2 == "brush"))
				{
					goto IL_9E8;
				}
			}
			else
			{
				if (!(text2 == "axislock"))
				{
					goto IL_9E8;
				}
				bool flag13 = CS$<>8__locals1.args.Length == 4;
				if (flag13)
				{
					float x;
					bool flag14 = !float.TryParse(CS$<>8__locals1.args[1], out x);
					if (flag14)
					{
						throw new InvalidCommandUsage();
					}
					float y;
					bool flag15 = !float.TryParse(CS$<>8__locals1.args[2], out y);
					if (flag15)
					{
						throw new InvalidCommandUsage();
					}
					float z;
					bool flag16 = !float.TryParse(CS$<>8__locals1.args[3], out z);
					if (flag16)
					{
						throw new InvalidCommandUsage();
					}
					this.Brush.initialBlockPosition = new Vector3(x, y, z);
				}
				else
				{
					bool flag17 = CS$<>8__locals1.args.Length == 2 && this._gameInstance.InteractionModule.HasFoundTargetBlock;
					if (flag17)
					{
						Vector3 blockPosition = this._gameInstance.InteractionModule.TargetBlockHit.BlockPosition;
						float num5;
						bool flag18 = !float.TryParse(CS$<>8__locals1.args[1], out num5);
						if (flag18)
						{
							throw new InvalidCommandUsage();
						}
						blockPosition.Y += num5;
						this.Brush.initialBlockPosition = blockPosition;
					}
					else
					{
						bool flag19 = CS$<>8__locals1.args.Length == 1 && this._gameInstance.InteractionModule.HasFoundTargetBlock;
						if (!flag19)
						{
							throw new InvalidCommandUsage();
						}
						this.Brush.initialBlockPosition = this._gameInstance.InteractionModule.TargetBlockHit.BlockPosition;
					}
				}
				this.Brush.lockModeActive = true;
				this.Brush.lockMode = BrushTool.LockMode.Always;
				this.Brush.unlockedAxis = BrushTool.AxisAndPlanes.XZ;
				this._gameInstance.Chat.Log(string.Format("Brush axis lock set to mode {0}, unlocked axis {1} and initial position {2}.", this.Brush.lockMode, this.Brush.unlockedAxis, this.Brush.initialBlockPosition));
				return;
			}
			bool flag20 = CS$<>8__locals1.args.Length != 2 && CS$<>8__locals1.args.Length != 3;
			if (flag20)
			{
				throw new InvalidCommandUsage();
			}
			BuilderToolArgGroup argGroup = (CS$<>8__locals1.args[0] == "arg") ? 0 : 1;
			string argValue = (CS$<>8__locals1.args.Length == 3) ? CS$<>8__locals1.args[2] : "";
			this.SendConfiguringToolArgUpdate(argGroup, CS$<>8__locals1.args[1], argValue, delegate(FailureReply err, SuccessReply reply)
			{
				bool flag21 = reply != null;
				if (flag21)
				{
					CS$<>8__locals1.<>4__this._gameInstance.Chat.Log(string.Concat(new string[]
					{
						(argGroup == 1) ? "Brush" : "Tool",
						" ",
						CS$<>8__locals1.args[1],
						" changed to ",
						argValue
					}));
				}
				else
				{
					CS$<>8__locals1.<>4__this._gameInstance.Chat.AddBsonMessage(err.Message);
				}
			});
			return;
			IL_9E8:
			throw new InvalidCommandUsage();
		}

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06004CDE RID: 19678 RVA: 0x001486E7 File Offset: 0x001468E7
		public BuilderToolsSettings builderToolsSettings
		{
			get
			{
				return this._gameInstance.App.Settings.BuilderToolsSettings;
			}
		}

		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06004CDF RID: 19679 RVA: 0x001486FE File Offset: 0x001468FE
		// (set) Token: 0x06004CE0 RID: 19680 RVA: 0x00148706 File Offset: 0x00146906
		public SelectionTool SelectionTool { get; private set; }

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06004CE1 RID: 19681 RVA: 0x0014870F File Offset: 0x0014690F
		// (set) Token: 0x06004CE2 RID: 19682 RVA: 0x00148717 File Offset: 0x00146917
		public PlaySelectionTool PlaySelection { get; private set; }

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06004CE3 RID: 19683 RVA: 0x00148720 File Offset: 0x00146920
		// (set) Token: 0x06004CE4 RID: 19684 RVA: 0x00148728 File Offset: 0x00146928
		public BrushTool Brush { get; private set; }

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06004CE5 RID: 19685 RVA: 0x00148731 File Offset: 0x00146931
		// (set) Token: 0x06004CE6 RID: 19686 RVA: 0x00148739 File Offset: 0x00146939
		public PasteTool Paste { get; private set; }

		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06004CE7 RID: 19687 RVA: 0x00148742 File Offset: 0x00146942
		// (set) Token: 0x06004CE8 RID: 19688 RVA: 0x0014874A File Offset: 0x0014694A
		public AnchorTool Anchor { get; private set; }

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06004CE9 RID: 19689 RVA: 0x00148753 File Offset: 0x00146953
		// (set) Token: 0x06004CEA RID: 19690 RVA: 0x0014875C File Offset: 0x0014695C
		public ToolInstance ActiveTool
		{
			get
			{
				return this._activeTool;
			}
			private set
			{
				bool flag;
				if (((value != null) ? value.ClientTool : null) != null)
				{
					ToolInstance activeTool = this._activeTool;
					flag = (((activeTool != null) ? activeTool.ClientTool : null) == value.ClientTool);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				if (flag2)
				{
					value.ClientTool.OnToolItemChange(value.ItemStack);
				}
				else
				{
					ToolInstance activeTool2 = this._activeTool;
					if (activeTool2 != null)
					{
						ClientTool clientTool = activeTool2.ClientTool;
						if (clientTool != null)
						{
							clientTool.SetInactive();
						}
					}
					this._activeTool = value;
					ToolInstance activeTool3 = this._activeTool;
					if (activeTool3 != null)
					{
						ClientTool clientTool2 = activeTool3.ClientTool;
						if (clientTool2 != null)
						{
							ToolInstance activeTool4 = this._activeTool;
							clientTool2.SetActive((activeTool4 != null) ? activeTool4.ItemStack : null);
						}
					}
				}
				this.UpdateUIForActiveTool();
			}
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x0014880C File Offset: 0x00146A0C
		public bool HasConfigurationToolBrushDataOrArguments()
		{
			ToolInstance configuringTool = this.ConfiguringTool;
			int num = (configuringTool != null) ? configuringTool.BuilderTool.GetItemToolArgs(this.ConfiguringTool.ItemStack).Count : 0;
			return (this.ConfiguringTool != null && this.ConfiguringTool.BrushData != null) || num != 0;
		}

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06004CEC RID: 19692 RVA: 0x00148862 File Offset: 0x00146A62
		// (set) Token: 0x06004CED RID: 19693 RVA: 0x0014886A File Offset: 0x00146A6A
		public ToolInstance ConfiguringTool
		{
			get
			{
				return this._configuringTool;
			}
			set
			{
				this._configuringTool = value;
				this.UpdateUIForConfiguringTool();
			}
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06004CEE RID: 19694 RVA: 0x0014887B File Offset: 0x00146A7B
		public bool HasActiveTool
		{
			get
			{
				return this.ActiveTool != null;
			}
		}

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06004CEF RID: 19695 RVA: 0x00148886 File Offset: 0x00146A86
		public bool HasActiveBrush
		{
			get
			{
				return this.HasActiveTool && this.ActiveTool.BuilderTool.IsBrushTool;
			}
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06004CF0 RID: 19696 RVA: 0x001488A3 File Offset: 0x00146AA3
		// (set) Token: 0x06004CF1 RID: 19697 RVA: 0x001488AB File Offset: 0x00146AAB
		public Vector3 BrushTargetPosition { get; private set; } = Vector3.NaN;

		// Token: 0x06004CF2 RID: 19698 RVA: 0x001488B4 File Offset: 0x00146AB4
		public BuilderToolsModule(GameInstance gameInstance) : base(gameInstance)
		{
			this.ClientTools = new Dictionary<string, ClientTool>();
			this.UpdateUIToolSettings();
			this.RegisterEvents();
			this._gameInstance.RegisterCommand("tool", new GameInstance.Command(this.ToolsCommand));
			this.SelectionArea = new SelectionArea(gameInstance);
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x00148938 File Offset: 0x00146B38
		public override void Initialize()
		{
			this.Brush = new BrushTool(this._gameInstance);
			Action<ClientTool> action = delegate(ClientTool tool)
			{
				this.ClientTools.Add(tool.ToolId, tool);
			};
			action(this.SelectionTool = new SelectionTool(this._gameInstance));
			action(this.PlaySelection = new PlaySelectionTool(this._gameInstance));
			action(this.Paste = new PasteTool(this._gameInstance));
			action(new EntityTool(this._gameInstance));
			action(new ExtrudeTool(this._gameInstance));
			action(new LineTool(this._gameInstance));
			action(new HitboxTool(this._gameInstance));
			action(new MachinimaTool(this._gameInstance));
			action(this.Anchor = new AnchorTool(this._gameInstance));
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x00148A34 File Offset: 0x00146C34
		protected override void DoDispose()
		{
			BrushTool brush = this.Brush;
			if (brush != null)
			{
				brush.Dispose();
			}
			this.UnregisterEvents();
			foreach (ClientTool clientTool in this.ClientTools.Values)
			{
				clientTool.Dispose();
			}
			this.SelectionArea.DoDispose();
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x00148AB8 File Offset: 0x00146CB8
		public bool ShouldSendMouseWheelEventToPlaySelectionTool()
		{
			if (this.HasActiveTool)
			{
				ToolInstance activeTool = this.ActiveTool;
				if (((activeTool != null) ? activeTool.ClientTool : null) != null && typeof(PlaySelectionTool).IsInstanceOfType(this.ActiveTool.ClientTool))
				{
					return ((PlaySelectionTool)this.ActiveTool.ClientTool).IsInTransformationMode();
				}
			}
			return false;
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x00148B1C File Offset: 0x00146D1C
		public void SendMouseWheelEventToPlaySelectionTool(int directionOfScroll)
		{
			bool flag = !this.ShouldSendMouseWheelEventToPlaySelectionTool();
			if (!flag)
			{
				((PlaySelectionTool)this.ActiveTool.ClientTool).OnScrollWheelEvent(directionOfScroll);
			}
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x00148B50 File Offset: 0x00146D50
		public void Update(float deltaTime)
		{
			bool flag = this._gameInstance.GameMode != 1;
			if (!flag)
			{
				bool flag2 = !this.HasActiveTool;
				if (!flag2)
				{
					BrushTool brush = this.Brush;
					if (brush != null)
					{
						brush.Update(deltaTime);
					}
					bool toolReachLock = this.builderToolsSettings.ToolReachLock;
					if (toolReachLock)
					{
						Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
						Vector3 vector = lookRay.Position + lookRay.Direction * (float)this.ToolsInteractionDistance;
						vector.X = (float)((int)Math.Floor((double)vector.X));
						vector.Y = (float)((int)Math.Floor((double)vector.Y));
						vector.Z = (float)((int)Math.Floor((double)vector.Z));
						this.BrushTargetPosition = vector;
					}
					else
					{
						bool flag3 = this.Brush != null && ((this.Brush.lockMode != BrushTool.LockMode.None && this.Brush.lockModeActive) || this.Brush._brushAxisLockPlane.IsEnabled());
						if (flag3)
						{
							int num;
							Vector3 lockedBrushPosition = this.Brush.GetLockedBrushPosition(out num);
							bool flag4 = num <= this.ToolsInteractionDistance;
							if (flag4)
							{
								this.BrushTargetPosition = lockedBrushPosition;
								bool flag5 = this.ToolVectorOffset != IntVector3.Zero;
								if (flag5)
								{
									this.BrushTargetPosition += this.ToolVectorOffset;
								}
								bool flag6 = this._toolSurfaceOffset != 0;
								if (flag6)
								{
									Vector3 value = this._gameInstance.InteractionModule.TargetBlockHit.Normal * (float)this._toolSurfaceOffset;
									this.BrushTargetPosition += value;
								}
							}
							else
							{
								this.BrushTargetPosition = Vector3.NaN;
							}
						}
						else
						{
							bool flag7 = this._gameInstance.InteractionModule.HasFoundTargetBlock && (this._gameInstance.InteractionModule.PlacingAtRange || this._gameInstance.InteractionModule.TargetBlockHit.Distance <= (float)this.ToolsInteractionDistance);
							if (flag7)
							{
								this.BrushTargetPosition = this._gameInstance.InteractionModule.TargetBlockHit.BlockPosition;
								bool flag8 = this.ToolVectorOffset != IntVector3.Zero;
								if (flag8)
								{
									this.BrushTargetPosition += this.ToolVectorOffset;
								}
								bool flag9 = this._toolSurfaceOffset != 0;
								if (flag9)
								{
									Vector3 value2 = this._gameInstance.InteractionModule.TargetBlockHit.Normal * (float)this._toolSurfaceOffset;
									this.BrushTargetPosition += value2;
								}
							}
							else
							{
								this.BrushTargetPosition = Vector3.NaN;
							}
						}
					}
					Input input = this._gameInstance.Input;
					bool flag10 = this.ActiveTool.ClientTool != null;
					if (flag10)
					{
						this.ActiveTool.ClientTool.Update(deltaTime);
					}
					bool flag11 = this.ActiveTool.BuilderTool.IsBrushTool && input.IsAnyKeyHeld(false);
					if (flag11)
					{
						this.Brush.OnKeyDown();
					}
					bool flag12 = input.IsShiftHeld();
					if (flag12)
					{
						InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
						bool flag13 = input.ConsumeBinding(inputBindings.UndoItemAction, false);
						if (flag13)
						{
							this.OnUndo();
						}
						else
						{
							bool flag14 = input.ConsumeBinding(inputBindings.RedoItemAction, false);
							if (flag14)
							{
								this.OnRedo();
							}
						}
					}
				}
			}
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x00148F04 File Offset: 0x00147104
		public void Draw(ref Matrix viewProjectionMatrix)
		{
			bool flag = !this.NeedsDrawing();
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with NeedsDrawing() first before calling this.");
			}
			bool flag2 = this.SelectionArea.NeedsDrawing();
			if (flag2)
			{
				bool flag3 = this.SelectionArea.RenderMode == SelectionArea.SelectionRenderMode.LegacySelection;
				if (flag3)
				{
					this.SelectionTool.Draw(ref viewProjectionMatrix);
				}
				else
				{
					this.PlaySelection.Draw(ref viewProjectionMatrix);
				}
			}
			bool flag4 = this.Anchor.NeedsDrawing();
			if (flag4)
			{
				this.Anchor.Draw(ref viewProjectionMatrix);
			}
			bool flag5 = this.ToolNeedsDrawing();
			if (flag5)
			{
				ToolInstance activeTool = this.ActiveTool;
				bool flag6 = ((activeTool != null) ? activeTool.ClientTool : null) != null && this.ActiveTool.ClientTool.NeedsDrawing();
				if (flag6)
				{
					bool flag7 = typeof(SelectionTool).IsInstanceOfType(this.ActiveTool.ClientTool) || typeof(PlaySelectionTool).IsInstanceOfType(this.ActiveTool.ClientTool);
					if (!flag7)
					{
						this.ActiveTool.ClientTool.Draw(ref viewProjectionMatrix);
					}
				}
				else
				{
					bool isBrushTool = this.ActiveTool.BuilderTool.IsBrushTool;
					if (isBrushTool)
					{
						this.Brush._brushAxisLockPlane.Draw(ref viewProjectionMatrix);
						bool flag8 = this.Brush._brushAxisLockPlane.GetMode() != BrushAxisLockPlane.EditMode.None && this.Brush._brushAxisLockPlane.IsEnabled();
						bool flag9 = !this.BrushTargetPosition.IsNaN() && !flag8;
						if (flag9)
						{
							this.Brush.Draw(ref viewProjectionMatrix, this.BrushTargetPosition - this._gameInstance.SceneRenderer.Data.CameraPosition, (float)this.builderToolsSettings.BrushOpacity * 0.01f);
							this.Brush.Draw(ref viewProjectionMatrix, this.BrushTargetPosition - this._gameInstance.SceneRenderer.Data.CameraPosition, (float)this.builderToolsSettings.BrushOpacity * 0.01f);
						}
					}
				}
			}
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x00149118 File Offset: 0x00147318
		public void DrawText(ref Matrix viewProjectionMatrix)
		{
			bool flag = !this.NeedsTextDrawing();
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with TextNeedsDrawing() first before calling this.");
			}
			bool flag2 = this.ActiveTool != null && this.ActiveTool.ClientTool != null && this.ActiveTool.ClientTool.NeedsTextDrawing();
			if (flag2)
			{
				this.ActiveTool.ClientTool.DrawText(ref viewProjectionMatrix);
			}
			bool flag3 = this.SelectionTool.NeedsDrawing() && this.SelectionArea.IsSelectionDefined();
			if (flag3)
			{
				this.SelectionTool.DrawText(ref viewProjectionMatrix);
			}
			bool flag4 = this.PlaySelection.NeedsDrawing() && this.SelectionArea.IsSelectionDefined();
			if (flag4)
			{
				this.PlaySelection.DrawText(ref viewProjectionMatrix);
			}
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x001491D4 File Offset: 0x001473D4
		public bool NeedsDrawing()
		{
			return this._gameInstance.GameMode == 1 && (this.ToolNeedsDrawing() || this.SelectionTool.NeedsDrawing() || this.PlaySelection.NeedsDrawing() || this.Anchor.NeedsDrawing());
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x00149228 File Offset: 0x00147428
		public bool NeedsTextDrawing()
		{
			bool result;
			if (this._gameInstance.GameMode == 1)
			{
				if (!this.PlaySelection.NeedsDrawing() && !this.SelectionTool.NeedsDrawing())
				{
					ToolInstance activeTool = this.ActiveTool;
					result = (((activeTool != null) ? activeTool.ClientTool : null) != null && this.ActiveTool.ClientTool.NeedsTextDrawing());
				}
				else
				{
					result = true;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x00149290 File Offset: 0x00147490
		private bool ToolNeedsDrawing()
		{
			bool flag = !this.HasActiveTool;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.ActiveTool.ClientTool != null && this.ActiveTool.ClientTool.NeedsDrawing();
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool isBrushTool = this.ActiveTool.BuilderTool.IsBrushTool;
					if (isBrushTool)
					{
						bool toolReachLock = this.builderToolsSettings.ToolReachLock;
						if (toolReachLock)
						{
							return true;
						}
						bool flag3 = this.Brush._brushAxisLockPlane.IsEnabled();
						if (flag3)
						{
							return true;
						}
						bool lockModeActive = this.Brush.lockModeActive;
						if (lockModeActive)
						{
							return true;
						}
						InteractionModule interactionModule = this._gameInstance.InteractionModule;
						bool flag4 = interactionModule.HasFoundTargetBlock && interactionModule.TargetBlockHit.Distance <= (float)this.ToolsInteractionDistance;
						if (flag4)
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x00149380 File Offset: 0x00147580
		private void UpdateUIToolSettings()
		{
			JObject jobject = new JObject();
			jobject["ToolDelayMin"] = this.builderToolsSettings.ToolDelayMin;
			jobject["ToolReachDistance"] = this.ToolsInteractionDistance;
			jobject["ToolReachLock"] = this.builderToolsSettings.ToolReachLock;
			jobject["BrushOpacity"] = this.builderToolsSettings.BrushOpacity;
			jobject["SelectionOpacity"] = this.builderToolsSettings.SelectionOpacity;
			jobject["BrushShapeRendering"] = this.builderToolsSettings.EnableBrushShapeRendering;
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x00149438 File Offset: 0x00147638
		private void UpdateUIForConfiguringTool()
		{
			bool flag = this._gameInstance.App.InGame.Instance.BuilderToolsModule.HasConfigurationToolBrushDataOrArguments() && !this._gameInstance.App.InGame.IsToolsSettingsModalOpened;
			bool flag2 = flag;
			if (flag2)
			{
				this._gameInstance.App.Interface.InGameView.InventoryPage.BuilderToolPanel.ConfiguringToolChange(this._configuringTool);
			}
			this._gameInstance.App.Interface.InGameView.InventoryPage.BuilderToolPanel.Visible = flag;
			ToolInstance configuringTool = this._configuringTool;
			string a;
			if (configuringTool == null)
			{
				a = null;
			}
			else
			{
				ClientItemStack itemStack = configuringTool.ItemStack;
				a = ((itemStack != null) ? itemStack.Id : null);
			}
			bool visible = a == "EditorTool_PlaySelection" && !this._gameInstance.App.InGame.IsToolsSettingsModalOpened;
			this._gameInstance.App.Interface.InGameView.InventoryPage.SelectionCommandsPanel.Visible = visible;
			this._gameInstance.App.Interface.InGameView.InventoryPage.Layout(null, true);
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x00149571 File Offset: 0x00147771
		private void UpdateUIForActiveTool()
		{
			this._gameInstance.App.Interface.InGameView.BuilderToolsLegend.ActiveToolChange(this._activeTool);
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x0014959C File Offset: 0x0014779C
		public void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = !this.HasActiveTool || interactionType == 7;
			if (!flag)
			{
				bool flag2 = clickType != InteractionModule.ClickType.None;
				if (flag2)
				{
					bool flag3 = clickType == InteractionModule.ClickType.Held;
					if (flag3)
					{
						context.State.State = 4;
					}
					long num = DateTime.UtcNow.Ticks / 10000L;
					bool flag4 = num - this.TimeOfLastToolInteraction < (long)this.builderToolsSettings.ToolDelayMin;
					if (flag4)
					{
						return;
					}
					this.TimeOfLastToolInteraction = num;
				}
				bool hasActiveBrush = this.HasActiveBrush;
				if (hasActiveBrush)
				{
					this.Brush.OnInteraction(interactionType, clickType, context, firstRun);
				}
				else
				{
					ToolInstance activeTool = this.ActiveTool;
					bool flag5 = ((activeTool != null) ? activeTool.ClientTool : null) != null;
					if (flag5)
					{
						this.ActiveTool.ClientTool.OnInteraction(interactionType, clickType, context, firstRun);
					}
				}
			}
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x00149674 File Offset: 0x00147874
		public void OnPickBlockInteraction()
		{
			BuilderToolsModule.<>c__DisplayClass65_0 CS$<>8__locals1 = new BuilderToolsModule.<>c__DisplayClass65_0();
			CS$<>8__locals1.<>4__this = this;
			bool flag = !this.HasActiveTool || this.BrushTargetPosition.IsNaN();
			if (!flag)
			{
				int block = this._gameInstance.MapModule.GetBlock(this.BrushTargetPosition, int.MaxValue);
				bool flag2 = block == int.MaxValue;
				if (!flag2)
				{
					BuilderToolsModule.<>c__DisplayClass65_0 CS$<>8__locals2 = CS$<>8__locals1;
					ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
					CS$<>8__locals2.blockName = ((clientBlockType != null) ? clientBlockType.Name : null);
					BuilderToolsModule.PickBlockActionData.ActionType actionType = this._gameInstance.Input.IsAltHeld() ? BuilderToolsModule.PickBlockActionData.ActionType.SetMask : BuilderToolsModule.PickBlockActionData.ActionType.SetMaterial;
					BuilderToolsModule.PickBlockActionData pickBlockActionData = new BuilderToolsModule.PickBlockActionData((int)this.BrushTargetPosition.X, (int)this.BrushTargetPosition.Y, (int)this.BrushTargetPosition.Z, block, actionType);
					bool hasActiveBrush = this.HasActiveBrush;
					if (hasActiveBrush)
					{
						bool flag3 = actionType == BuilderToolsModule.PickBlockActionData.ActionType.SetMaterial;
						string argKey;
						string argValue;
						if (flag3)
						{
							argKey = "Material";
							bool flag4 = pickBlockActionData.Equals(this._lastPickBlockAction);
							if (flag4)
							{
								argValue = CS$<>8__locals1.<OnPickBlockInteraction>g__NextMaterialValue|1(this.ActiveTool.BrushData.Material);
							}
							else
							{
								bool flag5 = this._gameInstance.Input.IsShiftHeld();
								if (flag5)
								{
									argValue = this.ActiveTool.BrushData.Material + "," + CS$<>8__locals1.blockName;
								}
								else
								{
									argValue = CS$<>8__locals1.blockName;
								}
							}
							this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_EYEDROP_SELECT");
						}
						else
						{
							argKey = "Mask";
							string mask = this.ActiveTool.BrushData.Mask;
							bool flag6 = pickBlockActionData.Equals(this._lastPickBlockAction);
							if (flag6)
							{
								argValue = CS$<>8__locals1.<OnPickBlockInteraction>g__NextMaskValue|0(mask);
							}
							else
							{
								bool flag7 = this._gameInstance.Input.IsShiftHeld() && !string.IsNullOrEmpty(mask);
								if (flag7)
								{
									argValue = BuilderToolsModule.AppendBlockNameToMaskList(mask, CS$<>8__locals1.blockName);
								}
								else
								{
									argValue = CS$<>8__locals1.blockName;
								}
							}
							this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_MASK_ADD");
						}
						this.SendActiveToolArgUpdate(1, argKey, argValue, delegate(FailureReply err, SuccessReply reply)
						{
							bool flag11 = reply != null;
							if (flag11)
							{
								CS$<>8__locals1.<>4__this._gameInstance.Chat.Log("Brush " + argKey + " changed to: " + argValue);
							}
							else
							{
								CS$<>8__locals1.<>4__this._gameInstance.Chat.AddBsonMessage(err.Message);
							}
						});
					}
					else
					{
						string argKey = this.ActiveTool.BuilderTool.GetFirstBlockArgId();
						bool flag8 = argKey != null;
						if (flag8)
						{
							ClientItemStack itemStack = this.ActiveTool.ItemStack;
							string argValue2 = (actionType == BuilderToolsModule.PickBlockActionData.ActionType.SetMaterial) ? CS$<>8__locals1.<OnPickBlockInteraction>g__NextMaterialValue|1(this.ActiveTool.BuilderTool.GetItemArgValueOrDefault(ref itemStack, argKey)) : CS$<>8__locals1.<OnPickBlockInteraction>g__NextMaskValue|0(this.ActiveTool.BuilderTool.GetItemArgValueOrDefault(ref itemStack, argKey));
							bool flag9 = pickBlockActionData.Equals(this._lastPickBlockAction);
							string argValue;
							if (flag9)
							{
								argValue = argValue2;
							}
							else
							{
								bool flag10 = this._gameInstance.Input.IsShiftHeld();
								if (flag10)
								{
									argValue = this.ActiveTool.BuilderTool.GetItemArgValueOrDefault(ref itemStack, argKey) + "," + CS$<>8__locals1.blockName;
								}
								else
								{
									argValue = CS$<>8__locals1.blockName;
								}
							}
							this.SendActiveToolArgUpdate(0, argKey, argValue, delegate(FailureReply err, SuccessReply reply)
							{
								bool flag11 = reply != null;
								if (flag11)
								{
									CS$<>8__locals1.<>4__this._gameInstance.Chat.Log(argKey + " changed to: " + argValue);
								}
								else
								{
									CS$<>8__locals1.<>4__this._gameInstance.Chat.AddBsonMessage(err.Message);
								}
							});
						}
					}
					this._lastPickBlockAction = pickBlockActionData;
				}
			}
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x00149A50 File Offset: 0x00147C50
		private static string AppendBlockNameToMaskList(string masks, string blockName)
		{
			bool flag = masks.Equals("-");
			string result;
			if (flag)
			{
				result = blockName;
			}
			else
			{
				string[] array = masks.Split(new char[]
				{
					','
				});
				bool flag2 = array.Length > 6;
				if (flag2)
				{
					ArraySegment<string> arraySegment = new ArraySegment<string>(array, 0, 6);
					masks = string.Join(",", arraySegment);
				}
				result = masks + "," + blockName;
			}
			return result;
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x00149ABC File Offset: 0x00147CBC
		public void setActiveBrushMaterial(string blockName, bool isShiftHeld, bool isAltHeld)
		{
			bool flag = !this.HasActiveBrush;
			if (!flag)
			{
				string argKey;
				string argValue;
				if (isAltHeld)
				{
					argKey = "Mask";
					string mask = this.ActiveTool.BrushData.Mask;
					bool flag2 = isShiftHeld && !string.IsNullOrEmpty(mask);
					if (flag2)
					{
						argValue = BuilderToolsModule.AppendBlockNameToMaskList(mask, blockName);
						this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_MASK_ADD");
					}
					else
					{
						argValue = blockName;
						this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_MASK_SET");
					}
				}
				else
				{
					argKey = "Material";
					if (isShiftHeld)
					{
						argValue = this.ActiveTool.BrushData.Material + "," + blockName;
					}
					else
					{
						argValue = blockName;
					}
					this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_EYEDROP_SELECT");
				}
				this.SendActiveToolArgUpdate(1, argKey, argValue, delegate(FailureReply err, SuccessReply reply)
				{
					bool flag3 = reply != null;
					if (flag3)
					{
						this._gameInstance.Chat.Log("Brush " + argKey + " changed to: " + argValue);
					}
					else
					{
						this._gameInstance.Chat.AddBsonMessage(err.Message);
					}
				});
			}
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x00149BE0 File Offset: 0x00147DE0
		public bool TrySelectActiveTool()
		{
			int activeInventorySectionType = this._gameInstance.InventoryModule.GetActiveInventorySectionType();
			ClientItemStack activeItem = this._gameInstance.InventoryModule.GetActiveItem();
			int activeSlot = this._gameInstance.InventoryModule.GetActiveSlot();
			return this.TrySelectActiveTool(activeInventorySectionType, activeSlot, activeItem);
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x00149C30 File Offset: 0x00147E30
		public bool TrySelectHotbarActiveTool()
		{
			int sectionId = -1;
			int activeHotbarSlot = this._gameInstance.InventoryModule.GetActiveHotbarSlot();
			ClientItemStack activeHotbarItem = this._gameInstance.InventoryModule.GetActiveHotbarItem();
			return this.TrySelectActiveTool(sectionId, activeHotbarSlot, activeHotbarItem);
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x00149C70 File Offset: 0x00147E70
		public bool TrySelectActiveTool(int sectionId, int slot, ClientItemStack itemStack)
		{
			ToolInstance toolInstance;
			bool flag = !this.TrySelectTool(itemStack, out toolInstance);
			bool result;
			if (flag)
			{
				this.ClearConfiguringTool();
				this.ActiveTool = null;
				result = false;
			}
			else
			{
				this.SetConfiguringTool(toolInstance, sectionId, slot);
				this.ActiveTool = toolInstance;
				InGameView inGameView = this._gameInstance.App.Interface.InGameView;
				bool hasActiveBrush = this.HasActiveBrush;
				BrushData brushData = this.ActiveTool.BrushData;
				int? favoriteMaterialsCount;
				if (brushData == null)
				{
					favoriteMaterialsCount = null;
				}
				else
				{
					string[] favoriteMaterials = brushData.FavoriteMaterials;
					favoriteMaterialsCount = ((favoriteMaterials != null) ? new int?(favoriteMaterials.Length) : null);
				}
				inGameView.OnActiveBuilderToolSelected(hasActiveBrush, favoriteMaterialsCount);
				result = true;
			}
			return result;
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x00149D10 File Offset: 0x00147F10
		public bool TryConfigureTool(int sectionId, int slot, ClientItemStack itemStack)
		{
			ToolInstance toolInstance;
			bool flag = !this.TrySelectTool(itemStack, out toolInstance);
			bool result;
			if (flag)
			{
				this.ClearConfiguringTool();
				result = false;
			}
			else
			{
				this._gameInstance.App.InGame.CloseToolsSettingsModal();
				this.SetConfiguringTool(toolInstance, sectionId, slot);
				result = true;
			}
			return result;
		}

		// Token: 0x06004D08 RID: 19720 RVA: 0x00149D5F File Offset: 0x00147F5F
		private void SetConfiguringTool(ToolInstance toolInstance, int sectionId, int slot)
		{
			this.ConfiguringTool = toolInstance;
			this._configuringToolSection = sectionId;
			this._configuringToolSlot = slot;
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x00149D78 File Offset: 0x00147F78
		public void ClearConfiguringTool()
		{
			bool flag = this.ConfiguringTool == null;
			if (!flag)
			{
				this.ConfiguringTool = null;
				this._configuringToolSection = 0;
				this._configuringToolSlot = -1;
			}
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x00149DAC File Offset: 0x00147FAC
		private bool TrySelectTool(ClientItemStack itemStack, out ToolInstance toolInstance)
		{
			toolInstance = null;
			BuilderTool toolFromItemStack = BuilderTool.GetToolFromItemStack(this._gameInstance, itemStack);
			bool flag = toolFromItemStack == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				BrushData brushData = (!toolFromItemStack.IsBrushTool) ? null : new BrushData(itemStack, toolFromItemStack, delegate(string arg, string val)
				{
					this.SendConfiguringToolArgUpdate(1, arg, val, delegate(FailureReply err, SuccessReply reply)
					{
						bool flag3 = err != null;
						if (flag3)
						{
							this._gameInstance.Chat.AddBsonMessage(err.Message);
						}
					});
				});
				this.Brush.UpdateBrushData(brushData, false);
				bool flag2 = brushData != null;
				if (flag2)
				{
					this._gameInstance.App.Interface.InGameView.BuilderToolsMaterialSlotSelector.SetItemStacks(brushData.GetFavoriteMaterialStacks());
				}
				this._gameInstance.App.Interface.InGameView.OnActiveItemSelectorChanged();
				ClientTool clientTool;
				this.ClientTools.TryGetValue(toolFromItemStack.Id, out clientTool);
				toolInstance = new ToolInstance(itemStack, toolFromItemStack, clientTool, brushData);
				result = true;
			}
			return result;
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06004D0B RID: 19723 RVA: 0x00149E7C File Offset: 0x0014807C
		public int ToolsInteractionDistance
		{
			get
			{
				bool useToolReachDistance = this.builderToolsSettings.useToolReachDistance;
				int result;
				if (useToolReachDistance)
				{
					result = this.builderToolsSettings.ToolReachDistance;
				}
				else
				{
					bool flag = this._gameInstance.App.Settings.PlaceBlocksAtRange(this._gameInstance.GameMode) && this._gameInstance.Input.IsAltHeld();
					if (flag)
					{
						result = this._gameInstance.App.Settings.CurrentCreativeInteractionDistance;
					}
					else
					{
						result = this._gameInstance.App.Settings.creativeInteractionDistance;
					}
				}
				return result;
			}
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x00149F11 File Offset: 0x00148111
		public void SendConfiguringToolArgUpdate(BuilderToolArgGroup argGroup, string argId, string argValue, Action<FailureReply, SuccessReply> callback)
		{
			this.SendToolArgUpdate(this._configuringToolSection, this._configuringToolSlot, argGroup, argId, argValue, callback);
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x00149F2C File Offset: 0x0014812C
		private void SendActiveToolArgUpdate(BuilderToolArgGroup argGroup, string argId, string argValue, Action<FailureReply, SuccessReply> callback)
		{
			int activeInventorySectionType = this._gameInstance.InventoryModule.GetActiveInventorySectionType();
			int activeSlot = this._gameInstance.InventoryModule.GetActiveSlot();
			this.SendToolArgUpdate(activeInventorySectionType, activeSlot, argGroup, argId, argValue, callback);
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x00149F6C File Offset: 0x0014816C
		private void SendToolArgUpdate(int sectionId, int slot, BuilderToolArgGroup argGroup, string argId, string argValue, Action<FailureReply, SuccessReply> callback)
		{
			int num = this._gameInstance.AddPendingCallback<SuccessReply>(this, delegate(FailureReply err, SuccessReply reply)
			{
				this._gameInstance.Engine.RunOnMainThread(this, delegate
				{
					Action<FailureReply, SuccessReply> callback2 = callback;
					if (callback2 != null)
					{
						callback2(err, reply);
					}
				}, false, false);
			});
			this._gameInstance.Connection.SendPacket(new BuilderToolArgUpdate(num, sectionId, slot, argGroup, argId, argValue));
		}

		// Token: 0x06004D0F RID: 19727 RVA: 0x00149FC6 File Offset: 0x001481C6
		private void OnUndo()
		{
			this._gameInstance.Connection.SendPacket(new BuilderToolGeneralAction(3));
		}

		// Token: 0x06004D10 RID: 19728 RVA: 0x00149FDF File Offset: 0x001481DF
		private void OnRedo()
		{
			this._gameInstance.Connection.SendPacket(new BuilderToolGeneralAction(4));
		}

		// Token: 0x06004D11 RID: 19729 RVA: 0x00149FF8 File Offset: 0x001481F8
		private void RegisterEvents()
		{
			Interface @interface = this._gameInstance.App.Interface;
			@interface.RegisterForEvent<BuilderToolArgGroup, string, string>("builderTools.argValueChange", this._gameInstance, delegate(BuilderToolArgGroup argGroup, string argId, string argValue)
			{
				this.SendConfiguringToolArgUpdate(argGroup, argId, argValue, delegate(FailureReply err, SuccessReply reply)
				{
					bool flag = err != null;
					if (flag)
					{
						this._gameInstance.Chat.AddBsonMessage(err.Message);
					}
				});
			});
			@interface.RegisterForEvent<ClientItemStack>("builderTools.selectActiveToolMaterial", this._gameInstance, delegate(ClientItemStack stack)
			{
				bool flag = !this.HasActiveBrush;
				if (!flag)
				{
					this.ActiveTool.BrushData.SetBrushMaterial(stack.Id);
					this._gameInstance.App.Interface.InGameView.BuilderToolsLegend.SetSelectedMaterial(stack);
					this._gameInstance.App.Interface.InGameView.BuilderToolsLegend.Layout(null, true);
				}
			});
		}

		// Token: 0x06004D12 RID: 19730 RVA: 0x0014A054 File Offset: 0x00148254
		private void UnregisterEvents()
		{
			Interface @interface = this._gameInstance.App.Interface;
			@interface.UnregisterFromEvent("builderTools.argValueChange");
			@interface.UnregisterFromEvent("builderTools.selectActiveToolMaterial");
		}

		// Token: 0x04002852 RID: 10322
		public readonly Dictionary<string, ClientTool> ClientTools;

		// Token: 0x04002858 RID: 10328
		private ToolInstance _activeTool;

		// Token: 0x04002859 RID: 10329
		public bool DrawHighlightAndUndergroundColor = true;

		// Token: 0x0400285A RID: 10330
		private ToolInstance _configuringTool;

		// Token: 0x0400285B RID: 10331
		private int _configuringToolSection;

		// Token: 0x0400285C RID: 10332
		private int _configuringToolSlot;

		// Token: 0x0400285E RID: 10334
		private BuilderToolsModule.PickBlockActionData _lastPickBlockAction;

		// Token: 0x0400285F RID: 10335
		public long TimeOfLastToolInteraction = 0L;

		// Token: 0x04002860 RID: 10336
		private int _toolSurfaceOffset = 0;

		// Token: 0x04002861 RID: 10337
		public IntVector3 ToolVectorOffset = IntVector3.Zero;

		// Token: 0x04002862 RID: 10338
		public SelectionArea SelectionArea;

		// Token: 0x04002863 RID: 10339
		public static readonly Logger BuilderToolsLogger = LogManager.GetCurrentClassLogger();

		// Token: 0x02000E67 RID: 3687
		private class PickBlockActionData
		{
			// Token: 0x060067A2 RID: 26530 RVA: 0x002183F0 File Offset: 0x002165F0
			public PickBlockActionData(int x, int y, int z, int blockId, BuilderToolsModule.PickBlockActionData.ActionType action)
			{
				this.X = x;
				this.Y = y;
				this.Z = z;
				this.BlockId = blockId;
				this.Action = action;
			}

			// Token: 0x060067A3 RID: 26531 RVA: 0x00218420 File Offset: 0x00216620
			public bool Equals(BuilderToolsModule.PickBlockActionData other)
			{
				return other != null && this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.BlockId == other.BlockId && this.Action == other.Action;
			}

			// Token: 0x0400464A RID: 17994
			public readonly int X;

			// Token: 0x0400464B RID: 17995
			public readonly int Y;

			// Token: 0x0400464C RID: 17996
			public readonly int Z;

			// Token: 0x0400464D RID: 17997
			public readonly int BlockId;

			// Token: 0x0400464E RID: 17998
			public readonly BuilderToolsModule.PickBlockActionData.ActionType Action;

			// Token: 0x020010A2 RID: 4258
			public enum ActionType
			{
				// Token: 0x04004EAB RID: 20139
				SetMaterial,
				// Token: 0x04004EAC RID: 20140
				SetMask
			}
		}
	}
}
