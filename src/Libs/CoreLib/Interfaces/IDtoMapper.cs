namespace CoreLib.Interfaces
{
    public interface IDtoMapper<RequestDto, Domain, ResponseDto>
    {
        ResponseDto? toDto(Domain domain);
        Domain? toDomain(RequestDto request);
    }
}
