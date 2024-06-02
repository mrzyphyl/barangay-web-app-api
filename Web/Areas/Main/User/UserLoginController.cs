using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Main.User
{
    public class UserLoginController : Controller
    {
        public JsonResult LoginUser() {
			try {

                return Json("sucess");
			}
			catch (Exception e) {
                throw new InvalidOperationException(e.Message);
			}

        }
    }
}
