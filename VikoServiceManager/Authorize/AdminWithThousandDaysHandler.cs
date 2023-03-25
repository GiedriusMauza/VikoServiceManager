using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace VikoServiceManager.Authorize
{
    public class AdminWithThousandDaysHandler : AuthorizationHandler<AdminWithMoreThanThousandDaysRequirement>
    {
        private readonly INumberOfDaysForAccount _numberOfDaysForAccount;

        public AdminWithThousandDaysHandler(INumberOfDaysForAccount numberOfDaysForAccount)
        {
            _numberOfDaysForAccount = numberOfDaysForAccount;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminWithMoreThanThousandDaysRequirement requirement)
        {
            if (!context.User.IsInRole("Admin"))
            {
                return Task.CompletedTask;
            }

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int numberOfDays = _numberOfDaysForAccount.Get(userId);
            if (numberOfDays >= requirement.Days)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
