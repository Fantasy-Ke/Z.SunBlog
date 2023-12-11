using Microsoft.AspNetCore.Mvc.Filters;

namespace Z.Fantasy.Core.ResultResponse;

public interface IActionResultWarp
{
    void Wrap(FilterContext context);
}
