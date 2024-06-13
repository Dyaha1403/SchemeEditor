namespace SchemeEditor.Services
{
    public abstract class Service
    {
        protected readonly ApplicationContext _context;
        public Service(ApplicationContext context)
        {
            _context = context;
        }
    }
}
