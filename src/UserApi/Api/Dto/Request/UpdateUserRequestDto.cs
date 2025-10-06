namespace UserApi.Api.Dto.Request
{
    public class UpdateUserRequestDto
    {
        public Guid id { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public bool? isActive { get; init; }
    }
}
