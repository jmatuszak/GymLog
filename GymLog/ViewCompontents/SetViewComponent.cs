using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLog.ViewCompontents
{
    public class SetViewComponent : ViewComponent
    {
        //private readonly DbContext _context;

        public SetViewComponent()
        {
           // _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            return View();
        }
    }
}
