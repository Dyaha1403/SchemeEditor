using SchemeEditor.Controls;
using SchemeEditor.Entities;

namespace SchemeEditor.Services
{
    public class ControlService : Service
    {
        public ControlService(ApplicationContext context) : base(context)
        {

        }

        public static ControlDTO FromControlToControlDTO(BaseControl control)
        {
            ControlDTO controlDTO = new ControlDTO()
            {
                Id = Guid.NewGuid(),
                Type = control.GetType(),
                Angle = control.Angle
            };

            return controlDTO;
        }

        public static BaseControl FromControlDTOToControl(ControlDTO controlDTO)
        {
            BaseControl control = (BaseControl)Activator.CreateInstance(controlDTO.Type)!;
            return control;
        }

        public void UpdateAngle(Guid controlDTOId, double angle)
        {
            using (_context)
            {
                ControlDTO? controlDTO = _context.Controls.FirstOrDefault(item => item.Id == controlDTOId);

                if(controlDTO != null)
                {
                    controlDTO.Angle = angle;
                    _context.SaveChanges();
                }
            }
        }
    }
}
