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

public enum EGameState
{
    INITILAIZING,
    PLAYING,
    GAMEOVER,
}

public enum EPowerupType
{
    EXTRA_HP,
    EXTRA_PATIENCE,
    SLOW_TIME,
    SIMPLIFY_ORDER,
    SCORE_MULTIPLIER,
    INCREASE_INGREDIENT_WHEEL_SIZE,
    NONE
}

public enum ECharacterState
{
    NORMAL,
    RIGHT_GRAB,
    WRONG_GRAB,
    CUS_SERVED,
    CUS_FAILED,
}
