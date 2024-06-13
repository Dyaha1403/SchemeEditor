using Microsoft.EntityFrameworkCore;
using SchemeEditor.Entities;
using SchemeEditor.Models;

namespace SchemeEditor.Services
{
    public class SchemeService : Service
    {
        public SchemeService(ApplicationContext context) : base(context)
        {
            
        }

        public SchemeDTO CreateScheme(string name)
        {
            SchemeDTO schemeDTO = new SchemeDTO()
            {
                Id = Guid.NewGuid(),
                CanvasItems = new List<CanvasItemDTO>(),
                Connections = new List<ConnectionDTO>(),
                Name = name
            };

            AddToContext(schemeDTO);

            return schemeDTO;
        }

        public SchemeDTO? ChangeNameScheme(Guid schemeId, string newName)
        {
            using (_context)
            {
                SchemeDTO? scheme = _context.Schemes.FirstOrDefault(s => s.Id == schemeId);
                if (scheme != null)
                {
                    scheme.Name = newName;
                    _context.SaveChanges();
                }
                return scheme;
            }

        }

        public CanvasItemDTO AddCanvasItemDTO(Guid schemeId, CanvasItemDTO newCanvasItemDTO)
        {
            using (_context)
            {
                SchemeDTO? scheme = _context.Schemes.Include(c => c.CanvasItems).FirstOrDefault(item => item.Id == schemeId);

                if(scheme != null && scheme.CanvasItems != null)
                {
                    newCanvasItemDTO.SchemeId = schemeId;
                    _context.CanvasItems.Add(newCanvasItemDTO);
                    scheme.CanvasItems.Add(newCanvasItemDTO);
                    _context.SaveChanges();
                }

                return newCanvasItemDTO;
            }
        }

        public ConnectionDTO AddConnectionDTO(Guid schemeId, ConnectionDTO newConnectionDTO)
        {
            using (_context)
            {
                SchemeDTO? scheme = _context.Schemes.Include(c => c.Connections).FirstOrDefault(item => item.Id == schemeId);

                if(scheme != null && scheme.Connections != null)
                {
                    newConnectionDTO.SchemeId = schemeId;
                    _context.Connections.Add(newConnectionDTO);
                    scheme.Connections.Add(newConnectionDTO);
                    _context.SaveChanges();
                }

                return newConnectionDTO;
            }
        }

        public CanvasItemDTO? RemoveCanvasItem(Guid schemeId, Guid canvasItemId)
        {
            using (_context)
            {
                SchemeDTO? scheme = _context.Schemes.Include(c => c.CanvasItems).FirstOrDefault(item => item.Id == schemeId);
                CanvasItemDTO? canvasItemDTO = _context.CanvasItems.FirstOrDefault(item => item.Id == canvasItemId);

                if (scheme != null && scheme.CanvasItems != null && canvasItemDTO != null)
                {
                    _context.CanvasItems.Remove(canvasItemDTO);
                    scheme.CanvasItems.Remove(canvasItemDTO);
                    _context.SaveChanges();
                }

                return canvasItemDTO;
            }
        }

        public ConnectionDTO? RemoveConnection(Guid schemeId, Guid connectionId)
        {
            using (_context)
            {
                SchemeDTO? scheme = _context.Schemes.Include(c => c.Connections).FirstOrDefault(item => item.Id == schemeId);
                ConnectionDTO? connectionDTO = _context.Connections.FirstOrDefault(item => item.Id == connectionId);

                if (scheme != null && scheme.Connections != null && connectionDTO != null)
                {
                    _context.Connections.Remove(connectionDTO);
                    scheme.Connections.Remove(connectionDTO);
                    _context.SaveChanges();
                }

                return connectionDTO;
            }
        }

        public List<CanvasItem> LoadCanvasItems(Guid schemeId)
        {
            using (_context)
            {
                List<CanvasItem> canvasItems = new List<CanvasItem>();
                List<CanvasItemDTO> canvasItemDTOs = _context.CanvasItems.Where(c => c.SchemeId == schemeId).ToList();

                if(canvasItemDTOs != null && canvasItemDTOs.Count >= 0)
                {
                    foreach(var canvasItemDTO in canvasItemDTOs)
                    {
                        CanvasItem? canvasItem = CanvasItemService.FromCanvasItemDTOToCanvasItem(canvasItemDTO);
                        if(canvasItem != null)
                        {
                            canvasItems.Add(canvasItem);
                        }
                    }
                }

                return canvasItems;
            }
        }

        public List<Connection> LoadConnection(Guid schemeId, List<CanvasItem> canvasItems)
        {
            using (_context)
            {
                List<Connection> connections = new List<Connection>();
                List<ConnectionDTO> connectionDTOs = _context.Connections.Where(c => c.SchemeId == schemeId).ToList();

                if(connectionDTOs != null && connectionDTOs.Count >= 0)
                {
                    foreach(var connectioDTO in connectionDTOs)
                    {
                        Connection? connection = ConnectionService.FromConnectionDTOToConnectio(connectioDTO, canvasItems);
                        if(connection != null)
                        {
                            connections.Add(connection);
                        }
                    }
                }
                return connections;
            }
        }

        public void AddToContext(SchemeDTO schemeDTO)
        {
            using (_context)
            {
                _context.Schemes.Add(schemeDTO);
                _context.SaveChanges();
            }
        }

        public SchemeDTO? LoadFromContext(string name)
        {
            using (_context)
            {
                SchemeDTO? schemeDTO = _context.Schemes.FirstOrDefault(c => c.Name == name);
                return schemeDTO;
            }
        }

        public List<SchemeDTO> GetAll()
        {
            using (_context)
            {
                List<SchemeDTO> schemes = _context.Schemes.ToList();
                return schemes;
            }
        }

        public void DeleteScheme(Guid schemeId)
        {
            using (_context)
            {
                SchemeDTO? schemeDTO = _context.Schemes.FirstOrDefault(s => s.Id == schemeId);
                if(schemeDTO != null)
                {
                    _context.Schemes.Remove(schemeDTO);
                    _context.SaveChanges();
                }
            }
        }
    }
}
