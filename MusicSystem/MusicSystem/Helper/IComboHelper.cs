using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicSystem.Helper
{
    public interface IComboHelper
    {
        IEnumerable<SelectListItem> GetComboAlbumesAsync();
    }
}
