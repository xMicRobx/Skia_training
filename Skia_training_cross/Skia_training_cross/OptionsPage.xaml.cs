using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Skia_training_cross
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeatsPage : ContentPage
    {
        public SeatsPage(Choice choice)
        {
            InitializeComponent();
            SelectedChoice = choice;
            this.BindingContext = this;
        }
        public Choice SelectedChoice { get; set; }
    }
}