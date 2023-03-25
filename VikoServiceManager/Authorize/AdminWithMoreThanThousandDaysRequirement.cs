using Microsoft.AspNetCore.Authorization;

namespace VikoServiceManager.Authorize
{
    public class AdminWithMoreThanThousandDaysRequirement : IAuthorizationRequirement
    {
        public int Days { get; set; }

        public AdminWithMoreThanThousandDaysRequirement(int days)
        {
            Days = days;
        }
    }
}
