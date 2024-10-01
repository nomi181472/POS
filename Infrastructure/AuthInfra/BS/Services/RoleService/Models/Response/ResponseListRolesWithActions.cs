namespace BS.Services.RoleService.Models.Response
{
    public class ResponseListRolesWithActions
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<ActionInResponseListRolesWithActions> Actions { get; set; }
    }

    public class ActionInResponseListRolesWithActions
    {
        public string ActionId { get; set; }
        public string ActionName { get; set; }
    }

}
