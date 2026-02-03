using System;
using System.Collections.Generic;
using System.Globalization;
using HytaleClient.InGame.Commands;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using SDL2;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008EC RID: 2284
	internal class AutoCameraModule : Module
	{
		// Token: 0x0600438B RID: 17291 RVA: 0x000D6B9C File Offset: 0x000D4D9C
		public AutoCameraModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._autoCameraController = new AutoCameraModule.AutoCameraController(this._gameInstance);
			this._gameInstance.RegisterCommand("cam", new GameInstance.Command(this.CamCommand));
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x000D6BEC File Offset: 0x000D4DEC
		[Usage("cam", new string[]
		{
			"add [index] [[x y z] [yaw pitch]]",
			"remove [index]",
			"clear",
			"list",
			"start [speed]",
			"pause",
			"stop",
			"save",
			"load"
		})]
		public void CamCommand(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			int num = 0;
			string text = args[num++].ToLower();
			string text2 = text;
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num2 <= 1697318111U)
			{
				if (num2 <= 993596020U)
				{
					if (num2 != 217798785U)
					{
						if (num2 == 993596020U)
						{
							if (text2 == "add")
							{
								int num3 = -1;
								bool flag2 = args.Length == 1 + num || args.Length >= 4 + num;
								if (flag2)
								{
									num3 = int.Parse(args[num++], CultureInfo.InvariantCulture);
									bool flag3 = num3 < 0;
									if (flag3)
									{
										this._gameInstance.Chat.Log("Index must be greater than 0");
										return;
									}
									bool flag4 = num3 > this._positions.Count;
									if (flag4)
									{
										this._gameInstance.Chat.Log(string.Format("Index must be less than than the number of positions saved, {0}", this._positions.Count));
										return;
									}
								}
								else
								{
									bool flag5 = args.Length != num;
									if (flag5)
									{
										this._gameInstance.Chat.Log("Invalid usage!");
										return;
									}
								}
								Vector3 position = this._gameInstance.LocalPlayer.Position;
								float num4 = position.X;
								float num5 = position.Y;
								float num6 = position.Z;
								bool flag6 = args.Length > 3 + num;
								if (flag6)
								{
									num4 = float.Parse(args[num++], CultureInfo.InvariantCulture);
									num5 = float.Parse(args[num++], CultureInfo.InvariantCulture);
									num6 = float.Parse(args[num++], CultureInfo.InvariantCulture);
								}
								float num7 = this._gameInstance.LocalPlayer.LookOrientation.Pitch;
								float num8 = this._gameInstance.LocalPlayer.LookOrientation.Yaw;
								bool flag7 = args.Length > 2 + num;
								if (flag7)
								{
									num8 = MathHelper.ToRadians(float.Parse(args[num++], CultureInfo.InvariantCulture));
									num7 = MathHelper.ToRadians(float.Parse(args[num], CultureInfo.InvariantCulture));
								}
								Tuple<Vector3, Vector2> item = new Tuple<Vector3, Vector2>(new Vector3(num4, num5, num6), new Vector2(num7, num8));
								bool flag8 = num3 >= 0;
								if (flag8)
								{
									this._positions.Insert(num3, item);
									this._gameInstance.Chat.Log(string.Format("Insert position at {0}: X: {1}, Y: {2}, Z: {3}, Yaw: {4}, Pitch: {5}", new object[]
									{
										num3,
										num4,
										num5,
										num6,
										num8,
										num7
									}));
								}
								else
								{
									this._positions.Add(item);
									this._gameInstance.Chat.Log(string.Format("Added position at {0}: X: {1}, Y: {2}, Z: {3}, Yaw: {4}, Pitch: {5}", new object[]
									{
										this._positions.Count - 1,
										num4,
										num5,
										num6,
										num8,
										num7
									}));
								}
								return;
							}
						}
					}
					else if (text2 == "list")
					{
						this._gameInstance.Chat.Log("Points:");
						for (int i = 0; i < this._positions.Count; i++)
						{
							Tuple<Vector3, Vector2> tuple = this._positions[i];
							this._gameInstance.Chat.Log(string.Format("{0}: {1}, {2}, {3}, {4}, {5}", new object[]
							{
								i,
								tuple.Item1.X,
								tuple.Item1.Y,
								tuple.Item1.Z,
								tuple.Item2.X,
								tuple.Item2.Y
							}));
						}
						return;
					}
				}
				else if (num2 != 1550717474U)
				{
					if (num2 == 1697318111U)
					{
						if (text2 == "start")
						{
							bool flag9 = !this._gameInstance.CameraModule.IsCustomCameraControllerSet();
							if (flag9)
							{
								bool flag10 = this._positions.Count <= 0;
								if (flag10)
								{
									this._gameInstance.Chat.Log("No points stored! Use .cam add");
								}
								else
								{
									bool flag11 = args.Length > num;
									if (flag11)
									{
										this._autoCameraController.Speed = float.Parse(args[num], CultureInfo.InvariantCulture);
									}
									else
									{
										this._autoCameraController.Speed = 1f;
									}
									this._autoCameraController.Start(this._positions);
									this._gameInstance.Chat.Log("Started Auto Camera!");
								}
							}
							else
							{
								this._gameInstance.Chat.Log("A custom camera controller is already set! Disable it before enabling the camera mod.");
							}
							return;
						}
					}
				}
				else if (text2 == "clear")
				{
					this._positions.Clear();
					this._gameInstance.Chat.Log("Cleared points!");
					return;
				}
			}
			else if (num2 <= 3411225317U)
			{
				if (num2 != 1887753101U)
				{
					if (num2 == 3411225317U)
					{
						if (text2 == "stop")
						{
							bool flag12 = !this._gameInstance.CameraModule.IsCustomCameraControllerSet();
							if (flag12)
							{
								this._gameInstance.Chat.Log("The auto camera has not been started.");
							}
							else
							{
								bool flag13 = this._gameInstance.CameraModule.Controller == this._autoCameraController;
								if (flag13)
								{
									this._autoCameraController.Stop();
									this._gameInstance.CameraModule.ResetCameraController();
									this._gameInstance.Chat.Log("Stopped Auto Camera!");
								}
								else
								{
									bool paused = this._autoCameraController.Paused;
									if (paused)
									{
										this._autoCameraController.Stop();
										this._gameInstance.Chat.Log("Stopped Auto Camera!");
									}
									else
									{
										this._gameInstance.Chat.Log("A custom camera controller is already set! Disable it before enabling the camera mod.");
									}
								}
							}
							return;
						}
					}
				}
				else if (text2 == "pause")
				{
					bool flag14 = !this._gameInstance.CameraModule.IsCustomCameraControllerSet();
					if (flag14)
					{
						this._gameInstance.Chat.Log("The auto camera has not been started.");
					}
					else
					{
						bool flag15 = this._gameInstance.CameraModule.Controller == this._autoCameraController;
						if (flag15)
						{
							this._autoCameraController.Paused = true;
							this._gameInstance.CameraModule.ResetCameraController();
							this._gameInstance.Chat.Log("Paused Auto Camera!");
						}
						else
						{
							this._gameInstance.Chat.Log("A custom camera controller is already set! Disable it before enabling the camera mod.");
						}
					}
					return;
				}
			}
			else if (num2 != 3439296072U)
			{
				if (num2 != 3683784189U)
				{
					if (num2 == 3859241449U)
					{
						if (text2 == "load")
						{
							try
							{
								string text3 = SDL.SDL_GetClipboardText();
								JArray jarray = JArray.Parse(text3);
								List<Tuple<Vector3, Vector2>> collection = jarray.ToObject<List<Tuple<Vector3, Vector2>>>();
								this._positions.Clear();
								this._positions.AddRange(collection);
								this._gameInstance.Chat.Log("Loaded the camera track from clipboard!");
							}
							catch (Exception ex)
							{
								this._gameInstance.Chat.Log("Failed to parse clipboard contents! " + ex.Message);
								AutoCameraModule.Logger.Error(ex, "Failed to parse clipboard contents:");
							}
							return;
						}
					}
				}
				else if (text2 == "remove")
				{
					int num9 = -1;
					bool flag16 = args.Length == 1 + num;
					if (flag16)
					{
						num9 = int.Parse(args[num], CultureInfo.InvariantCulture);
						bool flag17 = num9 < 0;
						if (flag17)
						{
							this._gameInstance.Chat.Log("Index must be greater than 0");
							return;
						}
						bool flag18 = num9 >= this._positions.Count;
						if (flag18)
						{
							this._gameInstance.Chat.Log(string.Format("Index must be less than than the number of positions saved {0}", this._positions.Count));
							return;
						}
					}
					bool flag19 = num9 == -1;
					if (flag19)
					{
						num9 = this._positions.Count - 1;
					}
					Tuple<Vector3, Vector2> tuple2 = this._positions[num9];
					this._positions.RemoveAt(num9);
					this._gameInstance.Chat.Log(string.Format("Removed point at {0}: {1}, {2}, {3}, {4}, {5}", new object[]
					{
						num9,
						tuple2.Item1.X,
						tuple2.Item1.Y,
						tuple2.Item1.Z,
						tuple2.Item2.X,
						tuple2.Item2.Y
					}));
					return;
				}
			}
			else if (text2 == "save")
			{
				SDL.SDL_SetClipboardText(JsonConvert.SerializeObject(this._positions, 1));
				this._gameInstance.Chat.Log("Copied camera track to clipboard!");
				return;
			}
			throw new InvalidCommandUsage();
		}

		// Token: 0x0400214A RID: 8522
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400214B RID: 8523
		private readonly List<Tuple<Vector3, Vector2>> _positions = new List<Tuple<Vector3, Vector2>>();

		// Token: 0x0400214C RID: 8524
		private readonly AutoCameraModule.AutoCameraController _autoCameraController;

		// Token: 0x02000DB4 RID: 3508
		private class AutoCameraController : ICameraController
		{
			// Token: 0x17001442 RID: 5186
			// (get) Token: 0x060065FB RID: 26107 RVA: 0x00212976 File Offset: 0x00210B76
			// (set) Token: 0x060065FC RID: 26108 RVA: 0x0021297E File Offset: 0x00210B7E
			public float SpeedModifier { get; set; } = 1f;

			// Token: 0x17001443 RID: 5187
			// (get) Token: 0x060065FD RID: 26109 RVA: 0x00212987 File Offset: 0x00210B87
			public Vector3 AttachmentPosition
			{
				get
				{
					return Vector3.Zero;
				}
			}

			// Token: 0x17001444 RID: 5188
			// (get) Token: 0x060065FE RID: 26110 RVA: 0x0021298E File Offset: 0x00210B8E
			public Vector3 PositionOffset
			{
				get
				{
					return Vector3.Zero;
				}
			}

			// Token: 0x17001445 RID: 5189
			// (get) Token: 0x060065FF RID: 26111 RVA: 0x00212995 File Offset: 0x00210B95
			public Vector3 RotationOffset
			{
				get
				{
					return Vector3.Zero;
				}
			}

			// Token: 0x17001446 RID: 5190
			// (get) Token: 0x06006600 RID: 26112 RVA: 0x0021299C File Offset: 0x00210B9C
			// (set) Token: 0x06006601 RID: 26113 RVA: 0x002129A4 File Offset: 0x00210BA4
			public Vector3 Position { get; private set; }

			// Token: 0x17001447 RID: 5191
			// (get) Token: 0x06006602 RID: 26114 RVA: 0x002129AD File Offset: 0x00210BAD
			// (set) Token: 0x06006603 RID: 26115 RVA: 0x002129B5 File Offset: 0x00210BB5
			public Vector3 Rotation { get; private set; }

			// Token: 0x17001448 RID: 5192
			// (get) Token: 0x06006604 RID: 26116 RVA: 0x002129BE File Offset: 0x00210BBE
			// (set) Token: 0x06006605 RID: 26117 RVA: 0x002129C6 File Offset: 0x00210BC6
			public Vector3 LookAt { get; private set; }

			// Token: 0x17001449 RID: 5193
			// (get) Token: 0x06006606 RID: 26118 RVA: 0x002129CF File Offset: 0x00210BCF
			public Vector3 MovementForceRotation
			{
				get
				{
					return this.Rotation;
				}
			}

			// Token: 0x1700144A RID: 5194
			// (get) Token: 0x06006607 RID: 26119 RVA: 0x002129D7 File Offset: 0x00210BD7
			// (set) Token: 0x06006608 RID: 26120 RVA: 0x002129DA File Offset: 0x00210BDA
			public Entity AttachedTo
			{
				get
				{
					return null;
				}
				set
				{
				}
			}

			// Token: 0x1700144B RID: 5195
			// (get) Token: 0x06006609 RID: 26121 RVA: 0x002129DD File Offset: 0x00210BDD
			public bool IsFirstPerson
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700144C RID: 5196
			// (get) Token: 0x0600660A RID: 26122 RVA: 0x002129E0 File Offset: 0x00210BE0
			public bool SkipCharacterPhysics
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700144D RID: 5197
			// (get) Token: 0x0600660B RID: 26123 RVA: 0x002129E3 File Offset: 0x00210BE3
			public bool CanMove
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700144E RID: 5198
			// (get) Token: 0x0600660C RID: 26124 RVA: 0x002129E6 File Offset: 0x00210BE6
			public bool AllowPitchControls
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700144F RID: 5199
			// (get) Token: 0x0600660D RID: 26125 RVA: 0x002129E9 File Offset: 0x00210BE9
			public bool DisplayCursor
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001450 RID: 5200
			// (get) Token: 0x0600660E RID: 26126 RVA: 0x002129EC File Offset: 0x00210BEC
			public bool DisplayReticle
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001451 RID: 5201
			// (get) Token: 0x0600660F RID: 26127 RVA: 0x002129EF File Offset: 0x00210BEF
			public bool InteractFromEntity
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001452 RID: 5202
			// (get) Token: 0x06006610 RID: 26128 RVA: 0x002129F2 File Offset: 0x00210BF2
			// (set) Token: 0x06006611 RID: 26129 RVA: 0x002129FA File Offset: 0x00210BFA
			public bool Paused { get; set; }

			// Token: 0x06006612 RID: 26130 RVA: 0x00212A03 File Offset: 0x00210C03
			public AutoCameraController(GameInstance gameInstance)
			{
				this._gameInstance = gameInstance;
			}

			// Token: 0x06006613 RID: 26131 RVA: 0x00212A2A File Offset: 0x00210C2A
			public void Reset(GameInstance gameInstance, ICameraController previousCameraController)
			{
			}

			// Token: 0x06006614 RID: 26132 RVA: 0x00212A2D File Offset: 0x00210C2D
			public void ApplyLook(float deltaTime, Vector2 look)
			{
			}

			// Token: 0x06006615 RID: 26133 RVA: 0x00212A30 File Offset: 0x00210C30
			public void SetRotation(Vector3 rotation)
			{
			}

			// Token: 0x06006616 RID: 26134 RVA: 0x00212A33 File Offset: 0x00210C33
			public void ApplyMove(Vector3 move)
			{
			}

			// Token: 0x06006617 RID: 26135 RVA: 0x00212A36 File Offset: 0x00210C36
			public void OnMouseInput(SDL.SDL_Event evt)
			{
			}

			// Token: 0x06006618 RID: 26136 RVA: 0x00212A39 File Offset: 0x00210C39
			public void Start(List<Tuple<Vector3, Vector2>> positions)
			{
				this._gameInstance.CameraModule.SetCustomCameraController(this);
				this._positions = new List<Tuple<Vector3, Vector2>>(positions);
				this.Paused = false;
				this._accumDelta = 0f;
			}

			// Token: 0x06006619 RID: 26137 RVA: 0x00212A6D File Offset: 0x00210C6D
			public void Stop()
			{
				this._positions = null;
				this._accumDelta = 0f;
			}

			// Token: 0x0600661A RID: 26138 RVA: 0x00212A84 File Offset: 0x00210C84
			public void Update(float deltaTime)
			{
				bool flag = this._positions == null || this.Paused;
				if (!flag)
				{
					this._accumDelta += deltaTime;
					float num = this._accumDelta * this.Speed;
					int num2 = -1;
					float num3 = 0f;
					for (int i = 0; i < this._positions.Count - 1; i++)
					{
						Tuple<Vector3, Vector2> tuple = this._positions[i];
						Tuple<Vector3, Vector2> tuple2 = this._positions[i + 1];
						float num4 = Vector3.Distance(tuple.Item1, tuple2.Item1);
						bool flag2 = num >= num3 && num < num3 + num4;
						if (flag2)
						{
							num2 = i;
							break;
						}
						num3 += num4;
					}
					bool flag3 = num2 == -1;
					if (flag3)
					{
						this._gameInstance.CameraModule.ResetCameraController();
						this.Stop();
					}
					else
					{
						Tuple<Vector3, Vector2> tuple3 = this._positions[num2];
						Tuple<Vector3, Vector2> tuple4 = this._positions[num2 + 1];
						Tuple<Vector3, Vector2> tuple5 = (num2 - 1 >= 0) ? this._positions[num2 - 1] : tuple3;
						Tuple<Vector3, Vector2> tuple6 = (num2 + 2 < this._positions.Count) ? this._positions[num2 + 2] : tuple4;
						float t = (num - num3) / Vector3.Distance(tuple3.Item1, tuple4.Item1);
						Vector3 item = tuple5.Item1;
						Vector3 item2 = tuple3.Item1;
						Vector3 item3 = tuple4.Item1;
						Vector3 item4 = tuple6.Item1;
						Vector3.Spline(ref t, ref item, ref item2, ref item3, ref item4, out this._tempVec3);
						this.Position = this._tempVec3;
						Vector2 item5 = tuple5.Item2;
						Vector2 item6 = tuple3.Item2;
						Vector2 item7 = tuple4.Item2;
						Vector2 item8 = tuple6.Item2;
						float value = MathHelper.Spline(t, item5.X, item6.X, item7.X, item8.X);
						float angle = MathHelper.SplineAngle(t, item5.Y, item6.Y, item7.Y, item8.Y);
						this.Rotation = new Vector3(MathHelper.Clamp(value, -1.5707964f, 1.5707964f), MathHelper.WrapAngle(angle), 0f);
					}
				}
			}

			// Token: 0x04004388 RID: 17288
			public float Speed = 1f;

			// Token: 0x04004389 RID: 17289
			private readonly GameInstance _gameInstance;

			// Token: 0x0400438A RID: 17290
			private float _accumDelta;

			// Token: 0x0400438B RID: 17291
			private Vector3 _tempVec3;

			// Token: 0x0400438C RID: 17292
			private List<Tuple<Vector3, Vector2>> _positions;
		}
	}
}
