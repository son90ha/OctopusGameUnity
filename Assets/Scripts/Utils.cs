public class Utils
{
    public static float ConvertTo360Degree(float angle)
    {
        var result = angle;

        if (System.Math.Abs(angle) >= 360) {
            result = result % 360;
        }
    
        if (result < 0) {
            result = 360 + (result % 360);
        } else if (result >= 360) {
            result = result % 360;
        }
    
        return result;
    }
}