using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;

namespace MusicSystem.Helper
{
    public class ComboHelper : IComboHelper
    {
        private readonly DataContext _context;
        public ComboHelper(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetComboAlbumesAsync()
        {
            List<SelectListItem> list = _context.AlbumSets.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString(),


            }).OrderBy(a => a.Text)
                .ToList();
            list.Insert(0, new SelectListItem { Text = "Seleccione un album...", Value = "0" });


            return list;
        }
    }
}
