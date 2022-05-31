using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;
public enum EItemType
{
    BLACK,
	GRAY,
	RED,
	GREEN,
	BLUE,
	YELLOW,
	ORANGE,
	CYAN,
	MAGENTA,
}

public static class CONST {
    public static readonly List<EItemType> ListItemType = new List<EItemType>{EItemType.BLACK, EItemType.GRAY, EItemType.GREEN, EItemType.BLUE, EItemType.YELLOW, EItemType.ORANGE, EItemType.CYAN, EItemType.MAGENTA};
}

