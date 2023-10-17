using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatTreeViewExample {
    public class MoveExplorerVM {
        private readonly ReadOnlyObservableCollection<MoveViewModel> _moves;
        public ReadOnlyObservableCollection<MoveViewModel> Moves => _moves;

        public MoveExplorerVM(MoveService service) {
            // You can ignore the service's implementation, it's just for demo purposes.
            service.Moves.Connect()
                .Transform(x => new MoveViewModel(x))
                .Bind(out _moves)
                .DisposeMany()
                .Subscribe();
        }
    }

    public class MoveViewModel {
        private readonly ReadOnlyObservableCollection<MoveViewModel> _children;
        public ReadOnlyObservableCollection<MoveViewModel> Children => _children;

        public string Text { get; }

        public MoveViewModel(MoveModel model) {
            model.Children.Connect()
               .Transform(x => new MoveViewModel(x))
               .Bind(out _children)
               .DisposeMany()
               .Subscribe();

            Text = model.IsWhiteMove ? $"{model.FullMoveNumber}. {model.Text}" : model.Text;
        }
    }
}
