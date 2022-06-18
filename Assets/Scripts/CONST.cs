using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;
public enum EItemType
{
    POWER_UP,
    BLACK,
	GRAY,
	RED,
	GREEN,
	BLUE,
    YELLOW,
    CYAN,
	MAGENTA,
    ORANGE,
}

public static class CONST {
    public static readonly List<EItemType> ListItemType = new List<EItemType>((EItemType[])Enum.GetValues(typeof(EItemType)));
}

