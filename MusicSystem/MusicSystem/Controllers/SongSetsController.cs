using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;
using MusicSystem.Data.Entities;
using MusicSystem.Dtos;
using MusicSystem.Helper;

namespace MusicSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SongSetsController : Controller
    {
        private readonly DataContext _context;
        private readonly IComboHelper _comboHelper;

        public SongSetsController(DataContext context, IComboHelper comboHelper)
        {
            _context = context;
            _comboHelper = comboHelper;

        }

        // GET: SongSets
        public async Task<IActionResult> Index()
        {
            var songs = await _context.SongSets
                .Include(a => a.AlbumSet)
                .ToListAsync();
            return View(songs);
        }


        // GET: SongSets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songSet = await _context.SongSets.Include(a => a.AlbumSet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (songSet == null)
            {
                return NotFound();
            }

            return View(songSet);
        }

        // GET: SongSets/Create
        public  IActionResult Create()
        {
            SongSetDto model = new SongSetDto
            {
                Albumes = _comboHelper.GetComboAlbumesAsync(),
            };
            return View(model);
        }

        // POST: SongSets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SongSetDto songSetDto)
        {
           

                try
                {
                    SongSet songSet = new SongSet
                    {
                        Id = songSetDto.Id,
                        Name = songSetDto.Name,
                        AlbumSet = await _context.AlbumSets.FindAsync(songSetDto.AlbumSetId)

                    };
                    _context.SongSets.Add(songSet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException is SqlException sqlException && sqlException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, $"Ya existe una cancion con el nombre {songSetDto.Name} en el álbum");
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
            
            return View(songSetDto);
        }

        // GET: SongSets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songSet = await _context.SongSets
                .Include(a => a.AlbumSet)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (songSet == null)
            {
                return NotFound();
            }

            SongSetDto model = new SongSetDto
            {
                Id = songSet.Id,
                Name = songSet.Name,
                AlbumSetId = songSet.AlbumSet.Id,
                Albumes = _comboHelper.GetComboAlbumesAsync(),
            };
            return View(model);
        }

        // POST: SongSets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SongSetDto songSetDto)
        {
            if (id != songSetDto.Id)
            {
                return NotFound();
            }


                try
                {
                    SongSet songSet = new SongSet
                    {
                        Id = songSetDto.Id,
                        Name = songSetDto.Name,
                        AlbumSet = await _context.AlbumSets.FindAsync(songSetDto.AlbumSetId)

                    };
                    _context.SongSets.Update(songSet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException is SqlException sqlException && sqlException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, $"Ya existe una cancion con el nombre {songSetDto.Name} en el álbum");
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
            
            return View(songSetDto);
        }

        // GET: SongSets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songSet = await _context.SongSets.Include(a => a.AlbumSet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (songSet == null)
            {
                return NotFound();
            }

            return View(songSet);
        }

        // POST: SongSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var songSet = await _context.SongSets.FindAsync(id);
            if (songSet != null)
            {
                _context.SongSets.Remove(songSet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongSetExists(int id)
        {
            return _context.SongSets.Any(e => e.Id == id);
        }
    }
}
