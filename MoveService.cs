using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FlatTreeViewExample {
    public class MoveService {
        private const string moves = """
        1. e4 c6 
        2. d4 d5 
        3. e5 c5 
        4. c3 Nc6 
            5. Nf3 cxd4 
            6. cxd4 Bg4 

            5. f4 cxd4 
            6. cxd4 h5 
            7. Nf3 Bg4 
            8. Be2 e6 
            9. O-O Nh6 

            5. Bb5 Qa5 
            6. Bxc6+ bxc6 
            7. Bd2 Qb6 

            5. Be3 Nh6 
            6. Bxh6 gxh6 
            7. Nf3 Bg4 
            8. Be2 Bxf3 
            9. Bxf3 e6 
            10. O-O cxd4 
            11. cxd4 Qb6 
        """;

        private readonly ISourceCache<MoveModel, int> _cache = new SourceCache<MoveModel, int>(x => x.Id);
        public IObservableCache<MoveModel, int> Moves => _cache;

        public MoveService() {
            var lines = moves.Split("\n").Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x));

            var moveStack = new Stack<MoveModel>();

            var currentMove = 1;
            var moveId = 0;

            foreach (var line in lines) {
                var (moveNumber, whiteMove, blackMove) = ParseMove(line);

                if (currentMove != moveNumber) {
                    for (var i = currentMove; i != moveNumber; --i) {
                        moveStack.Pop();
                        moveStack.Pop();
                    }
                }

                var whiteMoveModel = new MoveModel { Id = moveId++, FullMoveNumber = moveNumber, IsWhiteMove = true, Text = whiteMove, Parent = moveStack.Count > 0 ? moveStack.Peek() : null };
                var blackMoveModel = new MoveModel { Id = moveId++, FullMoveNumber = moveNumber, IsWhiteMove = false, Text = blackMove, Parent = whiteMoveModel };

                if (moveStack.Count > 0) {
                    moveStack.Peek().Children.AddOrUpdate(whiteMoveModel);
                } else {
                    _cache.AddOrUpdate(whiteMoveModel);
                }

                whiteMoveModel.Children.AddOrUpdate(blackMoveModel);

                moveStack.Push(whiteMoveModel);
                moveStack.Push(blackMoveModel);
                currentMove = moveNumber + 1;
            }


        }

        private (int moveNumber, string whiteMove, string blackMove) ParseMove(string line) {
            // Match line against a regex
            var components = line.Split(" ");
            var moveNumber = int.Parse(components[0].Trim('.'));
            var whiteMove = components[1];
            var blackMove = components[2];

            return (moveNumber, whiteMove, blackMove);
        }
    }

    public class MoveModel {
        public int Id { get; set; }

        public string Text { get; set; }

        public int FullMoveNumber { get; set; }

        public bool IsWhiteMove { get; set; }

        public MoveModel? Parent { get; set; }

        private readonly ISourceCache<MoveModel, int> _children = new SourceCache<MoveModel, int>(x => x.Id);
        public ISourceCache<MoveModel, int> Children => _children;
    }
}
