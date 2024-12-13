using IndoorLocalization_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace IndoorLocalization_API.Database
{
    public abstract class APIDatabaseContext<T>:ControllerBase where T : class
    {
        protected readonly IndoorLocalizationContext _context;
        public APIDatabaseContext(IndoorLocalizationContext context)
        {
            _context = context;
            
        }
       
    }
}
