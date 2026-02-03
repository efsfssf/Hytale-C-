using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000062 RID: 98
	[Flags]
	public enum KeyCombination
	{
		// Token: 0x04000203 RID: 515
		ModifierShift = 16,
		// Token: 0x04000204 RID: 516
		KeyTypeMask = 65535,
		// Token: 0x04000205 RID: 517
		ModifierMask = -65536,
		// Token: 0x04000206 RID: 518
		Shift = 65536,
		// Token: 0x04000207 RID: 519
		Control = 131072,
		// Token: 0x04000208 RID: 520
		Alt = 262144,
		// Token: 0x04000209 RID: 521
		Meta = 524288,
		// Token: 0x0400020A RID: 522
		ValidModifierMask = 983040,
		// Token: 0x0400020B RID: 523
		None = 0,
		// Token: 0x0400020C RID: 524
		Space = 1,
		// Token: 0x0400020D RID: 525
		Backspace = 2,
		// Token: 0x0400020E RID: 526
		Tab = 3,
		// Token: 0x0400020F RID: 527
		Escape = 4,
		// Token: 0x04000210 RID: 528
		PageUp = 5,
		// Token: 0x04000211 RID: 529
		PageDown = 6,
		// Token: 0x04000212 RID: 530
		End = 7,
		// Token: 0x04000213 RID: 531
		Home = 8,
		// Token: 0x04000214 RID: 532
		Insert = 9,
		// Token: 0x04000215 RID: 533
		Delete = 10,
		// Token: 0x04000216 RID: 534
		Left = 11,
		// Token: 0x04000217 RID: 535
		Up = 12,
		// Token: 0x04000218 RID: 536
		Right = 13,
		// Token: 0x04000219 RID: 537
		Down = 14,
		// Token: 0x0400021A RID: 538
		Key0 = 15,
		// Token: 0x0400021B RID: 539
		Key1 = 16,
		// Token: 0x0400021C RID: 540
		Key2 = 17,
		// Token: 0x0400021D RID: 541
		Key3 = 18,
		// Token: 0x0400021E RID: 542
		Key4 = 19,
		// Token: 0x0400021F RID: 543
		Key5 = 20,
		// Token: 0x04000220 RID: 544
		Key6 = 21,
		// Token: 0x04000221 RID: 545
		Key7 = 22,
		// Token: 0x04000222 RID: 546
		Key8 = 23,
		// Token: 0x04000223 RID: 547
		Key9 = 24,
		// Token: 0x04000224 RID: 548
		KeyA = 25,
		// Token: 0x04000225 RID: 549
		KeyB = 26,
		// Token: 0x04000226 RID: 550
		KeyC = 27,
		// Token: 0x04000227 RID: 551
		KeyD = 28,
		// Token: 0x04000228 RID: 552
		KeyE = 29,
		// Token: 0x04000229 RID: 553
		KeyF = 30,
		// Token: 0x0400022A RID: 554
		KeyG = 31,
		// Token: 0x0400022B RID: 555
		KeyH = 32,
		// Token: 0x0400022C RID: 556
		KeyI = 33,
		// Token: 0x0400022D RID: 557
		KeyJ = 34,
		// Token: 0x0400022E RID: 558
		KeyK = 35,
		// Token: 0x0400022F RID: 559
		KeyL = 36,
		// Token: 0x04000230 RID: 560
		KeyM = 37,
		// Token: 0x04000231 RID: 561
		KeyN = 38,
		// Token: 0x04000232 RID: 562
		KeyO = 39,
		// Token: 0x04000233 RID: 563
		KeyP = 40,
		// Token: 0x04000234 RID: 564
		KeyQ = 41,
		// Token: 0x04000235 RID: 565
		KeyR = 42,
		// Token: 0x04000236 RID: 566
		KeyS = 43,
		// Token: 0x04000237 RID: 567
		KeyT = 44,
		// Token: 0x04000238 RID: 568
		KeyU = 45,
		// Token: 0x04000239 RID: 569
		KeyV = 46,
		// Token: 0x0400023A RID: 570
		KeyW = 47,
		// Token: 0x0400023B RID: 571
		KeyX = 48,
		// Token: 0x0400023C RID: 572
		KeyY = 49,
		// Token: 0x0400023D RID: 573
		KeyZ = 50,
		// Token: 0x0400023E RID: 574
		Numpad0 = 51,
		// Token: 0x0400023F RID: 575
		Numpad1 = 52,
		// Token: 0x04000240 RID: 576
		Numpad2 = 53,
		// Token: 0x04000241 RID: 577
		Numpad3 = 54,
		// Token: 0x04000242 RID: 578
		Numpad4 = 55,
		// Token: 0x04000243 RID: 579
		Numpad5 = 56,
		// Token: 0x04000244 RID: 580
		Numpad6 = 57,
		// Token: 0x04000245 RID: 581
		Numpad7 = 58,
		// Token: 0x04000246 RID: 582
		Numpad8 = 59,
		// Token: 0x04000247 RID: 583
		Numpad9 = 60,
		// Token: 0x04000248 RID: 584
		NumpadAsterisk = 61,
		// Token: 0x04000249 RID: 585
		NumpadPlus = 62,
		// Token: 0x0400024A RID: 586
		NumpadMinus = 63,
		// Token: 0x0400024B RID: 587
		NumpadPeriod = 64,
		// Token: 0x0400024C RID: 588
		NumpadDivide = 65,
		// Token: 0x0400024D RID: 589
		F1 = 66,
		// Token: 0x0400024E RID: 590
		F2 = 67,
		// Token: 0x0400024F RID: 591
		F3 = 68,
		// Token: 0x04000250 RID: 592
		F4 = 69,
		// Token: 0x04000251 RID: 593
		F5 = 70,
		// Token: 0x04000252 RID: 594
		F6 = 71,
		// Token: 0x04000253 RID: 595
		F7 = 72,
		// Token: 0x04000254 RID: 596
		F8 = 73,
		// Token: 0x04000255 RID: 597
		F9 = 74,
		// Token: 0x04000256 RID: 598
		F10 = 75,
		// Token: 0x04000257 RID: 599
		F11 = 76,
		// Token: 0x04000258 RID: 600
		F12 = 77,
		// Token: 0x04000259 RID: 601
		F13 = 78,
		// Token: 0x0400025A RID: 602
		F14 = 79,
		// Token: 0x0400025B RID: 603
		F15 = 80,
		// Token: 0x0400025C RID: 604
		F16 = 81,
		// Token: 0x0400025D RID: 605
		F17 = 82,
		// Token: 0x0400025E RID: 606
		F18 = 83,
		// Token: 0x0400025F RID: 607
		F19 = 84,
		// Token: 0x04000260 RID: 608
		F20 = 85,
		// Token: 0x04000261 RID: 609
		F21 = 86,
		// Token: 0x04000262 RID: 610
		F22 = 87,
		// Token: 0x04000263 RID: 611
		F23 = 88,
		// Token: 0x04000264 RID: 612
		F24 = 89,
		// Token: 0x04000265 RID: 613
		OemPlus = 90,
		// Token: 0x04000266 RID: 614
		OemComma = 91,
		// Token: 0x04000267 RID: 615
		OemMinus = 92,
		// Token: 0x04000268 RID: 616
		OemPeriod = 93,
		// Token: 0x04000269 RID: 617
		Oem1 = 94,
		// Token: 0x0400026A RID: 618
		Oem2 = 95,
		// Token: 0x0400026B RID: 619
		Oem3 = 96,
		// Token: 0x0400026C RID: 620
		Oem4 = 97,
		// Token: 0x0400026D RID: 621
		Oem5 = 98,
		// Token: 0x0400026E RID: 622
		Oem6 = 99,
		// Token: 0x0400026F RID: 623
		Oem7 = 100,
		// Token: 0x04000270 RID: 624
		Oem8 = 101,
		// Token: 0x04000271 RID: 625
		MaxKeyType = 102
	}
}
