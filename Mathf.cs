using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Mathf
{
    public static int Clamp(int val, int min, int max)
    {
        if(val < min)
            return min;
        else if(val > max)
            return max;
        return val;
    }
}
