namespace UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo
{
    public record UserInfoDtoResponse
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
    }
}
