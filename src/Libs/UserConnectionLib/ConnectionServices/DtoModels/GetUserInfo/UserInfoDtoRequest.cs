namespace UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo
{
    public record UserInfoDtoRequest
    {
        public Guid userGuid { get; set; }
    }
}
