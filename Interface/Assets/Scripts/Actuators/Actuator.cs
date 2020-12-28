public class Actuator
{
    public int nPin = 1;
    public int nValues = 1;
    public int maxValue = 1;
    public int fade = 1;
}

public class RGB : Actuator
{
    public RGB()
    {
        nPin = 3;
        nValues = 3;
    }
}

public class Buzzer : Actuator
{    public Buzzer()
    {
        nPin = 1;
        nValues = 2;
        maxValue = 0;
        fade = 0;
    }
}