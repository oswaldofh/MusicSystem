using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;
using MusicSystem.Data.Entities;

namespace MusicSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AlbumSetsController : Controller
    {
        private readonly DataContext _context;

        public AlbumSetsController(DataContext context)
        {
            _context = context;
        }

        // GET: AlbumSets
        //RETORNA TODOS LOS REGISTROS GUARDADOS EN LA BASE DE DATOS
        public async Task<IActionResult> Index()
        {
            var albums = await _context.AlbumSets.FromSql($"EXEC dbo.GetAlbumSets").ToListAsync();
           return View(albums);
        }

        // GET: AlbumSets/Details/5
        //RETORNA UN REGISTRO PASANDO EL ID PPR PARAMETRO
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            


            var albumSet = await _context.AlbumSets.FirstOrDefaultAsync(m => m.Id == id);
            if (albumSet == null)
            {
                return NotFound();
            }

            return View(albumSet);
        }

        // GET: AlbumSets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AlbumSets/Create
        //SE CREA UN REGISTRO PASANDO EL MODELO DE LA ENTIDAD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] AlbumSet albumSet)
        {

           
                try
                {
                    _context.AlbumSets.Add(albumSet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException is SqlException sqlException && sqlException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un álbum con ese nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorGeneralGuardando", dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError("ErrorGeneral", exception.Message);
                }
            
            return View(albumSet);
        }

        // GET: AlbumSets/Edit/5
        //SE CONSULTA EL REGISTRO A MODIFICAR
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumSet = await _context.AlbumSets.FindAsync(id);
            if (albumSet == null)
            {
                return NotFound();
            }
            return View(albumSet);
        }

        // POST: AlbumSets/Edit/5
        //SE MODIFICA UN REGISTRO PASANDO EL MODELO DE LA ENTIDAD Y EL ID POR PARAMETRO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] AlbumSet albumSet)
        {
            if (id != albumSet.Id)
            {
                return NotFound();
            }

                try
                {
                    _context.AlbumSets.Update(albumSet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException is SqlException sqlException && sqlException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un album con ese nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError("GeneralError", dbUpdateException.InnerException.Message);
                    }
                }
            

            return View(albumSet);
        }

        // GET: AlbumSets/Delete/5
        //SE CONSULTA EL REGISTRO A ELIMINAR PASANDO EL ID POR PARAMETRO
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumSet = await _context.AlbumSets.FindAsync(id);
            if (albumSet == null)
            {
                return NotFound();
            }

            return View(albumSet);
        }

        // POST: AlbumSets/Delete/5
        //SE ELIMINA EL REGISTRO PASANDO EL ID POR PARAMETRO
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var albumSet = await _context.AlbumSets.FindAsync(id);

            if (albumSet != null)
            {
                _context.AlbumSets.Remove(albumSet);

            }
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumSetExists(int id)
        {
            return _context.AlbumSets.Any(e => e.Id == id);
        }
    }
}
