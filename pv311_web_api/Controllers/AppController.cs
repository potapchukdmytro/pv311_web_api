using Microsoft.AspNetCore.Mvc;

namespace pv311_web_api.Controllers
{
    public class AppController : ControllerBase
    {
        protected bool ValidateId(string? id, out string message)
        {
            if (string.IsNullOrEmpty(id))
            {
                message = "Id is required";
                return false;
            }

            var parseResult = Guid.TryParse(id, out Guid value);
            if (!parseResult)
            {
                message = "Id incorrect format";
                return false;
            }

            message = "Id valid";
            return true;
        }
    }
}
