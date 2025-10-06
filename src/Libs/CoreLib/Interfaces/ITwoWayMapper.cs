namespace CoreLib.Interfaces
{
    public interface ITwoWayMapper<From, To>
    {
        To? mapForward(From from);
        From? mapBackward(To to);
    }
}
