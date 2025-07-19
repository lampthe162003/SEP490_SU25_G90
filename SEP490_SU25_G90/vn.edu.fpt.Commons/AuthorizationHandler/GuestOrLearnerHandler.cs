using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SEP490_SU25_G90.vn.edu.fpt.Commons.AuthorizationHandler
{
    public class GuestOrLearnerHandler : AuthorizationHandler<GuestOrLearnerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GuestOrLearnerRequirement requirement)
        {
            // Allow if user is not authenticated (guest)
            if (!context.User.Identity?.IsAuthenticated == true)
            {
                context.Succeed(requirement);
            }
            // Allow if user is in "Learner" role
            else if (context.User.IsInRole("learner"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}