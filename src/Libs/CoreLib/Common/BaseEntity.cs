namespace CoreLib.Common
{
    public record BaseEntityDal<T> where T : IEquatable<T>
    {
        public T Id { get; set; }
    }
}
