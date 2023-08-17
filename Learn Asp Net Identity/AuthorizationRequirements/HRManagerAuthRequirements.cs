using Microsoft.AspNetCore.Authorization;

namespace Learn_Asp_Net_Identity.AuthorizationRequirements;

public class HRManagerAuthRequirements : IAuthorizationRequirement
{
    public int ProbationMonths { get; }


    public HRManagerAuthRequirements(int probationMonths)
    {
        ProbationMonths = probationMonths;
    }



    public class HRManagerProbationHandler : AuthorizationHandler<HRManagerAuthRequirements>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerAuthRequirements requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "EmploymentDate")) return Task.CompletedTask;

            var empDate = DateTime.Parse(context.User.FindFirst(x => x.Type == "EmploymentDate")?.Value);
            var period = DateTime.Now - empDate;
            if (period.Days > 30 * requirement.ProbationMonths)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
   
    
}

