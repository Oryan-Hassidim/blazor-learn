using System.Collections.Generic;

namespace Shared;

public class ClientPrincipal
{
    public string IdentityProvider { get; set; }
    public string UserId { get; set; }
    public string UserDetails { get; set; }
    public IEnumerable<string> UserRoles { get; set; }
}
public class ClientPrincipalRoot
{
    public ClientPrincipal clientPrincipal { get; set; }
}