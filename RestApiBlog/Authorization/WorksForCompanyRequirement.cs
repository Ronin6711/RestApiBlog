using Microsoft.AspNetCore.Authorization;

namespace RestApiBlog.Authorization
{
    public class WorksForCompanyRequirement : IAuthorizationRequirement
    {
        public string DomainName { get; }
        public WorksForCompanyRequirement(string domainName)
        {
            DomainName = domainName;
        }

    }
}
