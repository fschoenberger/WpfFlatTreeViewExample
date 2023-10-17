using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlatTreeViewExample {
    /// <summary>
    /// Interaction logic for MoveExplorer.xaml
    /// </summary>
    public partial class MoveExplorer : UserControl {
        public MoveExplorer() {
            InitializeComponent();

            var service = new MoveService();
            var vm = new MoveExplorerVM(service);

            this.DataContext = vm;
        }
    }
}
