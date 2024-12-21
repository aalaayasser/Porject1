using DALProject.Models;

namespace PLProj.Models
{
    public class KilometresViewModel
    {
        public int Id { get; set; }
        public long kiloMetres { get; set; }


        public static explicit operator KilometresViewModel(KiloMetres Model)
        {
            return new KilometresViewModel
            {
                Id = Model.Id,
                kiloMetres = Model.kiloMetres,
            };
        }
        public static explicit operator KiloMetres(KilometresViewModel viewModel)
        {
            return new KiloMetres
            {
                Id = viewModel.Id,
                kiloMetres = viewModel.kiloMetres,
                
            };
        }

    }
}
