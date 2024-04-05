using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;
using MusicSystem.Data.Entities;
using MusicSystem.Dtos;
using MusicSystem.Helper;
using MusicSystem.Repository.IRpository;
using System.Data;

namespace MusicSystem.Controllers
{
    public class PurchaseDetailsController : Controller
    {
        private readonly DataContext _context;
        private readonly IComboHelper _comboHelper;
        private readonly IUserRepository _userRepository;

        public PurchaseDetailsController(DataContext context, IComboHelper comboHelper, IUserRepository userRepository)
        {
            _context = context;
            _comboHelper = comboHelper;
            _userRepository = userRepository;
        }

        // GET: PurchaseDetails
        public async Task<IActionResult> Index()
        {
            User user = await _userRepository.GetUserAsync(User.Identity.Name);

            if (user.UserType.ToString() == "User")
            {
                return View(await _context.PurchaseDetails
                .Include(u => u.User)
                .Include(a => a.AlbumSet)
                .ThenInclude(s => s.SongSets)
                .Where(u => u.User.Document == user.Document)
                .ToListAsync());

            }


            return View(await _context.PurchaseDetails
                .Include(u => u.User)
                .Include(a => a.AlbumSet)
                .ThenInclude(s => s.SongSets)
                .ToListAsync());
        }

        // GET: PurchaseDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseDetail = await _context.PurchaseDetails
                .Include(u => u.User)
                .Include(a => a.AlbumSet)
                .ThenInclude(s => s.SongSets)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseDetail == null)
            {
                return NotFound();
            }

            return View(purchaseDetail);
        }

        // GET: PurchaseDetails/Create
        public IActionResult Create()
        {
            PurchaseDetailDto model = new PurchaseDetailDto
            {
                Albumes = _comboHelper.GetComboAlbumesAsync(),
            };
            return View(model);
        }

        // POST: PurchaseDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseDetailDto purchaseDetailDto)
        {

            try
            {
                PurchaseDetail purchase = new PurchaseDetail
                {
                    Id = purchaseDetailDto.Id,
                    Total = purchaseDetailDto.Total,
                    AlbumSet = await _context.AlbumSets.FindAsync(purchaseDetailDto.AlbumSetId),
                    User = await _userRepository.GetUserAsync(User.Identity.Name)

                };
                _context.PurchaseDetails.Add(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException is SqlException sqlException)
                {
                    ModelState.AddModelError("ErrorGeneralGuardando", dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("ErrorGeneral", exception.Message);
            }
            return View(purchaseDetailDto);
        }

        // GET: PurchaseDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseDetail = await _context.PurchaseDetails
                 .Include(u => u.User)
                .Include(a => a.AlbumSet)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (purchaseDetail == null)
            {
                return NotFound();
            }
            PurchaseDetailDto model = new PurchaseDetailDto
            {
                Id = purchaseDetail.Id,
                Total = purchaseDetail.Total,
                AlbumSetId = purchaseDetail.AlbumSet.Id,
                Albumes = _comboHelper.GetComboAlbumesAsync(),
            };
            return View(model);
        }

        // POST: PurchaseDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseDetailDto purchaseDetailDto)
        {
            if (id != purchaseDetailDto.Id)
            {
                return NotFound();
            }

            try
            {
                PurchaseDetail purchase = new PurchaseDetail
                {
                    Id = purchaseDetailDto.Id,
                    Total = purchaseDetailDto.Total,
                    AlbumSet = await _context.AlbumSets.FindAsync(purchaseDetailDto.AlbumSetId),
                    User = await _userRepository.GetUserAsync(User.Identity.Name)

                };
                _context.PurchaseDetails.Update(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException is SqlException sqlException)
                {
                    ModelState.AddModelError("ErrorGeneralGuardando", dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("ErrorGeneral", exception.Message);
            }
            return View(purchaseDetailDto);
        }

        // GET: PurchaseDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseDetail = await _context.PurchaseDetails
                 .Include(u => u.User)
                .Include(a => a.AlbumSet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseDetail == null)
            {
                return NotFound();
            }

            return View(purchaseDetail);
        }

        // POST: PurchaseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseDetail = await _context.PurchaseDetails.FindAsync(id);
            if (purchaseDetail != null)
            {
                _context.PurchaseDetails.Remove(purchaseDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseDetailExists(int id)
        {
            return _context.PurchaseDetails.Any(e => e.Id == id);
        }

    }
}
