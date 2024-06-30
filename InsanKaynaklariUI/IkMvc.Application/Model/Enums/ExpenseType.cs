using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IkMvc.Application.Model.Enums
{
    public enum ExpenseType
    {
        [Display(Name = "Yemek ve İçecek")]
        FoodAndBeverage,

        [Display(Name = "Ulaşım")]
        Transportation,

        [Display(Name = "Konaklama")]
        Accommodation,

        [Display(Name = "Eğitim ve Etkinlik")]
        EducationAndEvent,

        [Display(Name = "Ofis Malzemeleri")]
        OfficeSupplies,

        [Display(Name = "Bakım ve Onarım")]
        MaintenanceAndRepairs,

        [Display(Name = "Hediyeler ve İkramlar")]
        GiftsAndEntertainment,

        [Display(Name = "Diğer")]
        Other
    }
}
