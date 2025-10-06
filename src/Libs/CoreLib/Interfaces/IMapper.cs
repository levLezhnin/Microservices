namespace CoreLib.Interfaces
{
    public interface IMapper<From, To>
    {
        To? map(From from);
    }
}
